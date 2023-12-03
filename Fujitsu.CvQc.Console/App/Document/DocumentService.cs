using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Newtonsoft.Json;

namespace Fujitsu.CvQc.Console.App
{
    public class DocumentService : IDocumentService
    {
        private IJobDataService jobDataService = ServiceInjector.GetService<IJobDataService>();
        private ITransformationService transformationService = ServiceInjector.GetService<ITransformationService>();

        public ICollection<DocumentFileModel> GetDocumentFiles(string directoryPath)
        {
            var docxFilesList = new List<DocumentFileModel>();
            try
            {
                var docxFilesPaths = Directory.EnumerateFiles(directoryPath, "*.docx", SearchOption.AllDirectories);

                if (!docxFilesPaths.Any())
                {
                    jobDataService.Log($"No files found: {directoryPath}");
                }
                else
                {
                    foreach (string filePath in docxFilesPaths)
                    {
                        docxFilesList.Add(GetDocumentFile(filePath));
                        Thread.Sleep(1000);
                    }
                }
            }
            catch (FileNotFoundException e)
            {
                jobDataService.Log($"Unable to extract files: path: {directoryPath}, exception: {e.Message}");
                throw;
            }

            return docxFilesList;
        }

        public DocumentFileModel GetDocumentFile(string filePath)
        {
            var document = WordprocessingDocument.Open(filePath, true);

            if (document == null)
            {
                jobDataService.Log($"No file found: {filePath}");
                throw new Exception("Unable to open Word document");
            }

            var fileName = Path.GetFileName(filePath);
            jobDataService.Log($"File found: {fileName}");

            return new DocumentFileModel(fileName, document);
        }

        public Rule LoadRules(string rulesStr)
        {
            Rule? rules = JsonConvert.DeserializeObject<Rule>(rulesStr);

            if (rules == null)
            {
                throw new Exception("Unable to load rules");
            }

            return rules;
        }

        public DocumentModel GetDocumentMapping(Rule rules, DocumentFileModel documentFile)
        {
            DocumentModel documentModel = new DocumentModel();
            documentModel.FileName = documentFile.FileName;
            documentModel.DocumentMap = GetDocumentMapping(rules, documentFile.FileContent);
            return documentModel;
        }

        private DocumentMappingModel MapDocument(List<string> paragraphs, Rule rules)
        {
            DocumentMappingModel documentMapping = new DocumentMappingModel();
            RuleSection? currentSection;
            DocumentMappingSectionModel? currentDocumentMappingSection = null;

            foreach (var _paragraph in paragraphs)
            {
                string paragraph = NormalizeString(_paragraph);
                bool paragraphMatched = false;

                if (rules.sections != null)
                {

                    foreach (var section in rules.sections)
                    {
                        if (section.matches != null)
                        {

                            foreach (var _match in section.matches)
                            {
                                string match = NormalizeString(_match);

                                if (match.ToLower() == paragraph.ToLower())
                                {
                                    paragraphMatched = true;
                                    currentSection = section;
                                    currentDocumentMappingSection = null;

                                    foreach (var documentMappingSection in documentMapping.Sections)
                                    {
                                        if (documentMappingSection.Target == currentSection.target && documentMappingSection.Match.ToLower() == match.ToLower())
                                        {
                                            currentDocumentMappingSection = documentMappingSection;
                                        }
                                    }

                                    if (currentDocumentMappingSection == null)
                                    {
                                        currentDocumentMappingSection = new DocumentMappingSectionModel(match, section.target, section.exportationMode);
                                        documentMapping.Sections.Add(currentDocumentMappingSection);
                                    }
                                }
                            }
                        }
                    }
                }

                if (!paragraphMatched && currentDocumentMappingSection != null)
                {
                    currentDocumentMappingSection.Paragraphs.Add(paragraph);
                }
            }
            return documentMapping;
        }

        private string NormalizeString(string oddString)
        {
            string nomalizedString =
                oddString
                    .Normalize()
                    .Replace("’", "'");

            return nomalizedString;
        }

        private List<string> ExtractParagraphs(Body documentBody)
        {
            var descendants = documentBody.Descendants<Paragraph>().Cast<OpenXmlElement>().ToList();
            var paragraphs = new List<string>();

            foreach (var descendant in descendants)
            {
                if (!string.IsNullOrEmpty(descendant.InnerText))
                {
                    paragraphs.Add(descendant.InnerText);
                }
            }

            return paragraphs;
        }

        private Body GetDocumentBody(WordprocessingDocument document)
        {
            var body = document?.MainDocumentPart?.Document.Body;

            if (body == null)
            {
                throw new Exception("Unable to get document body");
            }

            return body;
        }


    public void CreateDocumentFromTemplate(string templatePath, DocumentModel document, string targetPath) {
        var documentMapping = document.DocumentMap;
        using (Stream templateStream = File.OpenRead(templatePath)) {
            using (MemoryStream documentStream = new MemoryStream((int)templateStream.Length)) {

                byte[] buffer = new byte[2048];
                int length = buffer.Length, size;
                while ((size = templateStream.Read(buffer, 0, length)) != 0)
                {
                    documentStream.Write(buffer, 0, size);
                }
                documentStream.Position = 0;
                            
                using (WordprocessingDocument targetDocument = WordprocessingDocument.Open(documentStream, true)) {

                    targetDocument.ChangeDocumentType(WordprocessingDocumentType.Document);
                    var mainPart = targetDocument.MainDocumentPart;
                    var body = mainPart?.Document.Body;
                    var bodyRuns = body?.Descendants<Run>().Cast<OpenXmlElement>().ToList();

                    var runs = new List<OpenXmlElement>();
                    if (bodyRuns != null) {
                        runs.AddRange(bodyRuns);
                    }

                    if (mainPart?.HeaderParts != null) {
                        foreach (var headerPart in mainPart.HeaderParts) {
                            var header = headerPart.Header;
                            var headerRuns = header?.Descendants<Run>().Cast<OpenXmlElement>().ToList();
                                                
                            if (headerRuns != null) {
                                runs.AddRange(headerRuns);    
                            }           
                        }
                    }

                    if (runs != null && documentMapping != null) {

                        foreach(var section in documentMapping.Sections)                        
                        {                         
                            var nodes = runs.Where(r => r.InnerText.ToLower() == section.Target.ToLower()).ToList();                            

                            if (nodes != null && nodes.Count!=0) {

                                if (section.ExportationMode != ExportationMode.Custom)
                                {
                                    //Si on trouver les noeuds contenant le targetText
                                    //alors on replace le contenu des noeuds cibles par celui de section(même contenu partout)
                                    foreach (var node in nodes) {
                                        ExportSection(section, node);
                                    }
                                }
                                else
                                {
                                    //Si on trouver les noeuds contenant le targetText
                                    //alors on replace le contenu de chaque noeud cible par celui de paragraph précis de la section conserné
                                    if (body != null )
                                    {
                                        AjouterLignes(section, nodes, body);
                                        ExportSection(section, nodes, body);
                                        SupprimerLignes(section, nodes, body);
                                    }

                                }

                            }
                            
                        }
                    }

                    // TODO: Add suffix to converted file name
                    targetDocument.Clone(targetPath + @"/" + document.FileName);
                }
            }
        }
    }

        // public void CreateDocumentFromTemplate(string templatePath, DocumentModel document, string targetPath)
        // {
        //     var documentMapping = document.DocumentMap;
        //     using (Stream templateStream = File.OpenRead(templatePath))
        //     {
        //         using (MemoryStream documentStream = new MemoryStream((int)templateStream.Length))
        //         {
        //             byte[] buffer = new byte[2048];
        //             int length = buffer.Length, size;
        //             while ((size = templateStream.Read(buffer, 0, length)) != 0)
        //             {
        //                 documentStream.Write(buffer, 0, size);
        //             }
        //             documentStream.Position = 0;

        //             using (WordprocessingDocument targetDocument = WordprocessingDocument.Open(documentStream, true))
        //             {
        //                 targetDocument.ChangeDocumentType(WordprocessingDocumentType.Document);
        //                 var mainPart = targetDocument.MainDocumentPart;
        //                 var body = mainPart?.Document.Body;
        //                 var bodyRuns = body?.Descendants<Run>().Cast<OpenXmlElement>().ToList();

        //                 var runs = new List<OpenXmlElement>();
        //                 if (bodyRuns != null)
        //                 {
        //                     runs.AddRange(bodyRuns);
        //                 }

        //                 if (mainPart?.HeaderParts != null)
        //                 {
        //                     foreach (var headerPart in mainPart.HeaderParts)
        //                     {
        //                         var header = headerPart.Header;
        //                         var headerRuns = header?.Descendants<Run>().Cast<OpenXmlElement>().ToList();

        //                         if (headerRuns != null)
        //                         {
        //                             runs.AddRange(headerRuns);
        //                         }
        //                     }
        //                 }
                        

        //                 if (runs != null && documentMapping != null)
        //                 {
        //                     foreach (var section in documentMapping.Sections)
        //                     {
        //                         var nodes = runs.Where(r => r.InnerText.ToLower() == section.Target.ToLower()).ToList();

        //                         if (nodes != null)
        //                         {
        //                             foreach (var node in nodes)
        //                             {
        //                                 ExportSection(section, node);
        //                             }
        //                         }
        //                     }
        //                 }
                        
        //                 try
        //                 {
        //                     if (!Directory.Exists(targetPath))
        //                         Directory.CreateDirectory(targetPath);

        //                     // TODO: Add suffix to converted file name
        //                     targetDocument.Clone(targetPath + @"/" + document.FileName);
        //                 }
        //                 catch (Exception)
        //                 {
        //                     System.Console.WriteLine("Unable to save the file to " + targetPath);

        //                     logService.Log("Unable to save the file to " + targetPath);
        //                 }
                        
        //             }
        //         }
        //     }
        // }

        private void ExportSection(DocumentMappingSectionModel section, OpenXmlElement node)
        {
            node.RemoveAllChildren<Text>();
            switch (section.ExportationMode)
            {
                case ExportationMode.Text:
                    transformationService.Transform_Text(section, node);
                    break;
                case ExportationMode.Paragraph:
                    transformationService.Transform_Paragraph(section, node);                    
                    break;
            }
        }

        private DocumentMappingModel GetDocumentMapping(Rule rules, WordprocessingDocument documentFile)
        {
            var body = GetDocumentBody(documentFile);
            var paragraphs = ExtractParagraphs(body);
            var documentMappingModel = MapDocument(paragraphs, rules);
            return documentMappingModel;
        }

        private void ExportSection(DocumentMappingSectionModel section, List<OpenXmlElement>  nodes, Body body) {

            if (nodes != null && section.Paragraphs != null && body != null)
            {

                if (section.Target.ToLower() == "_REALISATIONS_GRID_".ToLower())
                {
                    List<GridRealisationRow> gridRealisationRows = ExtractGrilles(section.Paragraphs);
                    string CurrentEntrepriseName = "EntrepriseName";
                    string marqueur = "Employeur";
                    string marqueurTableau = "GRILLES DE RÉALISATIONS PROFESSIONNELLES";

                    var tableau = body.Descendants<Table>().FirstOrDefault(t =>
                        t.Descendants<TableRow>().FirstOrDefault()?.InnerText.ToLower() == marqueurTableau.ToLower()
                    );

                    for (int i = 0; i < gridRealisationRows.Count; i += 1)
                    {
                        if (CurrentEntrepriseName.ToLower() != gridRealisationRows[i].EntrepriseName.ToLower())
                        {
                            CurrentEntrepriseName = gridRealisationRows[i].EntrepriseName;

                            // Create 1 row to the table.
                            TableRow? currentTableRow = tableau?.Descendants<TableRow>().LastOrDefault(tr => tr.InnerText.ToLower() == marqueur.ToLower())?.Clone() as TableRow ?? new TableRow();
                            var currentCell = currentTableRow?.Descendants<TableCell>()?.FirstOrDefault(tc => tc.InnerText.ToLower() == marqueur.ToLower());
                            var currentParagraph = currentCell?.Descendants<Paragraph>()?.FirstOrDefault(p => p.InnerText.ToLower() == marqueur.ToLower());

                            var runs = currentParagraph?.Descendants<Run>().Where(el => el.InnerText.Contains(marqueur));
                            if (runs != null)
                            {
                                foreach (Run run in runs)
                                {
                                    //string innerText = run.InnerText.Replace(marqueur, CurrentEntrepriseName);
                                    run.RemoveAllChildren<Text>();
                                    run.AppendChild(new Text(CurrentEntrepriseName));
                                }
                            }
                            tableau?.AppendChild(currentTableRow);
                        }

                        TableRow? tr2 = tableau?.Descendants<TableRow>().FirstOrDefault(
                                tr => tr.Descendants<TableCell>()?.FirstOrDefault(rc => rc.InnerText.ToLower() == section.Target.ToLower())?.ToList().Count() > 0
                                )?.Clone() as TableRow ?? new TableRow();
                        tableau?.AppendChild(tr2);
                        var runS = tr2.Descendants<Run>().Cast<OpenXmlElement>().ToList();
                        runS[0].RemoveAllChildren<Text>();
                            runS[0].AppendChild(new Text(gridRealisationRows[i].NumeroDuMandat));
                            //transformationService.Transform_Custom(gridRealisationRows[i].NumeroDuMandat, runS[0]);
                        runS[1].RemoveAllChildren<Text>();
                            runS[1].AppendChild(new Text(gridRealisationRows[i].Employeur));
                        runS[2].RemoveAllChildren<Text>();
                            runS[2].AppendChild(new Text(gridRealisationRows[i].TitreProjetMandat));
                        runS[3].RemoveAllChildren<Text>();
                            runS[3].AppendChild(new Text(gridRealisationRows[i].Periode));
                        runS[4].RemoveAllChildren<Text>();
                            runS[4].AppendChild(new Text(gridRealisationRows[i].TypeIntervention));
                        //runS[5].RemoveAllChildren<Text>();
                            //transformationService.Transform_Custom(gridRealisationRows[i].ReferenceEtTelephone, runS[5]);
                            //runS[5].AppendChild(new Text(gridRealisationRows[i].ReferenceEtTelephone));
                        runS[5].RemoveAllChildren<Text>();
                            runS[5].AppendChild(new Text(gridRealisationRows[i].EffortDuConseiller));
                        runS[6].RemoveAllChildren<Text>();
                            runS[6].AppendChild(new Text(gridRealisationRows[i].EnvergueDuProjet));
                    }

                    while (
                        tableau?.Descendants<TableRow>().Where(
                                tr => tr.Descendants<TableCell>()?.FirstOrDefault(rc => rc.InnerText.ToLower() == section.Target.ToLower())?.ToList().Count() > 0
                        ).ToList().Count > 0
                    )
                    {
                        tableau?.Descendants<TableRow>().FirstOrDefault(tr => tr.InnerText.ToLower() == marqueur.ToLower())?.Remove();

                        tableau?.Descendants<TableRow>().FirstOrDefault(
                                tr => tr.Descendants<TableCell>()?.FirstOrDefault(rc => rc.InnerText.ToLower() == section.Target.ToLower())?.ToList().Count() > 0
                        )?.Remove();
                    }

                }
                else 
                {
                    if (section.Target.ToLower() == "_REALISATIONS_".ToLower())
                    {
                        List<RealisationRow> realisationRows = ExtractRealisations(section.Paragraphs);

                        int c = 0;
                        foreach(var realisationRow in realisationRows)
                        {
                            var tableau = body.Descendants<Table>().FirstOrDefault(t =>
                                                //t.InnerText.ToLower().Contains("Nº".ToLower())
                                                //|| 
                                                t.InnerText.ToLower().Contains("No.".ToLower())
                                        )?.Clone() as Table;
                            //var runS = tableau.Descendants<Run>().Cast<OpenXmlElement>().ToList();
                            var runS = tableau?.Descendants<Run>().ToList();
                            var nb = runS?.Count;
                            ChangeText( runS?.FirstOrDefault(r => r.InnerText.ToLower() == "NomPrecEmployeur".ToLower()), realisationRow.NomPrecEmployeur,"NomPrecEmployeur");
                            ChangeText( runS?.FirstOrDefault(r => r.InnerText.ToLower() == "DureePrecEmployeur".ToLower()), realisationRow.DureePrecEmployeur, "DureePrecEmployeur");
                            ChangeText( runS?.FirstOrDefault(r => r.InnerText.ToLower() == "NumeroDuMandat".ToLower()), realisationRow.NumeroDuMandat, "NumeroDuMandat");
                            ChangeText( runS?.FirstOrDefault(r => r.InnerText.ToLower() == "RoleDuMandat".ToLower()), realisationRow.RoleDuMandat, "RoleDuMandat");
                            ChangeText( runS?.FirstOrDefault(r => r.InnerText.ToLower() == "DureeDuMandat".ToLower()), realisationRow.DureeDuMandat, "DureeDuMandat");
                            ChangeText( runS?.FirstOrDefault(r => r.InnerText.ToLower() == "NomClient".ToLower()), realisationRow.NomClient, "NomClient");
                            ChangeText( runS?.FirstOrDefault(r => r.InnerText.ToLower() == "DateDebutADateFin".ToLower()), realisationRow.DateDebutADateFin, "DateDebutADateFin");
                            ChangeText( runS?.FirstOrDefault(r => r.InnerText.ToLower() == "NomProjet".ToLower()), realisationRow.NomProjet, "NomProjet");
                            ChangeText( runS?.FirstOrDefault(r => r.InnerText.ToLower() == "NomContactClient".ToLower()), realisationRow.NomContactClient, "NomContactClient");
                            ChangeText( runS?.FirstOrDefault(r => r.InnerText.ToLower() == "TelephoneCourrielContactClient".ToLower()), realisationRow.TelephoneCourrielContactClient, "TelephoneCourrielContactClient");
                            ChangeText( runS?.FirstOrDefault(r => r.InnerText.ToLower() == "Environnement".ToLower()), realisationRow.Environnement, "Environnement");
                            ChangeText( runS?.FirstOrDefault(r => r.InnerText.ToLower() == "Methodologies".ToLower()), realisationRow.Methodologies, "Methodologies");
                            ChangeText( runS?.FirstOrDefault(r => r.InnerText.ToLower() == "DescriptionProjet".ToLower()), realisationRow.DescriptionProjet, "DescriptionProjet");
                            ChangeText( runS?.FirstOrDefault(r => r.InnerText.ToLower() == "DescriptionRole".ToLower()), realisationRow.DescriptionRole, "DescriptionRole");
                            ChangeText( runS?.FirstOrDefault(r => r.InnerText.ToLower() == "Realisation01".ToLower()), realisationRow?.Realisations?.ToString() ?? "", "Realisation01");


                            c++;
                            body.AppendChild(tableau);
                            body.AppendChild(new Paragraph((new Run(new Text(" ")))));
                        }

                    }
                    else
                    {
                        foreach (var paragraph in section.Paragraphs)
                        {
                            var node = nodes.FirstOrDefault(r => r.InnerText.ToLower() == section.Target.ToLower());

                            if (node != null)
                            {
                                // la transformation doit être propre à un type pour respecter la mise en forme défini par le besoin... pour plus tard
                                // pour le moment on copie colle juste le contenu de départ
                                node.RemoveAllChildren<Text>();
                                transformationService.Transform_Custom(paragraph, node);
                            }
                        }

                    }

                }

            }


            System.Console.WriteLine("Transform : " + section.Target);
        }
        
        private void ChangeText(Run? run, string text, string marqueur)
        {
            if(run!=null)
            {
                run.RemoveAllChildren<Text>();
                run.AppendChild(new Text(text));
            }

            System.Console.WriteLine("ChangeText : " + marqueur);
        }

        private void AjouterLignes(DocumentMappingSectionModel section, List<OpenXmlElement> nodes, Body body)
        {
            // _Employeur_
            if (nodes != null && section.Paragraphs != null && body != null)
            {

                if (
                    section.Target.ToLower() != "_REALISATIONS_".ToLower()
                    && section.Target.ToLower() != "_REALISATIONS_GRID_".ToLower()
                    )
                    while (section.Paragraphs.Count > nodes.Count)
                    {
                        var rr = body.LastOrDefault(ex => ex.InnerText == nodes.Last().InnerText);
                        var paragraphe = body.Descendants<Paragraph>().First(p => p?.LastChild?.InnerText == nodes.Last().InnerText);

                        //var ki = nodes.Last().InsertAfterSelf(new Run());
                        Paragraph p = new Paragraph();
                        Run runS = new Run();
                        runS.AppendChild(new Text(nodes.Last().InnerText));
                        runS.AppendChild(nodes.Last().GetFirstChild<RunProperties>());
                        // If the paragraph has no ParagraphProperties object, create one.
                        if (p.Elements<ParagraphProperties>().Count() == 0)
                        {
                            // Get the paragraph properties element of the paragraph.
                            p.PrependChild<ParagraphProperties>(new ParagraphProperties());

                            // Get a reference to the ParagraphProperties object.
                            ParagraphProperties? pPr = p.ParagraphProperties;

                            if (pPr != null) {
                                // If a ParagraphStyleId object doesn't exist, create one.
                                if (pPr.ParagraphStyleId == null)
                                    pPr.ParagraphStyleId = new ParagraphStyleId();

                                // Set the style of the paragraph.
                                pPr.ParagraphStyleId.Val = paragraphe?.ParagraphProperties?.ParagraphStyleId?.Val;                                
                            }
                        }

                        p.AppendChild(runS);
                        //paragraphe.AppendChild(runS);
                        body.InsertAfter(p, rr);
                        nodes.Add(runS);

                    }

                else if (section.Target.ToLower() == "_REALISATIONS_GRID_".ToLower())
                {

                }

            }


            System.Console.WriteLine("AjouterLignes : " + section.Target);
        }

        private void SupprimerLignes(DocumentMappingSectionModel section, List<OpenXmlElement> nodes, Body body)
        {
                //On vide les noeuds qui n'ont pas servir au cas ou il y avait plus de noeuds que de paragraphe
                var nodesRestants = nodes.Where(r => r.InnerText.ToLower() == section.Target.ToLower()).ToList();
                if (nodesRestants != null && nodesRestants.Count != 0)
                    foreach (var nodeRestant in nodesRestants)
                    {

                        var paragraphes = body.Descendants<Paragraph>().Where(
                            p => p.ChildElements.Where(
                                    e => e.InnerText == nodes.Last().InnerText
                                    ).ToList().Count != 0
                            ).ToList();
                        if (paragraphes != null && paragraphes.Count != 0)
                            foreach (var anana in paragraphes)
                                anana.Remove();

                        nodeRestant.RemoveAllChildren<Text>();
                        nodeRestant.RemoveAllChildren<RunProperties>();
                        nodeRestant.Remove();
                        var p = nodeRestant.Parent as Paragraph;
                        if (p is Paragraph)
                        {                            
                            if (p?.ParagraphProperties?.ParagraphStyleId != null)
                                p.ParagraphProperties.ParagraphStyleId.Val = "";

                            if (p != null) {
                                p.RemoveAllChildren();
                                p.Remove();
                            }
                        }

                    }

            System.Console.WriteLine("SupprimerLignes : " + section.Target);
        } 
        
    private List<GridRealisationRow> ExtractGrilles(List<string> paragraphs) {
        List<GridRealisationRow> gridRealisationRows = new List<GridRealisationRow>();
        int i = 8;// on saute la première ligne qui contient les entêtes
            int nombreRealisation = 0;//juste pour vérifier que tout se passe bien == 22?
            int nombreEntreprise = 0;//juste pour vérifier que tout se passe bien == 4 ?
        string entrepriseName = "";


        if (paragraphs != null && paragraphs.Count != 0)
            while (i < paragraphs.Count) 
            {
                if ((i+7) < paragraphs.Count)
                { 
                    int numeroDuMandat = 0;//Nombre
                    double effortDuConseiller = 0;//Nombre
                    int envergueDuProjet = 0;//Nombre

                    bool testNumeroDuMandat = int.TryParse(paragraphs[i],out numeroDuMandat);//Nombre
                    bool testEffortDuConseiller = double.TryParse(paragraphs[i], out effortDuConseiller);//Nombre
                    bool testEnvergueDuProjet = int.TryParse(paragraphs[i], out envergueDuProjet);//Nombre

                    // si on reussir à trouve une ligne ou le position est un num ainsi que (position+6) et (position+7) alors
                    if (testNumeroDuMandat && testEffortDuConseiller && testEnvergueDuProjet)
                    {
                        gridRealisationRows .Add( new GridRealisationRow
                            {
                                EntrepriseName = entrepriseName,
                                NumeroDuMandat = paragraphs[i],//Nombre
                                Employeur = paragraphs[i+1],
                                TitreProjetMandat = paragraphs[i + 2],
                                Periode = paragraphs[i + 3],
                                TypeIntervention = paragraphs[i + 4],
                                ReferenceEtTelephone = paragraphs[i + 5],
                                EffortDuConseiller = paragraphs[i + 6],//Nombre
                                EnvergueDuProjet = paragraphs[i + 7],//Nombre
                            }
                        );
                        nombreRealisation++;
                        i +=8;//Vérifier
                    }
                    else// c'est une nouvelle ligne de la meme entreprise sinon c'est une nouvelle entreprise
                    {
                        entrepriseName = paragraphs[i];
                        nombreEntreprise++;
                        i++;
                    }
                }
                else i++;

            }

        return gridRealisationRows;
    }
    
    private class RealisationRow
    {
        public string NomPrecEmployeur = "";//_NomPrecEmployeur_
        public string DureePrecEmployeur = "";//_DureePrecEmployeur_
        public string NomClient = "";//_NomClient_
        public string NomProjet = "";//_NomProjet_
        public string NumeroDuMandat = "";//_NumeroDuMandat_
        public string RoleDuMandat = "";//_RoleDuMandat_
        public string DureeDuMandat = "";//_DureeDuMandat_
        public string DateDebutADateFin = "";//_DateDebutADateFin_
        public string NomContactClient = "";//_NomContactClient_
        public string TelephoneCourrielContactClient = "";//_TelephoneCourrielContactClient_

        public string Environnement = "";//_ Environnement_
        public string Methodologies = "";//_Methodologies_
        public string DescriptionProjet = "";//_DescriptionProjet_
        public string DescriptionRole = "";//DescriptionRole

        public List<string> Realisations = new List<string>();//_Realisation_ Dans une realisation il peut y avoir plusieurs précision ?? à voir...
    }

  private List<RealisationRow> ExtractRealisations(List<string> paragraphs) {
        List<RealisationRow> gridRealisationRows = new List<RealisationRow>();
        int i = 0;// 
        RealisationRow realisationRow = new RealisationRow();
        realisationRow.NomPrecEmployeur = paragraphs[i];
        realisationRow.DureePrecEmployeur = paragraphs[i];
        i = i + 1;

        if (paragraphs != null && paragraphs.Count != 0)
            while (i < paragraphs.Count)
            {
                var p = paragraphs[i];
                if (
                    //paragraphs[i].Contains("No.")                    ||
                    paragraphs[i].Contains("Nº")
                    && ( (i + 5) < paragraphs.Count ) 
                    )
                {
                    realisationRow.NumeroDuMandat = paragraphs[i];
                    realisationRow.RoleDuMandat = paragraphs[i+1];
                    realisationRow.DureeDuMandat = paragraphs[i+2];
                    realisationRow.NomProjet = paragraphs[i+3];
                    realisationRow.DateDebutADateFin = paragraphs[i+4];
                    realisationRow.NomClient = paragraphs[i+5];
                    i+=6;
                }
                else
                {
                    if (
                            paragraphs[i].Contains("Environnement")
                        )
                    {
                        realisationRow.Environnement = paragraphs[i];
                        gridRealisationRows.Add(realisationRow);
                        //Si le prochain texte après Environnement c'est pas la new line N0 alors c'est le NomPrecEmployeur DureePrecEmployeur et il faut sauter deux lignes
                        realisationRow = new RealisationRow();
                        if ( (i + 1) < paragraphs.Count)
                        {
                            if (
                                !paragraphs[i+1].Contains("Nº")
                            )
                            {
                                i = i + 1;
                                realisationRow.NomPrecEmployeur = paragraphs[i];
                                realisationRow.DureePrecEmployeur = paragraphs[i];
                            }
                        }
                        i++;
                    }
                    else
                    {
                        realisationRow.DescriptionProjet += paragraphs[i];
                        realisationRow.DescriptionRole += paragraphs[i];
                        i++;
                    }
                }
                
            }

        return gridRealisationRows;
    }


        private class GridRealisationRow
        {
            public string EntrepriseName = "";
            public string NumeroDuMandat = "";//Nombre Int
            public string Employeur = "";
            public string TitreProjetMandat = "";
            public string Periode = "";
            public string TypeIntervention = "";
            public string ReferenceEtTelephone = "";
            public string EffortDuConseiller = "";//Nombre Double
            public string EnvergueDuProjet = "";//Nombre Int
        }        
    }
}

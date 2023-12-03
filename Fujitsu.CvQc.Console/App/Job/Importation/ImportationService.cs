namespace Fujitsu.CvQc.Console.App
{
    public class ImportationService : IImportationService
    {
        private IJobDataService jobDataService = ServiceInjector.GetService<IJobDataService>();
        private IDocumentService documentService = ServiceInjector.GetService<IDocumentService>();

        public void ProcessJob(JobModel job)
        {
            var project = jobDataService.GetProject(job);

            if (project != null) {
                var documentFiles = GetDocumentFiles(project);
                if (documentFiles.Any())
                {
                    var rules = documentService.LoadRules(project.Config.Rules);
                    project.Documents.Clear();
                    foreach (var documentFile in documentFiles)
                    {
                        var documentModel = documentService.GetDocumentMapping(rules, documentFile);
                        documentModel.ProjectId = project.Id;
                        project.Documents.Add(documentModel);                  
                    }
                    jobDataService.UpdateProject(project);
                }
            }    

            jobDataService.SetJobComplete(job);                    
        }

        private ICollection<DocumentFileModel> GetDocumentFiles(ProjectModel project) 
        {
            ICollection<DocumentFileModel> documentFiles = new List<DocumentFileModel>();

            switch (project.Config.ImportMode) 
            {
                case Constants.BatchImportMode:
                    System.Console.WriteLine("Import mode is batch");
                    documentFiles = documentService.GetDocumentFiles(project.Config.InputPath);                
                    break;

                case Constants.SingleImportMode:
                    System.Console.WriteLine("Import mode is single file");
                    documentFiles.Add(documentService.GetDocumentFile(project.Config.InputPath));                
                    break;

                default:
                    break;

            }

            return documentFiles;
        }

    }
}

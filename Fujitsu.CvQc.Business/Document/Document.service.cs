using Fujitsu.CvQc.Data;
using Fujitsu.CvQc.Data.Entities;

namespace Fujitsu.CvQc.Business;

public class DocumentService : IDocumentService
{
    private readonly IDocumentDataService documentDataService;

    public DocumentService(IDocumentDataService documentDataService)
    {
        this.documentDataService = documentDataService;
    }

    public DocumentModel? GetDocument(string id)
    {
        DocumentModel? model = null;

        var guid = new Guid(id);
        var entity = documentDataService.GetDocument(guid);
        if (entity != null)
        {
            model = MapToDocumentModel(entity);
        }

        return model;
    }

    public List<DocumentModel> GetDocuments(string projectId)
    {
        var models = new List<DocumentModel>();

        var guid = new Guid(projectId);
        var entities = documentDataService.GetProjectDocuments(guid);
        foreach (var entity in entities)
        {
            var model = MapToDocumentModel(entity);

            models.Add(model);
        } 

        return models;
    }

    #region Entities/Models mappers
    public DocumentMapping MapToDocumentMapping(DocumentMap mapping)
    {
        DocumentMapping result = new DocumentMapping();
        result.Sections = MapToDocumentMapSectionList(mapping.Sections);
        return result;
    }

    public IEnumerable<DocumentMapSectionModel> MapToDocumentMapSectionList(IEnumerable<DocumentMapSection> sectionList)
    {
        List<DocumentMapSectionModel> result = new List<DocumentMapSectionModel>();
        foreach (var section in sectionList)
        {
            result.Add(MapToDocumentMapSection(section));
        }

        return result;
    }

    public DocumentMapSectionModel MapToDocumentMapSection(DocumentMapSection section)
    {
        DocumentMapSectionModel result = new DocumentMapSectionModel();
        result.Paragraphs = section.Paragraphs.ToList().ConvertAll(x => x.Text).ToList();
        result.Target = section.Target;
        result.Match = section.Match;
        result.ExportationMode = section.ExportationMode;
        return result;
    }

    public DocumentModel MapToDocumentModel(Document document)
    {
        DocumentModel result = new DocumentModel();
        if (document.DocumentMap != null)
        {
            result.DocumentMap = MapToDocumentMapping(document.DocumentMap);
        }
        result.FileName = document.FileName;
        result.Importation = document.Importation;
        result.ProjectId = document.ProjectId.ToString();
        return result;
    }
    #endregion
}


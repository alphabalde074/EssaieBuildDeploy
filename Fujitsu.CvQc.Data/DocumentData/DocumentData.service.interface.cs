using Fujitsu.CvQc.Data.Entities;

namespace Fujitsu.CvQc.Data;
public interface IDocumentDataService
{
    public List<Document> GetProjectDocuments(Guid projectId);
    public Document? GetDocument(Guid id);
    public Document AddDocument(Document document);
    public Document UpdateDocument(Document document);
    public bool DeleteDocument(Document document);
}
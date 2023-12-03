namespace Fujitsu.CvQc.Business
{
    public interface IDocumentService
    {
        public List<DocumentModel> GetDocuments(string projectId);

        public DocumentModel? GetDocument(string id);
    }
}

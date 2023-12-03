using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace Fujitsu.CvQc.Console.App
{
    public interface IDocumentService
    {
        DocumentFileModel GetDocumentFile(string filePath);
        ICollection<DocumentFileModel> GetDocumentFiles(string directoryPath);
        Rule LoadRules(string rulesStr);
        DocumentModel GetDocumentMapping(Rule rules, DocumentFileModel documentFile);
        void CreateDocumentFromTemplate(string templatePath, DocumentModel document, string targetPath);
    }
}
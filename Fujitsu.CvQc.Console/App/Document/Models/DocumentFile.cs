using DocumentFormat.OpenXml.Packaging;

namespace Fujitsu.CvQc.Console.App
{
    public class DocumentFileModel
    {
        public string FileName { get; init; } = "";

        public WordprocessingDocument FileContent { get; init; }

        public DocumentFileModel(string fileName, WordprocessingDocument fileContent)
        {
            FileName = fileName;
            FileContent = fileContent;
        }
    }
}
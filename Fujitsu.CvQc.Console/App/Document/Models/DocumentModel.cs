namespace Fujitsu.CvQc.Console.App
{
    public class DocumentModel
    {
        public string FileName { get; set; } = "";

        public DocumentMappingModel? DocumentMap { get; set; } = null;

        public string ProjectId { get; set; } = "";

    }
}
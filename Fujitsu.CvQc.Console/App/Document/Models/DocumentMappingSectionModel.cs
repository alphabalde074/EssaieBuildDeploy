
namespace Fujitsu.CvQc.Console.App
{
    public class DocumentMappingSectionModel
    {
        public DocumentMappingSectionModel() { }
        public DocumentMappingSectionModel(string match, string target, ExportationMode exportationMode)
        {
            Match = match;
            Target = target;
            ExportationMode = exportationMode;
        }

        public string Match { get; set; } = "";

        public string Target { get; set; } = "";

        public ExportationMode ExportationMode { get; set; }

        public List<string> Paragraphs { get; set; } = new List<string>();
    }
}

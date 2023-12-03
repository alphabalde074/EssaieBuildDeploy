namespace Fujitsu.CvQc.Business
{
    public class DocumentMapSectionModel
    {
        public string Match { get; set; } = "";

        public string Target { get; set; } = "";

        public int ExportationMode { get; set; }

        public IEnumerable<string> Paragraphs { get; set; } = new List<string>();
    }
}

namespace Fujitsu.CvQc.Business
{
    public class DocumentMapping
    {
        public IEnumerable<DocumentMapSectionModel> Sections { get; set; } = new List<DocumentMapSectionModel>();
    }
}

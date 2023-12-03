
namespace Fujitsu.CvQc.Console.App
{
    public class ProjectModel
    {
        public string Id { get; set; } = "";
        public string Name { get; set; } = "";
        public ProjectConfigModel Config { get; set; } = new ProjectConfigModel();
        public ICollection<DocumentModel> Documents { get; set; } = new List<DocumentModel>();
    }
}

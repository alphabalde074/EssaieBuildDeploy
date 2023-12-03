
namespace Fujitsu.CvQc.Console.App
{
    public class ProjectConfigModel
    {
        public string InputPath { get; set; } = "";
        public string OutputPath { get; set; } = "";
        public string TemplatePath { get; set; } = "";
        public string OutputSuffix { get; set; } = "";
        public int ImportMode { get; set; } = 1;
        public string Rules { get; set; } = "";
    }
}

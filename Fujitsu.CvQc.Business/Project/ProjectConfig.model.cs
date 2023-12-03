namespace Fujitsu.CvQc.Business
{
    public class ProjectConfigModel
    {
        public int ImportMode { get; set; } = 0;
        public string InputPath { get; set; } = "";
        public string OutputPath { get; set; } = "";
        public string TemplatePath { get; set; } = "";
        public string OutputSuffix { get; set; } = "";
        public string Rules { get; set; } = "";
    }
}

namespace Fujitsu.CvQc.Business
{
    public class ValidationMessage
    {
        public string Title { get; set; } = "";
        public string Message { get; set; } = "";
        public string Severity { get; set; } = SeverityFlags.Danger;
    }
}
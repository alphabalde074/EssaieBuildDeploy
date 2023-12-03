namespace Fujitsu.CvQc.Console.App
{
    public class JobLogModel
    {
        public string JobId { get; set; } = "";

        public DateTime CreationDate { get; set; }

        public string Message { get; set; } = "";
    }
}

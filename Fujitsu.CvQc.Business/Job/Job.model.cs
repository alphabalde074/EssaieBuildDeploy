namespace Fujitsu.CvQc.Business;
public class JobModel
{
    public string Id { get; set; } = "";

    public string Mode { get; set; } = "";

    public DateTime CreationDate { get; set; }

    public DateTime? CompletionDate { get; set; } = null;

    public string Status { get; set; } = "";

    public string ProjectId { get; set; } = "";
}

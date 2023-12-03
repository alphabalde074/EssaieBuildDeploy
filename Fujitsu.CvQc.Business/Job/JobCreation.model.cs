using System.ComponentModel.DataAnnotations;

namespace Fujitsu.CvQc.Business;
public class JobCreationModel
{
    public JobCreationModel(string mode, string projectId)
    {
        Mode = mode ?? throw new ArgumentNullException(nameof(mode));
        ProjectId = projectId ?? throw new ArgumentNullException(nameof(projectId));
    }

    public string Mode { get; init; }

    [RegularExpression("^[0-9a-f]{8}-?[0-9a-f]{4}-?[0-9a-f]{4}-?[0-9a-f]{4}-?[0-9a-f]{12}$", ErrorMessage = "Invalid id format for project")]
    public string ProjectId { get; init; }
}

using Fujitsu.CvQc.Business;
using Microsoft.AspNetCore.Mvc;

namespace Fujitsu.CvQc.API.Domains;

[Route("api/[controller]")]
[ApiController]
public class JobController : ControllerBase
{
    private readonly IJobService jobService = ServiceInjector.serviceProvider!.GetService<IJobService>()!;

    [HttpGet("list/{projectId}")]
    public async Task<ActionResult<IEnumerable<JobModel>>> GetJobs(string projectId)
    {
        var task = Task.Run(() => jobService.GetJobs("ProjectId", projectId));
        return await task;
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(JobModel))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<JobModel?>> GetJob(string id)
    {
        var task = Task.Run(() => jobService.GetJob(id));
        return await task;
    }

    [HttpPost("new")]
    [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Post))]
    public async Task<ActionResult<string>> NewJob(JobCreationModel job)
    {
        var task = Task.Run(() => jobService.AddJob(job));
        return await task;
    }

    [HttpPut("complete")]
    [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Put))]
    public async Task<ActionResult<JobModel>> CompleteJob(JobModel job)
    {
        var task = Task.Run(() => jobService.CompleteJob(job.Id));
        return await task;
    }

    [HttpPut("stop")]
    [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Put))]
    public async Task<ActionResult<JobModel>> StopJob(JobModel job)
    {
        var task = Task.Run(() => jobService.StopJob(job.Id));
        return await task;
    }

    [HttpPut("resume")]
    [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Put))]
    public async Task<ActionResult<JobModel>> ResumeJob(JobModel job)
    {
        var task = Task.Run(() => jobService.ResumeJob(job.Id));
        return await task;
    }

    [HttpDelete("delete/{id}")]
    public async Task<ActionResult<bool>> DeleteJob(string id)
    {
        var task = Task.Run(() => jobService.DeleteJob(id));
        return await task;
    }

    [HttpGet("/api/project/{projectId}/[controller]/last")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(JobModel))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<JobModel?>> GetProjectLastJob(string projectId)
    {
        var task = Task.Run(() => jobService.GetProjectLastJob(projectId));
        return await task;
    }
}

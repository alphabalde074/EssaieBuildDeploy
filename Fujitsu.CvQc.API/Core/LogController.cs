using Fujitsu.CvQc.Business;
using Microsoft.AspNetCore.Mvc;

namespace Fujitsu.CvQc.API.Core;

[Route("api/job/[controller]")]
[ApiController]
public class LogController : ControllerBase
{
    private readonly ILogService logService = ServiceInjector.serviceProvider!.GetService<ILogService>()!;

    [HttpPost]
    public async Task<ActionResult<LogModel>> Log([FromBody] LogModel log)
    {
        var task = Task.Run(() => logService.AddLog(log));
        return await task;
    }

    [HttpGet("/api/job/{jobId}/[controller]/list")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<LogModel>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<LogModel>>> GetJobLogs(string jobId)
    {
        var task = await Task.Run(() => logService.GetJobLogs(jobId));
        return task;
    }
}

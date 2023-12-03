using Fujitsu.CvQc.Business;
using Microsoft.AspNetCore.Mvc;

namespace Fujitsu.CvQc.API.Domains;

[Route("api/[controller]")]
[ApiController]
public class ProjectController : ControllerBase
{
    private readonly IProjectService projectService = ServiceInjector.serviceProvider!.GetService<IProjectService>()!;

    [HttpGet("list")]
    public async Task<dynamic> GetProjects()
    {
        int aTestEssaieDep = 1;
        int aTestEssaieDep2 = 1;
        int aTestEssaieDep3 = 3;
        var task = Task.Run(() => projectService.GetProjects());
        return await task;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProjectModel?>> GetProject(string id)
    {
        var task = Task.Run(() => projectService.GetProject(id));
        return await task;
    }

    [HttpPost("add")]
    public async Task<dynamic> AddProject(ProjectModel model)
    {
        var task = Task.Run(() => projectService.AddProject(model));
        return await task;
    }

    [HttpPut("update")]
    public async Task<ActionResult<ProjectModel>> UpdateProject(ProjectModel model)
    {
        var task = Task.Run(() => projectService.UpdateProject(model));
        return await task;
    }

    [HttpDelete("delete/{id}")]
    public async Task<ActionResult<bool>> DeleteProject(string id)
    {
        var task = Task.Run(() => projectService.DeleteProject(id));
        return await task;
    }
}

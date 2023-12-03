namespace Fujitsu.CvQc.Business;

public interface IProjectService
{
    public ServerResponse<IEnumerable<ProjectModel>> GetProjects();
    public ProjectModel? GetProject(string id);
    public ServerResponse<ProjectModel> AddProject(ProjectModel model);
    public ProjectModel UpdateProject(ProjectModel model);
    public bool DeleteProject(string id);
}
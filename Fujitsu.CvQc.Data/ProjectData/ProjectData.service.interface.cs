using Fujitsu.CvQc.Data.Entities;

namespace Fujitsu.CvQc.Data;
public interface IProjectDataService
{
    public List<Project> GetProjects();
    public Project? GetProject(Guid id);
    public Project AddProject(Project entity);
    public Project UpdateProject(Project entity);
    public bool DeleteProject(Project entity);
}
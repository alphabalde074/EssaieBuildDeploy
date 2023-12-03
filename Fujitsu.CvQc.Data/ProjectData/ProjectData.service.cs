using Fujitsu.CvQc.Data.Entities;

namespace Fujitsu.CvQc.Data;
public class ProjectDataService : DataService<Project>, IProjectDataService
{

    public ProjectDataService(DataContext dataContext) : base(dataContext)
    {

    }

    public List<Project> GetProjects()
    {
        return GetAll();
    }

    public Project? GetProject(Guid id)
    {
        return Get(id);
    }

    public Project AddProject(Project entity)
    {
        return Add(entity);
    }

    public Project UpdateProject(Project entity)
    {
        return Update(entity);
    }

    public bool DeleteProject(Project entity)
    {
        return Delete(entity);
    }

    /*public List<DocumentMap> GetProjectDocumentsMapping(Guid projectId)
    {
        var project = dataContext.Projects.Single(project => project.Id == projectId);
        dataContext.Entry(project).Collection(project => project.DocumentMap).Load();
    }*/
}
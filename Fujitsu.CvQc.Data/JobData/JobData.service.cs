using Fujitsu.CvQc.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fujitsu.CvQc.Data;
public class JobDataService : DataService<Job>, IJobDataService
{
    public JobDataService(DataContext dataContext) : base(dataContext)
    {

    }

    public List<Job> GetJobs(string columnToSearch, string searchTerm)
    {       
        WaitAndLockThread();
        DbSet<Job> entities = dataContext.Set<Job>();
        Task<List<Job>> task;
        if (columnToSearch == "ProjectId")
            task = entities.Where(p => p.ProjectId.ToString() == searchTerm).ToListAsync();
        else
            task = entities.ToListAsync();

        Task.WaitAll(task);
        UnlockThread();

        return task.Result;
    }

    public Job? GetJob(Guid id)
    {
        return Get(id);
    }

    public Job AddJob(Job entity)
    {
        return Add(entity);
    }

    public Job UpdateJob(Job entity)
    {
        return Update(entity);
    }

    public bool DeleteJob(Job entity)
    {
        return Delete(entity);
    }

    public Job? GetProjectLastJob(Guid projectId)
    {
        WaitAndLockThread();
        DbSet<Job> entities = dataContext.Set<Job>();
        Task<Job?> task;
        task = entities.Where(j => j.ProjectId == projectId).OrderByDescending(o => o.CreationDate).FirstOrDefaultAsync();
        Task.WaitAll(task);
        UnlockThread();

        return task.Result;
    }
}
using Fujitsu.CvQc.Data.Entities;

namespace Fujitsu.CvQc.Data;
public interface IJobDataService
{
    public List<Job> GetJobs(string columnToSearch, string searchTerm);
    public Job? GetJob(Guid id);
    public Job AddJob(Job entity);
    public Job UpdateJob(Job entity);
    public bool DeleteJob(Job entity);
    public Job? GetProjectLastJob(Guid projectId);
}
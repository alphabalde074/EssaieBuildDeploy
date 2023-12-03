namespace Fujitsu.CvQc.Business
{
    public interface IJobService
    {
        public string AddJob(JobCreationModel job);

        public JobModel CompleteJob(string id);

        public JobModel? GetJob(string id);

        public List<JobModel> GetJobs(string columnToSearch, string searchTerm);
        
        public JobModel? GetProjectLastJob(string projectId);

        public bool DeleteJob(string id);

        public JobModel StopJob(string id);

        public JobModel ResumeJob(string id);
    }
}

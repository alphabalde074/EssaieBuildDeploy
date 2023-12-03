using Fujitsu.CvQc.Business;

namespace Fujitsu.CvQc.API.Test
{
    internal class JobServiceMock : IJobService
    {
        public string AddJob(JobCreationModel job)
        {
           return job.ProjectId;
        }

        public JobModel CompleteJob(string id)
        {
            return new JobModel();
        }

        public bool DeleteJob(string id)
        {
            return true;
        }

        public JobModel? GetJob(string id)
        {
            return new JobModel();
        }

        public List<JobModel> GetJobs(string columnToSearch, string searchTerm)
        {
            if (String.IsNullOrEmpty(searchTerm)) { return null; }
            return new List<JobModel>();
        }

        public JobModel? GetProjectLastJob(string projectId)
        {
            return new JobModel();
        }

        public JobModel ResumeJob(string id)
        {
            return new JobModel();
        }

        public JobModel StopJob(string id)
        {
            return new JobModel();
        }
    }
}
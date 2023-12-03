namespace Fujitsu.CvQc.Console.App
{
    public interface IJobDataService
    {
        public JobModel? GetJob(string jobId);
        public ProjectModel? GetProject(JobModel job);
        public void SetJobComplete(JobModel job);
        public void UpdateProject(ProjectModel project);
        public void Log(string message);        
    }
}

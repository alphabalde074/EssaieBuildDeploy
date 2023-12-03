namespace Fujitsu.CvQc.Console.App
{
    public class JobService : IJobDataService
    {
        private IDataService dataService = ServiceInjector.GetService<IDataService>();

        public JobModel? GetJob(string jobId)
        {
            var job = dataService.GetSync<JobModel>($"/api/job/{jobId}");
            return job;
        }

        public ProjectModel? GetProject(JobModel job)
        {
            var project = dataService.GetSync<ProjectModel>($"/api/project/{job.ProjectId}");
            return project;
        }        

        public void SetJobComplete(JobModel job)
        {
            dataService.PutSync("/api/job/complete", job);
        }        

        public void UpdateProject(ProjectModel project)
        {
            dataService.PutSync("/api/project/update", project);
        }

        public void Log(string message)
        {
            
            var log = new JobLogModel()
            {
                JobId = Environment.GetCommandLineArgs()[1],
                CreationDate = DateTime.Now,
                Message = message
            };
            System.Console.WriteLine(string.Format("%0: %1 %2", log.CreationDate, log.JobId, log.Message));
            dataService.PostSync("/api/job/log", log);
        }        
    }
}

using Fujitsu.CvQc.Console.App;

namespace Fujitsu.CvQc.Console
{
    public class StartupService : IStartupService
    {
        private IJobDataService jobService = ServiceInjector.GetService<IJobDataService>();
        private IImportationService importationService = ServiceInjector.GetService<IImportationService>();
        private IExportationService exportationService = ServiceInjector.GetService<IExportationService>();

        public void Launch(string[] args)
        {

            var jobId = GetJobId(args);
            if (!string.IsNullOrWhiteSpace(jobId))
            {                
                var job = jobService.GetJob(jobId);

                switch (job?.Mode) 
                {
                    case Constants.ImportMode:
                        importationService.ProcessJob(job);
                        System.Console.WriteLine("Import process complete");
                        break;

                    case Constants.ExportMode:
                        exportationService.ProcessJob(job);
                        System.Console.WriteLine("Export process complete");
                        break;

                    default:
                        System.Console.WriteLine("No valid job found");
                        break;
                }
            }   
            else {
                System.Console.WriteLine("JobId could not be found");
            }

            System.Console.WriteLine("Console app ended");
        }

        private string GetJobId(string[] args) 
        {
            if (args.Length > (int)ArgsPosition.JobId && !string.IsNullOrWhiteSpace(args[(int)ArgsPosition.JobId]))
            {
                var jobId = args[0];
                return jobId;
            }

            return string.Empty;
        }
    }
}

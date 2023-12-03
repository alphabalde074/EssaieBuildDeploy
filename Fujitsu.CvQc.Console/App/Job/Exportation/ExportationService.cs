namespace Fujitsu.CvQc.Console.App
{
    public class ExportationService : IExportationService
    {
        private IJobDataService jobDataService = ServiceInjector.GetService<IJobDataService>();
        private IDocumentService documentService = ServiceInjector.GetService<IDocumentService>();

        public void ProcessJob(JobModel job)
        {
            var project = jobDataService.GetProject(job);

            if (ValidateProject(project) && project != null)
            {
                foreach (var document in project.Documents)
                {
                    if (document.DocumentMap != null)
                    {
                        documentService.CreateDocumentFromTemplate(project.Config.TemplatePath, document, project.Config.OutputPath);
                    }
                    else
                    {
                        jobDataService.Log($"No DocumentMap found for {document.FileName}");
                    }                    
                }                
            }

            jobDataService.SetJobComplete(job);
        }

        private bool ValidateProject(ProjectModel? project)
        {           
            bool isValid = false;

            if (project == null || !project.Documents.Any())  
            {
                jobDataService.Log("No documents found");
            }        
            else if (string.IsNullOrWhiteSpace(project.Config.TemplatePath)) 
            {
                jobDataService.Log("Template Path is not defined");
            }               
            else if (string.IsNullOrWhiteSpace(project.Config.InputPath)) 
            {
                jobDataService.Log("Input Path is not defined");            
            }                          
            else
            {
                isValid = true;
            }

            return isValid;
        }
    }
}

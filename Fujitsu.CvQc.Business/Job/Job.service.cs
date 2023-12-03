using Fujitsu.CvQc.Data;
using Fujitsu.CvQc.Data.Entities;
using System.Diagnostics;

namespace Fujitsu.CvQc.Business;

public class JobService : IJobService
{
    private const string NEW_JOB_MESSAGE = "Process launched";
    private const string COMPLETE_JOB_MESSAGE = "Process completed";

    private readonly IJobDataService jobDataService;
    private readonly IProjectDataService projectDataService;

    public JobService(IJobDataService jobDataService, IProjectDataService projectDataService)
    {
        this.jobDataService = jobDataService;
        this.projectDataService = projectDataService;
    }
    public List<JobModel> GetJobs(string columnToSearch, string searchTerm)
    {
        var models = new List<JobModel>();

        var entities = jobDataService.GetJobs(columnToSearch, searchTerm);
        foreach (var entity in entities)
        {
            var model = MapToJobModel(entity);

            models.Add(model);
        }

        return models;
    }
    public string AddJob(JobCreationModel job)
    {
        Project? project = projectDataService.GetProject(new Guid(job.ProjectId)) ?? throw new ArgumentException("Invalid project");
        var entity = new Job
        {
            Mode = job.Mode,
            Status = "NEW",
            ProjectId = project.Id,
            CreationDate = DateTime.Now
        };

        jobDataService.AddJob(entity);
        StartJobProcess(entity.Id.ToString());
        return NEW_JOB_MESSAGE;
    }

    public JobModel CompleteJob(string jobId)
    {
        var guid = new Guid(jobId);
        var entity = jobDataService.GetJob(guid);
        if (entity != null)
        {
            entity.Status = "COMPLETE";
            entity.CompletionDate = DateTime.Now;
            entity = jobDataService.UpdateJob(entity);
        }

        JobModel model = MapToJobModel(entity);

        return model;
    }

    public JobModel ResumeJob(string jobId)
    {
        var guid = new Guid(jobId);
        var entity = jobDataService.GetJob(guid);
        if (entity != null)
        {
            entity.Status = "NEW";
            entity = jobDataService.UpdateJob(entity);
        }

        StartJobProcess(jobId);

        JobModel model = MapToJobModel(entity);

        return model;
    }

    private void StartJobProcess(string jobId)
    {
        var cmd = new ProcessStartInfo("cmd", "/k " +  @"cd ..\Fujitsu.CvQc.Console & dotnet run " + jobId);

        var process = Process.Start(cmd);        
    }

    public JobModel StopJob(string jobId)
    {
        var guid = new Guid(jobId);
        var entity = jobDataService.GetJob(guid);
        if (entity != null)
        {
            entity.Status = "STOPPED";
            entity = jobDataService.UpdateJob(entity);
        }

        JobModel model = MapToJobModel(entity);

        return model;
    }

    public JobModel? GetJob(string id)
    {
        JobModel? model = null;

        var guid = new Guid(id);
        var entity = jobDataService.GetJob(guid);
        if (entity != null)
        {
            model = MapToJobModel(entity);
        }

        return model;
    }

    public JobModel? GetProjectLastJob(string projectId)
    {
        JobModel? model = null;

        var guid = new Guid(projectId);
        var entity = jobDataService.GetProjectLastJob(guid);
        if (entity != null)
        {
            model = MapToJobModel(entity);
        }

        return model;
    }
    
    public bool DeleteJob(string id)
    {
        var result = false;

        var guid = new Guid(id);
        var entity = jobDataService.GetJob(guid);
        if (entity != null)
        {
            result = jobDataService.DeleteJob(entity);
        }

        return result;
    }

    #region Entities/Models mappers
    public JobModel MapToJobModel(Job? entity)
    {
        if (entity == null)
        {
            throw new NullReferenceException("MapToJobModel: Job entity cannot be null");
        }


        JobModel model = new JobModel()
        {
            Id = entity.Id.ToString(),
            ProjectId = entity.ProjectId.ToString(),
            CreationDate = entity.CreationDate,
            CompletionDate = entity.CompletionDate,
            Status = entity.Status,
            Mode = entity.Mode
        };
        return model;
    }
    #endregion
}

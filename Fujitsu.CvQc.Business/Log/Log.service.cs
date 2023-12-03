using Fujitsu.CvQc.Data;
using Fujitsu.CvQc.Data.Entities;

namespace Fujitsu.CvQc.Business;

public class LogService : ILogService
{
    private readonly IJobDataService jobDataService;
    private readonly ILogDataService logDataService;

    public LogService(IJobDataService jobDataService, ILogDataService logDataService)
    {
        this.jobDataService = jobDataService;
        this.logDataService = logDataService;
    }

    public LogModel AddLog(LogModel log)
    {
        Job? job = jobDataService.GetJob(new Guid(log.JobId)) ?? throw new ArgumentException("Invalid job");
        var entity = new Log
        {
            JobId = job.Id,
            CreationDate = log.CreationDate,
            Message = log.Message
        };

        entity = logDataService.AddLog(entity);

        return MapToLogModel(entity);
    }

    public LogModel? GetLog(string id)
    {
        LogModel? model = null;

        var guid = new Guid(id);
        var entity = logDataService.GetLog(guid);
        if (entity != null)
        {
            model = MapToLogModel(entity);
        }

        return model;
    }

    public List<LogModel> GetJobLogs(string jobId)
    {
        var models = new List<LogModel>();

        var guid = new Guid(jobId);
        var entities = logDataService.GetJobLogs(guid);
        foreach (var entity in entities)
        {
            var model = MapToLogModel(entity);

            models.Add(model);
        }

        return models;
    }

    #region Entities/Models mappers
    public LogModel MapToLogModel(Log? entity)
    {
        if (entity == null)
        {
            throw new NullReferenceException("MapToLogModel: Log entity cannot be null");
        }


        LogModel model = new LogModel()
        {
            JobId = entity.JobId.ToString(),
            CreationDate = entity.CreationDate,
            Message = entity.Message
        };
        return model;
    }
    #endregion
}

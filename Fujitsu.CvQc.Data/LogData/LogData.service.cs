using Fujitsu.CvQc.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Fujitsu.CvQc.Data;
public class LogDataService : DataService<Log>, ILogDataService
{


    public LogDataService(DataContext dataContext) : base(dataContext)
    {

    }

    public Log? GetLog(Guid id)
    {
        return Get(id);
    }

    public Log AddLog(Log entity)
    {
        return Add(entity);
    }

    public Log UpdateLog(Log entity)
    {
        return Update(entity);
    }

    public bool DeleteLog(Log entity)
    {
        return Delete(entity);
    }

    public IEnumerable<Log> GetJobLogs(Guid jobId)
    {
        WaitAndLockThread();
        var task = dataContext.Logs.Where(log => log.JobId == jobId).ToListAsync();
        Task.WaitAll(task);
        var result = task.Result;
        UnlockThread();

        return result;
    }
}
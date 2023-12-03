using Fujitsu.CvQc.Data.Entities;

namespace Fujitsu.CvQc.Data;
public interface ILogDataService
{
    public Log? GetLog(Guid id);
    public Log AddLog(Log entity);
    public Log UpdateLog(Log entity);
    public bool DeleteLog(Log entity);
    public IEnumerable<Log> GetJobLogs(Guid jobId);
}
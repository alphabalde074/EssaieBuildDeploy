namespace Fujitsu.CvQc.Business
{
    public interface ILogService
    {
        public LogModel AddLog(LogModel log);

        public LogModel? GetLog(string id);

        public List<LogModel> GetJobLogs(string jobId);
    }
}

namespace Fujitsu.CvQc.Console.App
{
    public interface IDataService
    {
        T? GetSync<T>(string url);
        bool PostSync<T>(string url, T payload);
        bool PutSync<T>(string url, T payload);
        bool DeleteSync(string url);
    }
}
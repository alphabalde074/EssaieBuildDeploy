namespace Fujitsu.CvQc.API.Startup;

public interface IStartupService
{
    public void Launch(string[] args, WebApplicationBuilder builder);
}

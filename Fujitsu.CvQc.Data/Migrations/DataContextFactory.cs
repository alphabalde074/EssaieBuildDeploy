using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;

namespace Fujitsu.CvQc.Data.Migrations;
public class DataContextFactory : IDesignTimeDbContextFactory<DataContext>
{
    public DataContextFactory()
    {
    }

    private readonly IConfiguration? Configuration;
    public DataContextFactory(IConfiguration configuration)
    {
        Configuration = configuration;
    }
    public DataContext CreateDbContext(string[] args)
    {
        string directory = Directory.GetCurrentDirectory();

        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        IConfiguration Configuration = new ConfigurationBuilder()
            .SetBasePath(directory)
            .AddJsonFile("appSettings.json")
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
        string connectionString = Configuration.GetConnectionString("DataContext") ?? "";
        connectionString = DataAccess.FormatConnectionString(Configuration, connectionString);
        optionsBuilder.UseSqlServer(connectionString);

        return new DataContext(optionsBuilder.Options);
    }
}
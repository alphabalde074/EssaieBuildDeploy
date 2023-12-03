namespace Fujitsu.CvQc.API.Startup;

public class StartupService : IStartupService
{

    public void Launch(string[] args, WebApplicationBuilder builder)
    {

        builder.Services.AddCors(o => o.AddPolicy("MainUI", builder =>
        {
            builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        }));

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        
        builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

        var app = builder.Build();
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        // app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();
        app.UseCors("MainUI");
        app.Run();
    }
}

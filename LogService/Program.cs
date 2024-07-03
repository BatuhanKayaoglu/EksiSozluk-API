using LogService;
using Quartz;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();

public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                services.AddLogging();

                // Quartz
                services.AddQuartz(q =>
                {
                    q.UseMicrosoftDependencyInjectionScopedJobFactory();

                    // Trigger configuration
                    var jobKey = new JobKey("WorkerJob");

                    q.AddJob<Worker>(opts => opts.WithIdentity(jobKey));

                    q.AddTrigger(opts => opts
                        .ForJob(jobKey)
                        .WithIdentity("WorkerJob-trigger")
                        .WithSimpleSchedule(x => x
                            .WithIntervalInSeconds(10) // 10 saniyede bir tetiklenir
                            .RepeatForever()));
                });

                services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
            });
}
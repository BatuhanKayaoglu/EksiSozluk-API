using Quartz;

namespace LogServices
{
    public class Worker : IJob
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        public Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            return Task.CompletedTask;
        }
    }       
}

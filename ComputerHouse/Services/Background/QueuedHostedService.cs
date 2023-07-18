using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ComputerHouse.Services.Background
{
    public class QueuedHostedService : BackgroundService
    {
        private readonly IBackgroundTaskQueue _taskQueue;
        private readonly ILogger<QueuedHostedService> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public QueuedHostedService(IBackgroundTaskQueue taskQueue, ILogger<QueuedHostedService> logger, IServiceScopeFactory serviceScopeFactory)
        {
            _taskQueue = taskQueue;
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Queued hosted service is starting...");

            // Start monitoring the queue for tasks.
            while(!stoppingToken.IsCancellationRequested)
            {
                // Wait to grab the next task from the queue.
                var workItem = await _taskQueue.DequeueAsync(stoppingToken);
                try
                {
                    // Execute the de-queued task.
                    await workItem(_serviceScopeFactory, stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error in executing {nameof(workItem)}.", ex.StackTrace);
                }
            }

            _logger.LogInformation("Queued hosted service is stopping..");
        }
    }
}

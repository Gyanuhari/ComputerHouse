using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ComputerHouse.Services.Background
{
    /// <summary>
    /// Note: This is just for learning.
    /// Background job to cleanup old files left in blob storage from batch download processing.
    /// </summary>
    public class DownloadCleanupService : BackgroundService
    {
        private readonly ILogger<DownloadCleanupService> _logger;
        private IBlobService _blobService;
        private int _executionCount = 0;

        public DownloadCleanupService(ILogger<DownloadCleanupService> logger, IBlobService blobService)
        {
            _logger = logger;
            _blobService = blobService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Cleanup Service Started");

            while(!stoppingToken.IsCancellationRequested)
            {
                // ToDo: Consider moving to configurable setting.
                await CleanupDownloads(stoppingToken);
                await Task.Delay(TimeSpan.FromHours(12), stoppingToken);
            }
        }

        private async Task CleanupDownloads(CancellationToken token)
        {
            if (token.IsCancellationRequested)
                return;

            var executions = Interlocked.Increment(ref _executionCount);
            _logger.LogInformation($"Cleanup job is running. Executions: {executions}");
            await _blobService.DeleteFiles("provide blob path here", new[] { ".zip" }, DateTimeOffset.UtcNow.AddDays(7), token);

        }
    }
}

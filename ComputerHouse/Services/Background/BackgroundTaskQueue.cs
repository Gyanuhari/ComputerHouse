using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace ComputerHouse.Services.Background
{
    /// <summary>
    /// Implementation of a background task queue. Queuing asynchronous tasks to be dequeued and executed in the background.
    /// </summary>
    public class BackgroundTaskQueue : IBackgroundTaskQueue
    {
        private readonly ConcurrentQueue<Func<IServiceScopeFactory, CancellationToken, Task>> _workItems 
            = new ConcurrentQueue<Func<IServiceScopeFactory, CancellationToken, Task>>();

        private readonly SemaphoreSlim _signal = new SemaphoreSlim(0);

        public async Task<Func<IServiceScopeFactory, CancellationToken, Task>> DequeueAsync(CancellationToken cancellationToken)
        {
            await _signal.WaitAsync(cancellationToken);
            _workItems.TryDequeue(out var workItem);

            return workItem;
        }

        public void QueueBackgroundWorkItem(Func<IServiceScopeFactory, CancellationToken, Task> workItem)
        {
            if (workItem == null)
                throw new ArgumentNullException(nameof(workItem));

            _workItems.Enqueue(workItem);
            _signal.Release();
        }
    }
}

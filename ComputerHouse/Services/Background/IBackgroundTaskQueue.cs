using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ComputerHouse.Services.Background
{
    public interface IBackgroundTaskQueue
    {
        void QueueBackgroundWorkItem(Func<IServiceScopeFactory, CancellationToken, Task> workItem);

        Task<Func<IServiceScopeFactory, CancellationToken, Task>> DequeueAsync(CancellationToken cancellationToken);
    }
}

using System;
using System.Threading;
using System.Threading.Tasks;

namespace ComputerHouse.Services
{
    /// <summary>
    /// Service endpoints for working with cloud blob storage.
    /// </summary>
    public interface IBlobService
    {
        Task DeleteFiles(string path, string[] extensions, DateTimeOffset before, CancellationToken token = default);
    }
}

using System.Threading;
using System.Threading.Tasks;

namespace ComputerHouse.Web.Services
{
    public interface IGlobalSettingsService
    {
        CancellationToken OnSettingsChanged { get; }

        Task<GlobalSettings> GetSettings();

        Task Save(GlobalSettings settings);
    }
}

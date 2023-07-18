using ComputerHouse.Data.Repositories;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ComputerHouse.Web.Services
{
    public class GlobalSettingsService : IGlobalSettingsService
    {
        private const string StorageKey = "ComputerHouse.GlobalSettings";

        // Cancellation source triggered when the global settings are updated.
        private static CancellationTokenSource changedSource;

        private readonly KeyValueRepository repository;

        private readonly IMemoryCache memoryCache;

        /// <summary>
        /// Cancellatin token that fires when the settings are updated and saved.
        /// </summary>
        public CancellationToken OnSettingsChanged => (changedSource ?? (changedSource = new CancellationTokenSource())).Token;

        public GlobalSettingsService(KeyValueRepository repository, IMemoryCache memoryCache)
        {
            this.repository = repository;
            this.memoryCache = memoryCache;
        }

        public async Task<GlobalSettings> GetSettings()
        {
            return await memoryCache.GetOrCreateAsync($"{nameof(GlobalSettingsService)}. {nameof(GlobalSettingsService.GetSettings)}", async entry =>
            {
                entry.AddExpirationToken(new CancellationChangeToken(OnSettingsChanged));
                entry.AbsoluteExpiration = DateTime.UtcNow.AddMinutes(60);
                var keyValue = await repository.GetByKey(StorageKey).ConfigureAwait(false);
                var settings = keyValue?.GetValue<GlobalSettings>() ?? new GlobalSettings();
                settings.RowVersion = keyValue?.RowVersion;

                return settings;
            }).ConfigureAwait(false);
        }

        public async Task Save(GlobalSettings settings)
        {
            var keyValue = await repository.GetByKey(StorageKey).ConfigureAwait(false);
            if (keyValue == null)
            {
                keyValue = new KeyValue()
                {
                    keyValue = StorageKey,
                    Created = DateTime.UtcNow,
                    Type = StorageKey,
                };
            }

            keyValue.RowVersion = settings.RowVersion;
            keyValue.SetValue(settings);

            repository.Upsert(keyValue);
            changedSource?.Cancel();
            changedSource = null;
        }
    }
}

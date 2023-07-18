namespace ComputerHouse.Settings
{
    public class AzureBlobOptions
    {
        // Note: Contains properties to map in appsettings.
        public const string SectionName = "AzureBlob";

        public string ConnectionString { get; set; }

        public string Container { get; set; }

        public string Key { get; set; }
    }
}

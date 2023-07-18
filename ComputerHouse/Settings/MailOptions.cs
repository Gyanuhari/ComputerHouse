namespace ComputerHouse.Settings
{
    public class MailOptions
    {
        public const string SectionName = "Mail";

        public string AdminAddress { get; set; }

        public string FromName { get; set; }

        public string FromAddress { get; set; }

        public string ApiKey { get; set; }
    }
}

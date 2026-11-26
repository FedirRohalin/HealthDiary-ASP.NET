namespace HealthDiary.Web.Configurations
{
    public class AppConfiguration
    {
        public string ApplicationName { get; set; } = string.Empty;
        public ConnectionStringsSettings ConnectionStrings { get; set; } = new();
        public StepLabSettings StepLabSettings { get; set; } = new();
    }

    public class ConnectionStringsSettings
    {
        public string DefaultConnection { get; set; } = string.Empty;
    }

    public class StepLabSettings
    {
        public string DefaultPrinterModel { get; set; } = string.Empty;
        public int MaxVideoSizeMb { get; set; }
        public string ApiKey { get; set; } = string.Empty;
    }
}
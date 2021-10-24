namespace CardMon.Core.Helpers
{
    public class AppSettings
    {
        public string INB_RootUrl { get; set; }
        public string ONB_RootUrl { get; set; }
        public string PinReset_RootUrl { get; set; }
        public Endpoint Endpoints { get; set; }
        public string Service_Name { get; set; }
        public AuthConfig AuthConfig { get; set; }
    }
}

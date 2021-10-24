namespace CardMon.Core.Helpers
{
    public class AuthConfig
    {
        public string Secret { get; set; }
        public string ValidIssuer { get; set; }
        public string ValidAudience { get; set; }
    }
}

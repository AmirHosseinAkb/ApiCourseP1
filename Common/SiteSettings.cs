namespace Common
{
    public class SiteSettings
    {
        public string ElmahPath { get; set; }
        public JwtSettings JwtSettings { get; set; }
    }

    public class JwtSettings
    {
        public string Audience { get; set; }
        public string Issuer { get; set; }
        public string SecretKey { get; set; }
        public int ExpireDays { get; set; } = 14;
        public int NotBeforeMinutes { get; set; }=0;

    }
}

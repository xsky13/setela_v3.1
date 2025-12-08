namespace SetelaServerV3._1.Application.Features.Auth.Config
{
    public class AuthOptions
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Key { get; set; }
        public int ExpMinutes { get; set; }
    }
}

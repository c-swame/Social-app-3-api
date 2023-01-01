using Microsoft.Extensions.Configuration;

namespace Social_app_3_api.Constants
{
    public class TokenConstants
    {
        public readonly static string Secret = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json").Build().GetValue<string>("JWTSecret");
    }
}

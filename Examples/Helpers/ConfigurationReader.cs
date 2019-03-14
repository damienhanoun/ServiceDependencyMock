using Microsoft.Extensions.Configuration;

namespace Examples
{
    public static class ConfigurationReader
    {
        private static readonly IConfiguration configuration;

        static ConfigurationReader()
        {
            configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
        }

        public static string Get(string name)
        {
            return configuration[name];
        }
    }
}

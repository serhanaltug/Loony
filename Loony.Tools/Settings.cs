using Microsoft.Extensions.Configuration;

namespace Loony.Tools
{
    public static class Settings
    {
        public static string readFromConfigFile(string key)
        {
            var result = "Key not valid";
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json").Build();

            if (config.GetSection(key) != null) { result = config.GetSection(key).Value; }
            return result;
        }

    }
}

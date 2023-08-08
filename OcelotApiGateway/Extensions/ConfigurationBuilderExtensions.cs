using Newtonsoft.Json;
using Ocelot.Configuration.File;

namespace OcelotApiGateway.Extensions
{
    public static class ConfigurationBuilderExtensions
    {
        public static IConfigurationBuilder AddOcelotConfigs(this IConfigurationBuilder builder, IWebHostEnvironment env, string folder)
        {
            const string primaryConfigFile = "ocelot.json";

            var configFiles = new DirectoryInfo(folder)
                .GetFiles("*.json", SearchOption.AllDirectories)
                .ToList();

            var fileConfiguration = new FileConfiguration();

            foreach (var configFile in configFiles)
            {
                var lines = File.ReadAllText(configFile.FullName);
                var config = JsonConvert.DeserializeObject<FileConfiguration>(lines)!;

                fileConfiguration.Aggregates.AddRange(config.Aggregates);
                fileConfiguration.Routes.AddRange(config.Routes);
            }

            var json = JsonConvert.SerializeObject(fileConfiguration);
            File.WriteAllText(primaryConfigFile, json);
            builder.AddJsonFile(primaryConfigFile, false, false);

            return builder;
        }
    }
}

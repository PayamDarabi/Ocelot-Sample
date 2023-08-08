using Newtonsoft.Json;
using Ocelot.Configuration.File;

namespace OcelotApiGateway.Extensions
{
    public static class OcelotConfigurationBuilder
    {
        const string primaryConfigFile = "ocelot.json";
        public static IConfigurationBuilder AddOcelotConfigs(this IConfigurationBuilder builder, string rootFolder)
        {
            var configFiles = new DirectoryInfo(rootFolder).GetFiles("*.json", SearchOption.AllDirectories).ToList();
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

using Microsoft.Extensions.Configuration;
using System;
//@BaseCode

namespace CommonBase.Modules.Configuration
{
    public static partial class Configurator
    {
        public static IConfigurationRoot LoadAppSettings()
        {
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var builder = new ConfigurationBuilder()
                .SetBasePath(Environment.CurrentDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environmentName ?? "Development"}.json", optional: true)
                .AddEnvironmentVariables();

            return builder.Build();
        }
    }
}

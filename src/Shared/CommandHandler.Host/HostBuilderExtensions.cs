using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Console.Host
{
    public static class HostBuilderExtensions
    {
        public static IHostBuilder AddConfigFile(this IHostBuilder builder,
            Action<IConfigurationBuilder> configBuilder = null)
        {
            builder.ConfigureAppConfiguration(b =>
            {
                b.SetBasePath(Directory.GetCurrentDirectory());
                b.AddJsonFile("appsettings.json", optional: false);
                configBuilder?.Invoke(b);
            });
            return builder;
        }
    }
}

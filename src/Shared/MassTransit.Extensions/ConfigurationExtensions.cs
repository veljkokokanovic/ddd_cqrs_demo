using System;
using MassTransit.RabbitMqTransport;
using Microsoft.Extensions.Configuration;

namespace MassTransit.Extensions
{
    public static class ConfigurationExtensions
    {
        public static IRabbitMqHost ConfigureHost(this IRabbitMqBusFactoryConfigurator configurator, IConfigurationSection busConfig,
            Action<IRabbitMqHostConfigurator> hostConfig = null)
        {
            return configurator.Host(new Uri(busConfig["Host"]), hostCfg =>
            {
                hostCfg.Username(busConfig["User"]);
                hostCfg.Password(busConfig["Password"]);
                hostConfig?.Invoke(hostCfg);
            });
        }
    }
}

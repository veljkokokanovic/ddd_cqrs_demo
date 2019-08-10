using System;
using System.IO;
using System.Threading.Tasks;
using CommandHandler.Host;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Order.CommandHandler
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await new HostBuilder()
                .AddConfigFile()
                .ConfigureServices(ServicesConfigurator.ConfigureBus)
                .ConfigureServices(ServicesConfigurator.ConfigureDomain)
                .ConfigureServices(ServicesConfigurator.ConfigureEventStore)
                .RunConsoleAsync();
        }
    }
}

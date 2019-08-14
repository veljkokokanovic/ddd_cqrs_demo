using System;
using System.Threading.Tasks;
using Console.Host;
using Microsoft.Extensions.Hosting;

namespace Delivery.CommandHandler
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

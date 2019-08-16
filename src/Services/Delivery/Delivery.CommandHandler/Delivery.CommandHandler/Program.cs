using System;
using System.Threading.Tasks;
using Console.Host;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;

namespace Delivery.CommandHandler
{
    class Program
    {
        static async Task Main(string[] args)
        {
            System.Console.Title = "Delivery Command Handler";

            await new HostBuilder()
                .AddConfigFile()
                .ConfigureServices(ServicesConfigurator.ConfigureBus)
                .ConfigureServices(ServicesConfigurator.ConfigureDomain)
                .ConfigureServices(ServicesConfigurator.ConfigureEventStore)
                .ConfigureLogging((hostingContext, logging) =>
                {
                    logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                    logging.AddConsole();
                })
                .RunConsoleAsync();
        }
    }
}

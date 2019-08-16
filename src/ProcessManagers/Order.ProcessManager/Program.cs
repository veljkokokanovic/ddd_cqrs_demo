using System;
using System.Threading.Tasks;
using Console.Host;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;

namespace Order.ProcessManager
{
    class Program
    {
        static async Task Main(string[] args)
        {
            System.Console.Title = "Process Manager";

            await new HostBuilder()
                .AddConfigFile()
                .ConfigureServices(ServicesConfigurator.ConfigureBus)
                .ConfigureLogging((hostingContext, logging) =>
                {
                    logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                    logging.AddConsole();
                })
                .RunConsoleAsync();
        }
    }
}

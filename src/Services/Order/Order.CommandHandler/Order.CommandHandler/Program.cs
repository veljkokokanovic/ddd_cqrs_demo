using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;
using System.IO;
using System.Threading.Tasks;


namespace Order.CommandHandler
{
    class Program
    {
        static async Task Main(string[] args)
        {
            System.Console.Title = "Order Command Handler";

            await new HostBuilder()
                 .ConfigureAppConfiguration(b =>
                 {
                     b.SetBasePath(Directory.GetCurrentDirectory());
                     b.AddJsonFile("appsettings.json", optional: false);
                 })
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

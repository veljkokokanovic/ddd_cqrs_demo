using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;
using System.IO;
using System.Threading.Tasks;

namespace Order.ProcessManager
{
    class Program
    {
        static async Task Main(string[] args)
        {
            System.Console.Title = "Process Manager";

            await new HostBuilder()
                .ConfigureAppConfiguration(b =>
                {
                    b.SetBasePath(Directory.GetCurrentDirectory());
                    b.AddJsonFile("appsettings.json", optional: false);
                })
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

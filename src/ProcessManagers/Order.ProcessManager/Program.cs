using System;
using System.Threading.Tasks;
using Console.Host;
using Microsoft.Extensions.Hosting;

namespace Order.ProcessManager
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await new HostBuilder()
                .AddConfigFile()
                .ConfigureServices(ServicesConfigurator.ConfigureBus)
                .RunConsoleAsync();
        }
    }
}

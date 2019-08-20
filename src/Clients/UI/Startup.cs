
using MassTransit;
using MassTransit.Extensions;
using MassTransit.ExtensionsDependencyInjectionIntegration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;
using UI.MessageConsumers;
using UI.SignalR;
using DefaultUserIdProvider = UI.SignalR.DefaultUserIdProvider;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace UI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddSignalR();
            services.AddMassTransit(ConfigureBus);
            services.AddTransient<IUserIdProvider, DefaultUserIdProvider>();
            services.AddSingleton<IHostedService, BusService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            
            app.UseSignalR(route =>
            {
                route.MapHub<NotificationHub>("/commandnotify");
            });
        }

        private void ConfigureBus(IServiceCollectionConfigurator configurator)
        {
            configurator.AddConsumer<ProductAddedToOrderConsumer>();
            configurator.AddConsumer<ProductQuantityChangedConsumer>();
            configurator.AddConsumer<CommandErrorConsumer>();
            configurator.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                var busConfig = Configuration.GetSection("Bus");
                var host = cfg.ConfigureHost(busConfig);
                cfg.ReceiveEndpoint(host, "UISubscriptions", e =>
                {
                    e.ConfigureConsumer<ProductAddedToOrderConsumer>(provider);
                    e.ConfigureConsumer<ProductQuantityChangedConsumer>(provider);
                    e.ConfigureConsumer<CommandErrorConsumer>(provider);
                });
            }));
        }

        public class BusService : IHostedService
        {
            private readonly IBusControl _busControl;

            public BusService(IBusControl busControl)
            {
                _busControl = busControl;
            }

            public Task StartAsync(CancellationToken cancellationToken)
            {
                return _busControl.StartAsync(cancellationToken);
            }

            public Task StopAsync(CancellationToken cancellationToken)
            {
                return _busControl.StopAsync(cancellationToken);
            }
        }
    }
}

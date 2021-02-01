using System;
using System.Threading.Tasks;
using GreenPipes;
using IChat.Don.Consumers;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace IChat.Don
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = new HostBuilder()
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.AddJsonFile("appsettings.json", optional: true);
                    config.AddEnvironmentVariables();

                    if (args != null)
                    {
                        config.AddCommandLine(args);
                    }
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddMassTransit(cfg =>
                    {
                        cfg.AddBus(ConfigureBus);
                    });

                    services.AddSingleton<IHostedService, BusService>();
                })
                .ConfigureLogging((hostingContext, logging) =>
                {
                    logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                    logging.AddConsole();
                });

            try
            {
                await builder.RunConsoleAsync();
                
                Log.Logger.Information("Started Don service");
            }
            catch (Exception exception)
            {
                Log.Logger.Error($"An exception is caught: {exception.Message}");
            }
        }

        static IBusControl ConfigureBus(IServiceProvider provider)
        {
            var bus = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.Host("rabbitmq://localhost");
                
                cfg.ReceiveEndpoint("don-queue", e =>
                {
                    e.Consumer<PublishMessageConsumer>();
                    e.Consumer<SendMessageConsumer>();
                    
                    e.UseMessageRetry(r =>
                    {
                        r.Interval(3, TimeSpan.FromMilliseconds(100));
                    });
                });
                
                // generate queues as many as U want
                cfg.ReceiveEndpoint("don-backup-queue", e =>
                {
                    e.Consumer<SendMessageConsumer>();
                });
            });

            return bus;
        }
    }
}
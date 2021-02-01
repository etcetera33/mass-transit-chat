using System;
using System.Threading.Tasks;
using GreenPipes;
using IChat.Common.Exceptions;
using IChat.Joe.Consumers;
using IChat.Joe.Observers;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace IChat.Joe
{
    public static class Program
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
                
                
                cfg.ReceiveEndpoint("joe-queue", e =>
                {
                    e.Consumer<QuestionAskedConsumer>();
                    
                    e.UseMessageRetry(r =>
                    {
                        r.Interval(4, TimeSpan.FromMilliseconds(100));
                    });
                    
                    e.Consumer<PublishMessageConsumer>(c => c.UseMessageRetry(r => 
                        {
                            r.Immediate(10);
                            
                            r.Ignore<ArgumentNullException>();
                            r.Ignore<SillyException>(x => x.Message.Contains("C#"));
                        })
                    );
                });
            });

            bus.ConnectReceiveObserver(new BaseConsumerObserver());

            return bus;
        }
    }
}
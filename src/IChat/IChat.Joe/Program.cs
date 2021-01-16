using System;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;

namespace IChat.Joe
{
    class Program
    {
        public static void Main(string[] args)
        {
            // var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
            // {
            //     cfg.Host("rabbitmq://localhost");
            //     
            //     cfg.ReceiveEndpoint("event-listener", e =>
            //     {
            //         e.Consumer<PublishMessageConsumer>();
            //         e.Consumer<SendMessageConsumer>();
            //     });
            // });
            //
            // var source = new CancellationTokenSource(TimeSpan.FromSeconds(10));
            //
            // await busControl.StartAsync(source.Token);
            Console.WriteLine("Joe started successfully");
        }
    }
}
using System;
using System.Linq;
using System.Threading.Tasks;
using IChat.Client.Producers;
using IChat.Common;
using IChat.Contracts;
using MassTransit;

namespace IChat.Client
{
    public class Program
    {
        public static async Task Main()
        {
            /*
             * Setting up the Bus
             */
            #region Bus setup

            var bus = Bus.Factory.CreateUsingRabbitMq(sbc =>
            {
                sbc.Host("rabbitmq://localhost");
                
                EndpointConvention.Map<ISendMessage>(new Uri("rabbitmq://localhost/don-queue"));
                
                sbc.ReceiveEndpoint("test_queue", ep =>
                {
                    // ep.Handler<Message>(context =>
                    // {
                    //     return Console.Out.WriteLineAsync($"Received: {context.Message.Text}");
                    // });
                });
            });

            await bus.StartAsync(); // This is important!

            #endregion
            
            /*
             * Output setup
             */
            #region Output setup

            Console.BackgroundColor = ConsoleColor.Blue;
            Console.ForegroundColor = ConsoleColor.White;

            Console.WriteLine("What feature would you like to demo?");
            
            foreach (var enumType in Enum.GetValues(typeof(FeatureTypes)).Cast<FeatureTypes>())
            {
                Console.WriteLine($"{(int) enumType} - {EnumHelper<FeatureTypes>.GetDisplayValue(enumType)}");
            }

            Console.WriteLine("q to eqit");
            #endregion

            string input;
            
            await Task.Run(async () =>
            {
                do
                {
                    input = Console.ReadLine();

                    if (!int.TryParse(input, out var inputInteger) && input != "q")
                    {
                        Console.BackgroundColor = ConsoleColor.Red;
                        Console.WriteLine("Wrong format");
                        Console.ResetColor();

                        continue;
                    }
                    
                    if (input == "q")
                    {
                        continue;
                    }

                    var producersFactory = new ProducersFactory(bus);
                    await producersFactory.ProduceMessage(inputInteger);
                    
                } while (input != "q");
            });
            
            await bus.StopAsync();
        }
    }
}
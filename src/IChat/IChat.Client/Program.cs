using System;
using System.Threading.Tasks;
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

            var bus = Bus.Factory.CreateUsingRabbitMq(sbc =>
            {
                sbc.Host("rabbitmq://localhost");
                
                EndpointConvention.Map<ISendMessage>(new Uri("rabbitmq://localhost/don-queue"));
            });

            await bus.StartAsync();

            /*
             * Output setup
             */

            Console.BackgroundColor = ConsoleColor.Blue;
            Console.ForegroundColor = ConsoleColor.White;

            Console.WriteLine("Type your message here (q to exit):");
            

            string input;
            
            await Task.Run(async () =>
            {
                do
                {
                    input = Console.ReadLine();

                    if (input == "q")
                    {
                        continue;
                    }
                    
                    await bus.Publish<IMessagePublished>(new { Message = input });    
                    // await bus.Send<ISendMessage>(new { Message = input });

                    // await bus.Publish<IQuestionAsked>(new { Question = input });    

                } while (input != "q");
            });
            
            await bus.StopAsync();
        }
    }
}
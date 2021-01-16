using System;
using System.Threading.Tasks;
using IChat.Contracts;
using MassTransit;

namespace IChat.Don.Consumers
{
    public class PublishMessageConsumer : IConsumer<IMessagePublished>
    {
        public async Task Consume(ConsumeContext<IMessagePublished> context)
        {
            await Task.Delay(100);
            
            Console.WriteLine($"Published message received: {context.Message.Message}");
        }
    }
}
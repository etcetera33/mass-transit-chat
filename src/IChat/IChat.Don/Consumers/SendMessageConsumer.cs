using System;
using System.Threading.Tasks;
using IChat.Contracts;
using MassTransit;

namespace IChat.Don.Consumers
{
    public class SendMessageConsumer : IConsumer<ISendMessage>
    {
        public async Task Consume(ConsumeContext<ISendMessage> context)
        {
            await Task.Delay(100);
            
            Console.WriteLine($"Sent message received: {context.Message.Message}");
        }
    }
}
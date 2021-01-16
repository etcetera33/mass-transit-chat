using System.Threading.Tasks;
using IChat.Contracts;
using MassTransit;

namespace IChat.Client.Producers
{
    public class MessagePublishedProducer : IBaseProducer
    {
        private readonly IBusControl _bus;

        public MessagePublishedProducer(IBusControl bus)
        {
            _bus = bus;
        }
        
        public async Task ProduceMessage()
        {
            await _bus.Publish<IMessagePublished>(new {Text = "hello"});    
        }
    }
}
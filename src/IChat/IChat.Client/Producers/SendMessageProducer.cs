using System.Threading.Tasks;
using IChat.Contracts;
using MassTransit;

namespace IChat.Client.Producers
{
    public class SendMessageProducer : IBaseProducer
    {
        private readonly IBusControl _bus;

        public SendMessageProducer(IBusControl bus)
        {
            _bus = bus;
        }
        
        public async Task ProduceMessage()
        {
            await _bus.Send<ISendMessage>(new {Text = "hello"});
        }
    }
}
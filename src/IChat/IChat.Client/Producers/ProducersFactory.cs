using System;
using System.Threading.Tasks;
using MassTransit;

namespace IChat.Client.Producers
{
    public class ProducersFactory
    {
        private readonly IBusControl _bus;

        public ProducersFactory(IBusControl bus)
        {
            _bus = bus;
        }
        
        public async Task ProduceMessage(int chosenFeature)
        {
            IBaseProducer producer = (chosenFeature) switch 
            {
                (int) FeatureTypes.SimplePublish => new SendMessageProducer(_bus),

                (int) FeatureTypes.SimpleSend => new MessagePublishedProducer(_bus),
                
                _ => null
            };

            if (producer == null)
            {
                Console.WriteLine("Feature is invalid");
            }
            else
            {
                await producer.ProduceMessage();
            }
        }
    }
}
using System.Threading.Tasks;

namespace IChat.Client.Producers
{
    public interface IBaseProducer
    {
        Task ProduceMessage();
    }
}
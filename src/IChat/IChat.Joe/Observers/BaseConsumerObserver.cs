using System;
using System.Threading.Tasks;
using MassTransit;

namespace IChat.Joe.Observers
{
    public class BaseConsumerObserver : IReceiveObserver
    {
        public async Task PreReceive(ReceiveContext context)
        {
            await Task.Run(() => Console.WriteLine($"[{DateTime.Now}] Message pre received"));
        }

        public async Task PostReceive(ReceiveContext context)
        {
            await Task.Run(() => Console.WriteLine($"[{DateTime.Now}] Post received observer"));
        }

        public async Task PostConsume<T>(ConsumeContext<T> context, TimeSpan duration, string consumerType) where T : class
        {
            await Task.Run(() => Console.WriteLine(
                $"[{DateTime.Now}] Message consumed. Time: {duration}. Consumer type: {consumerType}"));
        }

        public async Task ConsumeFault<T>(ConsumeContext<T> context, TimeSpan duration, string consumerType, Exception exception) where T : class
        {
            await Task.Run(() => Console.WriteLine($"[{DateTime.Now}] Message consume faulted. Time: {duration}. Exception: {exception.Message}"));
        }

        public async Task ReceiveFault(ReceiveContext context, Exception exception)
        {
            await Task.Run(() => Console.WriteLine($"[{DateTime.Now}] Message receive faulted. Exception: {exception.Message}"));
        }
    }
}
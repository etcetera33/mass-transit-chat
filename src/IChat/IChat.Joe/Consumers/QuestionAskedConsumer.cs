using System;
using System.Threading.Tasks;
using IChat.Common.Exceptions;
using IChat.Contracts;
using MassTransit;

namespace IChat.Joe.Consumers
{
    public class QuestionAskedConsumer : IConsumer<IQuestionAsked>
    {
        public Task Consume(ConsumeContext<IQuestionAsked> context)
        {
            Console.WriteLine("Silly exception thrown");
            
            throw new SillyException("Silly Mitya`s Exception");
        }
    }
}
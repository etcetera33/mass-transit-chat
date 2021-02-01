using System;

namespace IChat.Common.Exceptions
{
    public class SillyException : Exception
    {
        public SillyException(string exceptionMessage): base(exceptionMessage)
        {
            
        }
    }
}
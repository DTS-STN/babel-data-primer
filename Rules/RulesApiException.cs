using System;

namespace DataPrimer.Rules
{
    public class RulesApiException : Exception
    {
        public RulesApiException()
        {
        }

        public RulesApiException(string message) : base(message)
        {
        }

        public RulesApiException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
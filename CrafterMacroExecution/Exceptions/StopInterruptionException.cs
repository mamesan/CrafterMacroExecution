using System;

namespace CrafterMacroExecution.Exceptions
{
    public class StopInterruptionException : SystemException
    {
        public StopInterruptionException()
        {
        }

        public StopInterruptionException(string message)
            : base(message)
        {
        }

        public StopInterruptionException(string message, SystemException inner)
            : base(message, inner)
        {
        }
    }
}
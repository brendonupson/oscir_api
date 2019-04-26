using System;
namespace DomainLayer.Exceptions
{
    public class DataWriteException : Exception
    {
        public DataWriteException(string message) : base(message)
        {

        }

        public DataWriteException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}

using System;
namespace DomainLayer.Exceptions
{
    public class DataReadException : Exception
    {
        public DataReadException(string message) : base(message)
        {

        }

        public DataReadException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}

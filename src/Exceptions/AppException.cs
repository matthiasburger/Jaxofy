using System;

namespace Jaxofy.Exceptions
{
    [Serializable]
    public class AppException : Exception
    {
        public AppException(string message) : base(message)
        {
        }
    }
}
using System;

namespace Jaxofy.Exceptions
{
    public class AssertionException : Exception
    {
        public AssertionException(string message): base(message)
        {
            
        }
    }
}
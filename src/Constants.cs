using Jaxofy.Data.Models.Base;
using Jaxofy.Models;

namespace Jaxofy
{
    /// <summary>
    /// Useful class containing constant values commonly used throughout the application's codebase.
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// Possible errors returned by the API.
        /// </summary>
        public static class Errors
        {
            public static Error _
                => new(1, "");

            public static Error ResourceNotFound<T>(params long[] keys) where T : IEntity<long>
                => new(2, $"Could not find {typeof(T).Name} with key(s): [{string.Join(", ", keys)}]");


            public static Error LoginFailed => new(1000, "Login failed");

            public static Error UserNotFound => new(1001, "User not found");

            public static Error UserAlreadyExists => new(1002, "User already exists");

            public static Error UserCreationFailed => new(1003, "User creation failed");

        }
    }
}
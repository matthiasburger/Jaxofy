using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using IronSphere.Extensions;
using Jaxofy.Extensions;
using Jaxofy.Models.Settings;
using Konscious.Security.Cryptography;
using Microsoft.Extensions.Options;

namespace Jaxofy.Services.PasswordHashing
{
    /// <summary>
    /// Argon2 implementation of the <see cref="IPasswordHashing"/> interface. 
    /// </summary>
    public class PasswordHashingArgon2 : IPasswordHashing
    {
        private readonly IOptionsMonitor<Argon2HashingParameters> _parameters;

        public PasswordHashingArgon2(IOptionsMonitor<Argon2HashingParameters> parameters)
        {
            _parameters = parameters;
        }

        public async Task<string> HashPassword(string password)
        {
            Argon2HashingParameters argon2Options = _parameters.CurrentValue;

            try
            {
                byte[] salt = new byte[argon2Options.SaltLength];
                using RandomNumberGenerator rng = RandomNumberGenerator.Create();
                rng.GetBytes(salt);

                using Argon2id argon2 = new(password.GetBytes(Encoding.UTF8))
                {
                    Salt = salt,
                    Iterations = argon2Options.TimeCost,
                    MemorySize = argon2Options.MemoryCostKiB,
                    DegreeOfParallelism = argon2Options.Parallelism
                };

                byte[] hash = await argon2.GetBytesAsync(argon2Options.HashLength);

                return $"$argon2id$v=19$m={argon2.MemorySize},t={argon2.Iterations},p={argon2.DegreeOfParallelism}${salt.ToBase64String(true)}${hash.ToBase64String(true)}";
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> Verify(string password, string hash)
        {
            if (password.IsNullOrEmpty() || hash.IsNullOrEmpty() || !hash.StartsWith("$argon2id$"))
            {
                return false;
            }

            try
            {
                string[] segments = hash.Split('$');

                if (segments.Length != 6)
                {
                    return false;
                }

                string[] parameters = segments[3].Split(',');

                int m = int.Parse(parameters[0].Replace("m=", string.Empty));
                int t = int.Parse(parameters[1].Replace("t=", string.Empty));
                int p = int.Parse(parameters[2].Replace("p=", string.Empty));

                byte[] saltBytes = segments[4].ToBytesFromBase64();
                byte[] hashBytes = segments[5].ToBytesFromBase64();

                if (saltBytes.Length < 8 || hashBytes.Length < 16)
                {
                    return false;
                }
                
                using Argon2id argon2 = new(password.GetBytes(Encoding.UTF8))
                {
                    MemorySize = m,
                    Iterations = t,
                    DegreeOfParallelism = p,
                    Salt = saltBytes,
                };

                return hashBytes.SequenceEqual(await argon2.GetBytesAsync(hashBytes.Length));
            }
            catch
            {
                return false;
            }
        }
    }
}

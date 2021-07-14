using System.Threading.Tasks;
using DasTeamRevolution.Models.Settings;
using DasTeamRevolution.Services.PasswordHashing;
using Microsoft.Extensions.Options;
using Xunit;
using Moq;

namespace DasTeamRevolution.Tests
{
    public class PasswordHashingTests
    {
        [Theory]
        [InlineData("ExtremelyBadPassword")]
        [InlineData("Slightly.Stronger;043!")]
        [InlineData("ULTIMATE_Blast;HolyCrap! it even has 1337  UNICODE SMILEYS in it 😀 😁 😂")]
        public async Task IPasswordHashing_HashPassword_VerifiesCorrectly(string password)
        {
            IOptionsMonitor<Argon2HashingParameters> parameters = Mock.Of<IOptionsMonitor<Argon2HashingParameters>>(
                optionsMonitor => optionsMonitor.CurrentValue == new Argon2HashingParameters
                {
                    TimeCost = 16,
                    MemoryCostKiB = 16384,
                    Parallelism = 4
                }
            );

            IPasswordHashing passwordHashing = new PasswordHashingArgon2(parameters);

            string hash = await passwordHashing.HashPassword(password);

            Assert.NotEqual(password, hash);
            Assert.True(hash.Length > 32);
            Assert.True(await passwordHashing.Verify(password, hash));
            Assert.False(await passwordHashing.Verify("Wrong Password !!!", hash));
        }
    }
}
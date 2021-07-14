using System;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

namespace DasTeamRevolution.BackgroundServices
{
    public class TestServiceSettings : PeriodicServiceSettings
    {
        public static PeriodicServiceSettings Default => new TestServiceSettings
        {
            Frequency = TimeSpan.FromSeconds(10),
            TimeOut = TimeSpan.FromSeconds(5)
        };
    }
    
    public class TestService : PeriodicBackgroundService
    {
        private readonly ILogger<TestService> _logger;
        private readonly Random _random;
        
        public TestService(ILogger<TestService> logger):base(TestServiceSettings.Default)
        {
            _logger = logger;
            _random = new Random();
        }

        private Task _sendMail()
        {
            int randNext = _random.Next(1, 10);
            
            _logger.LogError($"waiting {randNext} seconds");
            return Task.Delay(TimeSpan.FromSeconds(randNext), CancellationToken.None);
        }

        protected override async Task Execute(CancellationToken stoppingToken) => await _sendMail();


        #pragma warning disable 1998
        protected override async Task OnSuccess()
        {
            _logger.LogError($"send mail succeeded");
        }
        protected override async Task OnTimeout()
        {
            _logger.LogError($"send mail failed");
        }
        protected override async Task OnError<T>(T exception)
        {
            _logger.LogError(exception.Message);
        }
        #pragma warning restore 1998

    }
}
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Jaxofy.BackgroundServices
{
    public abstract class PeriodicBackgroundService : BackgroundService
    {
        private class ServiceSettings : PeriodicServiceSettings
        {
            public static PeriodicServiceSettings Default => new ServiceSettings();
        }

        private readonly PeriodicServiceSettings _serviceSettings;

        protected PeriodicBackgroundService()
        {
            _serviceSettings = ServiceSettings.Default;
        }

        protected PeriodicBackgroundService(PeriodicServiceSettings serviceSettings)
        {
            _serviceSettings = serviceSettings;
        }

        protected abstract Task Execute(CancellationToken stoppingToken);

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(_serviceSettings.Frequency, stoppingToken);

                try
                {
                    Task pingTask = Execute(stoppingToken);

                    if (_serviceSettings.TimeOut.HasValue)
                    {
                        Task cancelTask = Task.Delay(_serviceSettings.TimeOut.Value, stoppingToken);

                        //double await so exceptions from either task will bubble up
                        await await Task.WhenAny(pingTask, cancelTask);
                    }
                    else
                    {
                        await pingTask;
                    }

                    if (pingTask.IsCompletedSuccessfully)
                    {
                        await OnSuccess();
                    }
                    else
                    {
                        await OnTimeout();
                    }
                }
                catch (Exception ex)
                {
                    await OnError(ex);
                }
            }
        }
    }
}
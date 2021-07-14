using System;

namespace DasTeamRevolution.BackgroundServices
{
    public abstract class PeriodicServiceSettings
    {
        public TimeSpan Frequency { get; protected set; }
        public TimeSpan? TimeOut { get; protected set; }
    }
}
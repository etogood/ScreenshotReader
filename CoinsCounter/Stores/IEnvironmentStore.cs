using System;

namespace CoinsCounter.Stores
{
    interface IEnvironmentStore
    {
        public string? Environment { get; set; }

        event Action? EnvironmentUpdated;
    }
}

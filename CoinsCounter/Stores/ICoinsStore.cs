using System;
using System.Collections.Generic;

namespace CoinsCounter.Stores
{
    public interface ICoinsStore
    {
        void UpdateCoinsList();
        Dictionary<string, int> CoinsCount { get; set; }

        event Action CoinAdded;
    }
}

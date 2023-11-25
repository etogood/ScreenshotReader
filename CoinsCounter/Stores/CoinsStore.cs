using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;

namespace CoinsCounter.Stores
{
    class CoinsStore : ICoinsStore
    {
        private readonly IHost _host;
        public CoinsStore(IHost host)
        {
            _host = host;
            UpdateCoinsList();
        }
        public void UpdateCoinsList()
        {
            if (!File.Exists(File.ReadAllText(@".env")))
            {
                OpenFileDialog openFileDialog = new();
                if (openFileDialog.ShowDialog() == true)
                    File.WriteAllText(@".env", openFileDialog.FileName);
            }
            _host.Services.GetRequiredService<IEnvironmentStore>().Environment = File.ReadAllText(@".env");
            var coins = File.ReadAllLines(_host.Services.GetRequiredService<IEnvironmentStore>().Environment!);
            CoinsCount.Clear();
            foreach (var item in coins)
            {
                var str = item.Split(":");
                CoinsCount.Add(str[0], int.Parse(str[1]));
            }
        }

        private Dictionary<string, int> _coinsCount = new();
        public Dictionary<string, int> CoinsCount 
        {
            get => _coinsCount;
            set
            {
                _coinsCount = value;
                CoinAdded?.Invoke();
            }
        }

        public event Action? CoinAdded;
    }
}

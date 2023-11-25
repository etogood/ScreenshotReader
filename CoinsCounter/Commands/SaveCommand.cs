using CoinsCounter.Commands.Base;
using CoinsCounter.Stores;
using CoinsCounter.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Win32;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Documents;

namespace CoinsCounter.Commands
{
    class SaveCommand : Command
    {
        private readonly IHost _host;
        public SaveCommand(IHost host)
        {
            _host = host;
        }

        public override bool CanExecute(object? parameter) => 
            _host.Services.GetRequiredService<MainWindowViewModel>().ImageText != string.Empty;

        public override void Execute(object? parameter)
        {
            var env = _host.Services.GetRequiredService<IEnvironmentStore>().Environment;
            var coins = _host.Services.GetRequiredService<ICoinsStore>().CoinsCount;
            var text = _host.Services.GetRequiredService<MainWindowViewModel>().ImageText;

            if (string.IsNullOrWhiteSpace(text)) return;

            var coinsList = text.Split("\r\n\r\n");
            
            foreach (var item in coinsList)
            {
                if (coins.ContainsKey(item)) coins[item]++;
                else coins.Add(item, 1);
            }

            List<string> coinsStringArray = new();

            foreach (var item in coins)
            {
                coinsStringArray.Add(item.Key + ":" + item.Value);
            }

            File.WriteAllLines(env, coinsStringArray);
        }
    }
}

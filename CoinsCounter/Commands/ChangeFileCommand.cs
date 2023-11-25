using CoinsCounter.Commands.Base;
using CoinsCounter.Stores;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Win32;
using System.Drawing;
using System.IO;

namespace CoinsCounter.Commands
{
    class ChangeFileCommand : Command
    {
        private readonly IHost _host;

        public ChangeFileCommand(IHost host)
        {
            _host = host;
        }
        public override bool CanExecute(object? parameter) => true;

        public override void Execute(object? parameter)
        {
            OpenFileDialog openFileDialog = new();
            if (openFileDialog.ShowDialog() == true)
                File.WriteAllText(@".env", openFileDialog.FileName);
            _host.Services.GetRequiredService<ICoinsStore>().UpdateCoinsList();
        }
    }
}

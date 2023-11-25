using CoinsCounter.Commands.Base;
using CoinsCounter.ViewModels;
using IronOcr;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

namespace CoinsCounter.Commands
{
    class UpdateImageCommand : Command
    {
        private readonly IHost _host;
        public UpdateImageCommand(IHost host)
        {
            _host = host;
        }
        public override bool CanExecute(object? parameter) => Clipboard.ContainsImage();

        public override void Execute(object? parameter) 
        { 
            var image = Clipboard.GetImage();

            _host.Services.GetRequiredService<MainWindowViewModel>().CurrentImage = Clipboard.GetImage();

            Bitmap bitmap;
            using (var outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(image));
                enc.Save(outStream);
                bitmap = new Bitmap(outStream);
            }

            _host.Services.GetRequiredService<MainWindowViewModel>().ImageText = new IronTesseract().Read(bitmap).Text;
        }
    }
}

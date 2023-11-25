using CoinsCounter.Commands.Base;
using CoinsCounter.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Windows;
using Tesseract;
using System.Windows.Forms;
using System.Drawing;
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
            _host.Services.GetRequiredService<MainWindowViewModel>().CurrentImage = Clipboard.GetImage();

            byte[] data;
            JpegBitmapEncoder encoder = new();
            encoder.Frames.Add(BitmapFrame.Create(Clipboard.GetImage()));
            using (MemoryStream ms = new())
            {
                encoder.Save(ms);
                data = ms.ToArray();
            }

            using var engine = new TesseractEngine(@"./tessdata", "Latin", EngineMode.Default);
            using var img = Pix.LoadFromMemory(data);
            using var page = engine.Process(img);
            var text = page.GetText();

            _host.Services.GetRequiredService<MainWindowViewModel>().ImageText = text;
        }
    }
}

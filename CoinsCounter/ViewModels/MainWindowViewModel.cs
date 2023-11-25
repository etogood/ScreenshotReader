using CoinsCounter.Commands;
using CoinsCounter.Stores;
using CoinsCounter.ViewModels.Base;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Windows.Input;
using System.Windows.Media;

namespace CoinsCounter.ViewModels
{
    public class MainWindowViewModel : ViewModel
    {
        private readonly IHost _host;

        private readonly ICoinsStore _coinsStore;
        private readonly IEnvironmentStore _environmentStore;
        public ICommand UpdateImageCommand { get; }
        public ICommand ChangeFileCommand { get; }
        public ICommand SaveCommand { get; }

        public Dictionary<string, int> CurrentCoinsCount => _coinsStore.CoinsCount;

        private ImageSource? _currentImage;

        public ImageSource? CurrentImage
        {
            get => _currentImage;
            set => Set(ref _currentImage, value);
        }

        private string? _imageText;

        public string? ImageText
        {
            get => _imageText;
            set => Set(ref _imageText, value);
        }


        public string? SelectedDestination => _host.Services.GetRequiredService<IEnvironmentStore>().Environment;

        public MainWindowViewModel(IHost host)
        {
            _host = host;

            _environmentStore = host.Services.GetRequiredService<IEnvironmentStore>();
            _environmentStore.EnvironmentUpdated += OnEnvironmentUpdated;

            _coinsStore = host.Services.GetRequiredService<ICoinsStore>();
            _coinsStore.CoinAdded += OnCoinAdded;

            UpdateImageCommand = host.Services.GetRequiredService<UpdateImageCommand>();
            SaveCommand = host.Services.GetRequiredService<SaveCommand>();
            ChangeFileCommand = host.Services.GetRequiredService<ChangeFileCommand>();

        }

        private void OnCoinAdded()
        {
            OnPropertyChanged(nameof(CurrentCoinsCount));
        }

        private void OnEnvironmentUpdated()
        {
            OnPropertyChanged(nameof(SelectedDestination));
        }
    }
}
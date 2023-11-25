using Microsoft.Win32;
using System;
using System.IO;

namespace CoinsCounter.Stores
{
    class EnvironmentStore : IEnvironmentStore
    {
        public EnvironmentStore()
        {
            if (!File.Exists(@".env"))
            {
                OpenFileDialog openFileDialog = new();
                if (openFileDialog.ShowDialog() == true)
                    File.WriteAllText(@".env", openFileDialog.FileName);
            }

            Environment = File.ReadAllText(@".env");
        }

        private string? _environment;
        public string? Environment
        {
            get => _environment;
            set
            {
                _environment = value;
                EnvironmentUpdated?.Invoke();
            }
        }

        public event Action? EnvironmentUpdated;
    }
}

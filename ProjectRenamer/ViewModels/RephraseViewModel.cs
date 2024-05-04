using CommunityToolkit.Mvvm.Input;
using Microsoft.Toolkit.Uwp.Notifications;
using ProjectRenamer.Models;
using ProjectRenamer.Services;
using System.ComponentModel;
using System.Windows.Input;

namespace ProjectRenamer.ViewModels
{
    public class RephraseViewModel : INotifyPropertyChanged
    {
        private readonly RephraseService _service;
        public RephraseSettings Settings { get; set; } = new RephraseSettings();
        public ICommand RephraseCommand { get; init; }

        public RephraseViewModel()
        {
            _service = new RephraseService();
            RephraseCommand = new RelayCommand(() =>
            {
                try
                {
                    _service.Rephrase(Settings);
                    
                    new ToastContentBuilder()
                        .AddText("Successfully renamed!")
                        .Show();
                }
                catch(Exception ex) 
                {
                    new ToastContentBuilder()
                        .AddText("Error occurred")
                        .AddText(ex.Message)
                        .Show();
                }
            });
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged is null) return;
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

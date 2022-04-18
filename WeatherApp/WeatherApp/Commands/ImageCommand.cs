using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Notifications;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace WeatherApp.Commands
{
    public class ImageCommand : ICommand
    {
        private Viewmodel.WeatherViewModel weatherViewModel;
        public event EventHandler CanExecuteChanged;

        public ImageCommand(Viewmodel.WeatherViewModel weatherViewModel)
        {
            this.weatherViewModel = weatherViewModel;
        }

        public void Fire_CanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
        public bool CanExecute(object parameter)
        {
            return weatherViewModel != null;
        }

        public async void Execute(object parameter)
        {
            string popupMsg = "";
            if (weatherViewModel.SelectedPlace.tempC >= 18)
            {
                popupMsg = "It's a beautiful sunny day!";
            }
            else if(weatherViewModel.SelectedPlace.tempC <= 0)
            {
                popupMsg = "It's cold today! Keep warm!";
            }
            else
            {
                popupMsg = "It's a cloudy day :(";
            }

            ContentDialog weatherStatus = new ContentDialog()
            {
                Title = "Current Weather Status",
                Content = popupMsg,
                PrimaryButtonText = "OK"
            };
            await weatherStatus.ShowAsync();
        }
    }
}

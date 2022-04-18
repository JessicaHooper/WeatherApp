using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using WeatherApp.Models;
using WeatherApp.Viewmodel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace WeatherApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DetailsPage : Page
    {
        internal static WeatherViewModel wvm;
        private Viewmodel.WeatherViewModel weatherViewModel;
        private WeatherModel selectedPlace;

        public DetailsPage()
        {
            this.InitializeComponent();
            this.weatherViewModel = wvm;

        }

        /*        public DetailsPage(WeatherViewModel weatherViewModel)
                {
                    if (weatherViewModel.SelectedPlace != null)
                    {
                        this.weatherViewModel = weatherViewModel;
                        Debug.WriteLine(weatherViewModel.SelectedPlaceName);
                        SelectedPlaceName.Text = weatherViewModel.SelectedPlaceName;
                    }
                }*/




        //Manage page navigation
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            SystemNavigationManager.GetForCurrentView().BackRequested += OnBackRequested;
            base.OnNavigatedTo(e);
            WeatherViewModel item = e.Parameter as WeatherViewModel;
        }

        private void OnBackRequested(object sender, BackRequestedEventArgs backRequestedEventArgs)
        {
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
            backRequestedEventArgs.Handled = true;
        }
    }
}

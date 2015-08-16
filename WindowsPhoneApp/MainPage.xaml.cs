using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Services.Maps;
using Windows.Devices.Geolocation;
using Windows.UI.Popups;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace WindowsPhoneApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;
            readCurrentLocation();

        }


        private async void readCurrentLocation()
        {
            Geolocator locator = new Geolocator();

            if (locator.LocationStatus == PositionStatus.Disabled)
            {
                MessageDialog msg = new MessageDialog("GPS is not enabled.");
                await msg.ShowAsync();
            }
            else
            {
                Geoposition position = await locator.GetGeopositionAsync();

                BasicGeoposition queryHint = new BasicGeoposition();
                queryHint.Latitude = position.Coordinate.Latitude;
                queryHint.Longitude = position.Coordinate.Longitude;

                var result = await MapLocationFinder.FindLocationsAsync("Via busseto 15, Riccione", new Geopoint(queryHint));

                // Get the coordinates
                if (result.Status == MapLocationFinderStatus.Success)
                {
                    double lat = result.Locations[0].Point.Position.Latitude;
                    double lon = result.Locations[0].Point.Position.Longitude;

                    MessageDialog msg = new MessageDialog("Latitude: " + lat + "Longitude: "+lon);
                    await msg.ShowAsync();
                }
            }
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
                      
        }

        private void GoToMapPage(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MapPage));
        }

        private void RegisterButton_Pressed(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(RegistrationPage));
        }

        private void LoginButton_Pressed(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(LoginPage));
        }
      
        
    }
}

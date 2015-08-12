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
using Windows.Devices.Geolocation;
using Windows.UI.Xaml.Controls.Maps;
using Windows.Storage.Streams;

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
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: Prepare page for display here.

            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.
            readCurrentLocation();
            
        }

        private void addMarker(Geoposition position)
        {
            MapIcon icon = new MapIcon();
            icon.Title = "My position";
            icon.Location = position.Coordinate.Point;
            icon.Image = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/Square71x71Logo.scale-240.png"));
        }

        private async void readCurrentLocation()
        {

            /*var gl = new Geolocator() { DesiredAccuracy = PositionAccuracy.High };
            Geoposition location = await gl.GetGeopositionAsync(TimeSpan.FromMinutes(5), TimeSpan.FromSeconds(5));
            var pin = new MapIcon()
            {
                Location = location.Coordinate.Point,
                Title = "You are here",
                NormalizedAnchorPoint = new Point() { X = 0, Y = 0 },
            };
            MapControl.MapElements.Add(pin);
            await MapControl.TrySetViewAsync(location.Coordinate.Point, 12);*/
            Geolocator locator = new Geolocator();
            Geoposition position = await locator.GetGeopositionAsync();
           /* MapControl.Center = new Geopoint(new BasicGeoposition()
            {
                Latitude = position.Coordinate.Point.Position.Latitude,
                Longitude = position.Coordinate.Point.Position.Longitude
            });*/
            MapControl.ZoomLevel = 12;
            MapControl.LandmarksVisible = true;
            MapIcon icon = new MapIcon();
            icon.Title = "My position";
            icon.Location = position.Coordinate.Point;
            // addMarker(position);
            MapControl.MapElements.Add(icon);
            MapControl.Center = position.Coordinate.Point;
            await MapControl.TrySetViewAsync(position.Coordinate.Point,15);
            
            
            //this.Dispatcher.RunAsync(startPositionChangeListener(locator));
        }

        private void startPositionChangeListener(Geolocator locator)
        {
            locator.MovementThreshold = 100;
            locator.ReportInterval = 5 * 1000;
            locator.DesiredAccuracy = PositionAccuracy.High;
        }
    }
}

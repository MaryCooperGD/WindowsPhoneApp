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
using Windows.UI.Popups;
using Windows.UI.Core;
using Windows.Services.Maps;
using Parse;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace WindowsPhoneApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    /// 

    public sealed partial class MapPage : Page
    {

        private Geoposition pos = null;
        private int distance = 1;

        public MapPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
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
                pos = position;

                buildMap();

                myMapControl.ZoomLevel = 12;
                myMapControl.LandmarksVisible = true;
                MapIcon icon = new MapIcon()
                {
                    Location = position.Coordinate.Point,
                    Title = "You are here",
                    NormalizedAnchorPoint = new Point() { X = 0, Y = 0 },
                };
                icon.Title = "My position";
                icon.Location = position.Coordinate.Point;
                myMapControl.MapElements.Add(icon);
                myMapControl.Center = position.Coordinate.Point;
                await myMapControl.TrySetViewAsync(position.Coordinate.Point, 15);

                await this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    startPositionChangeListener(locator);
                });



                /*
                
                //position finder via adress for db
                Geoposition pos = await locator.GetGeopositionAsync();

                BasicGeoposition queryHint = new BasicGeoposition();
                queryHint.Latitude = pos.Coordinate.Latitude;
                queryHint.Longitude = pos.Coordinate.Longitude;

                var result = await MapLocationFinder.FindLocationsAsync("Via Santo Stefano 21, Trecastelli", new Geopoint(queryHint), 3);

                // Get the coordinates
                if (result.Status == MapLocationFinderStatus.Success)
                {
                    MapIcon icon2 = new MapIcon()
                    {

                    };
                    icon2.Title = "food";

                    double lat = result.Locations[0].Point.Position.Latitude;
                    double lon = result.Locations[0].Point.Position.Longitude;

                    icon2.Location = new Geopoint(new BasicGeoposition()
                    {
                        Latitude = lat,
                        Longitude = lon
                    });
                    myMapControl.MapElements.Add(icon2);
                    

                   // MessageDialog msg = new MessageDialog("Latitude: " + lat + "Longitude: " + lon);
                    //await msg.ShowAsync();
                }
                */
            }
        }

        private void startPositionChangeListener(Geolocator locator)
        {
            locator.MovementThreshold = 5;
            locator.ReportInterval = 5 * 1000;
            locator.DesiredAccuracy = PositionAccuracy.High;
        }

        private async void Slider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            
            int value = (int)e.NewValue;
            distance = value;
            MessageDialog msg = new MessageDialog("Range of " +value+" km.");
            await msg.ShowAsync();
            buildMap();
        }

        private async void buildMap()
        {
            if(pos == null)
                return;


            myMapControl.MapElements.Clear();
            try
            {
                //per il momento nessun check sull'ora
                var query = from places in ParseObject.GetQuery(RegistrationManager.ParseName)
                            where places.Get<double>("lat") != 0
                            select places;
                IEnumerable<ParseObject> results = await query.FindAsync();

                

                foreach (ParseObject obj in results)
                {
                    
                    if (DistanceTo(obj.Get<double>("lat"), obj.Get<double>("lng"), pos.Coordinate.Latitude, pos.Coordinate.Longitude) < distance)
                    {
                        MapIcon icon = new MapIcon() { };
                        RegistrationManager.getInstance().reset(obj);
                        icon.Title = RegistrationManager.getInstance().LocalName;
                        icon.Location = new Geopoint(new BasicGeoposition()
                        {
                            Latitude = RegistrationManager.getInstance().Lat,
                            Longitude = RegistrationManager.getInstance().Lng
                        });
                        myMapControl.MapElements.Add(icon);
                    }

                    
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }

        }

        public double DistanceTo(double lat1, double lon1, double lat2, double lon2)
        {
            double rlat1 = Math.PI * lat1 / 180;
            double rlat2 = Math.PI * lat2 / 180;
            double theta = lon1 - lon2;
            double rtheta = Math.PI * theta / 180;
            double dist =
                Math.Sin(rlat1) * Math.Sin(rlat2) + Math.Cos(rlat1) *
                Math.Cos(rlat2) * Math.Cos(rtheta);
            dist = Math.Acos(dist);
            dist = dist * 180 / Math.PI;
            dist = dist * 60 * 1.1515;

           
           return dist * 1.609344;

        }

    }
}

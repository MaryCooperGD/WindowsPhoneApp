using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Services.Maps;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace WindowsPhoneApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SecondRegistrationPage : Page
    {
        List<String> daysList = new List<String>();
        int times;
        private RegistrationManager manager = RegistrationManager.getInstance();
        private String dayToModify;
        private bool modifying;
        
        public SecondRegistrationPage()
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
            times = 0;
            days.Text = "Monday";
            if (e.Parameter !=null)
            {
                dayToModify = (String)e.Parameter;
                modifying = true;
                days.Text = dayToModify;
                registrationVariant.Visibility = Visibility.Collapsed;
                modifyVariant.Visibility = Visibility.Visible;
                modifyVariant.Text = modifyVariant.Text + dayToModify;
            }
           
            
        }

        private async void DayOff_Checked(object sender, RoutedEventArgs e)
        {
            MessageDialog msg = new MessageDialog("This is your day off");
            await msg.ShowAsync();
        }

        private void times_submitted(object sender, RoutedEventArgs e)
        {
            if (modifying)
            {
                //TODO submit: save modified data on db
                Frame.GoBack();
            }
            else if (!modifying)
            {
                switch (times)
                {
                    case 0:
                        days.Text = "Tuesday";
                        manager.addDayTime(RegistrationManager.DayOfWeek.MON.ToString(), new DayTimespan(openingAMTimePicker.Time, closingAMTimePicker.Time,
                            openingPMTimePicker.Time, closingPMTimePicker.Time));
                        break;
                    case 1:
                        days.Text = "Wednesday";
                        manager.addDayTime(RegistrationManager.DayOfWeek.TUE.ToString(), new DayTimespan(openingAMTimePicker.Time, closingAMTimePicker.Time,
                           openingPMTimePicker.Time, closingPMTimePicker.Time));
                        break;
                    case 2:
                        days.Text = "Thursday";
                        manager.addDayTime(RegistrationManager.DayOfWeek.WED.ToString(), new DayTimespan(openingAMTimePicker.Time, closingAMTimePicker.Time,
                           openingPMTimePicker.Time, closingPMTimePicker.Time));
                        break;
                    case 3:
                        days.Text = "Friday";
                        manager.addDayTime(RegistrationManager.DayOfWeek.THU.ToString(), new DayTimespan(openingAMTimePicker.Time, closingAMTimePicker.Time,
                           openingPMTimePicker.Time, closingPMTimePicker.Time));
                        break;
                    case 4:
                        days.Text = "Saturday";
                        manager.addDayTime(RegistrationManager.DayOfWeek.FRI.ToString(), new DayTimespan(openingAMTimePicker.Time, closingAMTimePicker.Time,
                           openingPMTimePicker.Time, closingPMTimePicker.Time));
                        break;
                    case 5:
                        days.Text = "Sunday";
                        manager.addDayTime(RegistrationManager.DayOfWeek.SAT.ToString(), new DayTimespan(openingAMTimePicker.Time, closingAMTimePicker.Time,
                           openingPMTimePicker.Time, closingPMTimePicker.Time));
                        break;

                }
                times++;
                dayOffBox.IsChecked = false;
                if (times == 7)
                {
                    manager.addDayTime(RegistrationManager.DayOfWeek.SUN.ToString(), new DayTimespan(openingAMTimePicker.Time, closingAMTimePicker.Time,
                           openingPMTimePicker.Time, closingPMTimePicker.Time));
                    Frame.Navigate(typeof(ThirdRegistrationPage));
                }
            }
            
                
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
                //position finder via adress
                Geoposition pos = await locator.GetGeopositionAsync();

                BasicGeoposition queryHint = new BasicGeoposition();
                queryHint.Latitude = pos.Coordinate.Latitude;
                queryHint.Longitude = pos.Coordinate.Longitude;

                var result = await MapLocationFinder.FindLocationsAsync(manager.address, new Geopoint(queryHint), 3);

                // Get the coordinates
                if (result.Status == MapLocationFinderStatus.Success)
                {
                    manager.Lat = result.Locations[0].Point.Position.Latitude;
                    manager.Lng = result.Locations[0].Point.Position.Longitude;

                    /* 
                    MessageDialog msg = new MessageDialog("Latitude: " + lat + "Longitude: " + lon);
                    await msg.ShowAsync();
                    */
                }

            }
        }
       
    }
}

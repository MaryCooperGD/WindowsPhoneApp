using Parse;
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
        private bool mChecked;
        
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
            else
            {
               readCurrentLocation();
            }
           
            
        }

        private async void DayOff_Checked(object sender, RoutedEventArgs e)
        {
            MessageDialog msg = new MessageDialog("This is your day off");
            await msg.ShowAsync();
            mChecked = true;
        }

        private async void saveModify(RegistrationManager.DayOfWeek dayOfWeek)
        {
            //l'errore risiede nel fatto che va modificato il parse object, non il reg manager! Altrimenti nasce una nuova copia i oggetto!

            DayTimespan newspan = new DayTimespan(openingAMTimePicker.Time, closingAMTimePicker.Time,
                            openingPMTimePicker.Time, closingPMTimePicker.Time, mChecked);

            ParseObject account = RegistrationManager.getInstance().currAccount;

            IDictionary<string, string> dict = account.Get<IDictionary<string, string>>("dictionary");

            if (dict.ContainsKey(dayOfWeek.ToString()))
            {
                dict.Remove(dayOfWeek.ToString());
                string obj = Newtonsoft.Json.JsonConvert.SerializeObject(newspan);
                dict.Add(dayOfWeek.ToString(), obj);
                account["dictionary"] = dict;
                await account.SaveAsync();
                return;
            }

            RegistrationManager.getInstance().changeDayTime(dayOfWeek.ToString(), new DayTimespan(openingAMTimePicker.Time, closingAMTimePicker.Time,
                            openingPMTimePicker.Time, closingPMTimePicker.Time, mChecked));
            await RegistrationManager.getInstance().getParseObject().SaveAsync();
        }

        private void times_submitted(object sender, RoutedEventArgs e)
        {
            if (modifying)
            {
                switch (dayToModify)
                {
                    case "Monday":
                        saveModify(RegistrationManager.DayOfWeek.MON);
                        break;
                    case "Tuesday":
                        saveModify(RegistrationManager.DayOfWeek.TUE);
                        break;
                    case "Wednesday":
                        saveModify(RegistrationManager.DayOfWeek.WED);
                        break;
                    case "Thursday":
                        saveModify(RegistrationManager.DayOfWeek.THU);
                        break;
                    case "Friday":
                        saveModify(RegistrationManager.DayOfWeek.FRI);
                        break;
                    case "Saturday":
                        saveModify(RegistrationManager.DayOfWeek.SAT);
                        break;
                    case "Sunday":
                        saveModify(RegistrationManager.DayOfWeek.SUN);
                        break;
                }
                Frame.GoBack();
            }
            else if (!modifying)
            {
                

                switch (times)
                {
                    case 0:
                        days.Text = "Tuesday";
                        manager.addDayTime(RegistrationManager.DayOfWeek.MON.ToString(), new DayTimespan(openingAMTimePicker.Time, closingAMTimePicker.Time,
                            openingPMTimePicker.Time, closingPMTimePicker.Time,mChecked));
                        break;
                    case 1:
                        days.Text = "Wednesday";
                        manager.addDayTime(RegistrationManager.DayOfWeek.TUE.ToString(), new DayTimespan(openingAMTimePicker.Time, closingAMTimePicker.Time,
                           openingPMTimePicker.Time, closingPMTimePicker.Time,mChecked));
                        break;
                    case 2:
                        days.Text = "Thursday";
                        manager.addDayTime(RegistrationManager.DayOfWeek.WED.ToString(), new DayTimespan(openingAMTimePicker.Time, closingAMTimePicker.Time,
                           openingPMTimePicker.Time, closingPMTimePicker.Time,mChecked));
                        break;
                    case 3:
                        days.Text = "Friday";
                        manager.addDayTime(RegistrationManager.DayOfWeek.THU.ToString(), new DayTimespan(openingAMTimePicker.Time, closingAMTimePicker.Time,
                           openingPMTimePicker.Time, closingPMTimePicker.Time,mChecked));
                        break;
                    case 4:
                        days.Text = "Saturday";
                        manager.addDayTime(RegistrationManager.DayOfWeek.FRI.ToString(), new DayTimespan(openingAMTimePicker.Time, closingAMTimePicker.Time,
                           openingPMTimePicker.Time, closingPMTimePicker.Time,mChecked));
                        break;
                    case 5:
                        days.Text = "Sunday";
                        manager.addDayTime(RegistrationManager.DayOfWeek.SAT.ToString(), new DayTimespan(openingAMTimePicker.Time, closingAMTimePicker.Time,
                           openingPMTimePicker.Time, closingPMTimePicker.Time,mChecked));
                        break;

                }
                times++;
                dayOffBox.IsChecked = false;
                if (times == 7)
                {
                    manager.addDayTime(RegistrationManager.DayOfWeek.SUN.ToString(), new DayTimespan(openingAMTimePicker.Time, closingAMTimePicker.Time,
                           openingPMTimePicker.Time, closingPMTimePicker.Time,mChecked));
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
                    try
                    {
                        manager.Lat = result.Locations[0].Point.Position.Latitude;
                        manager.Lng = result.Locations[0].Point.Position.Longitude;


                        //MessageDialog msg = new MessageDialog("Latitude: " + manager.Lat + "Longitude: " + manager.Lng);
                        //await msg.ShowAsync();
                    }
                    catch (Exception e)
                    {
                        System.Diagnostics.Debug.WriteLine("hueeeee");
                    }
                }

            }
        }
       
    }
}

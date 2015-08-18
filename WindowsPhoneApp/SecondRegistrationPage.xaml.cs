using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
        }

        private async void DayOff_Checked(object sender, RoutedEventArgs e)
        {
            MessageDialog msg = new MessageDialog("This is your day off");
            await msg.ShowAsync();
        }

        private void times_submitted(object sender, RoutedEventArgs e)
        {
            switch(times)
            {
                case 0:
                    days.Text = "Tuesday";
                    break;
                case 1:
                    days.Text = "Wednesday";
                    break;
                case 2:
                    days.Text = "Thursday";
                    break;
                case 3:
                    days.Text = "Friday";
                    break;
                case 4:
                    days.Text = "Saturday";
                    break;
                case 5:
                    days.Text = "Sunday";
                    break;
                   
            }
            times++;
            dayOffBox.IsChecked = false;
            if (times==7)
                 {
                     Frame.Navigate(typeof(ThirdRegistrationPage));
                 }
                
        }
       
    }
}

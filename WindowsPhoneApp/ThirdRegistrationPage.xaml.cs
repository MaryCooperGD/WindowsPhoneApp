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
    public sealed partial class ThirdRegistrationPage : Page
    {
        public ThirdRegistrationPage()
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

        }

        private async void submit_final(object sender, RoutedEventArgs e)
        {
            MessageDialog msg;
            if(string.IsNullOrEmpty(usr.Text) || string.IsNullOrEmpty(psw.Password))
            {
               msg  = new MessageDialog("Please, fill all the fields!");
                await msg.ShowAsync();
            }
            else
            {
                msg = new MessageDialog("Thank you for your registration!");
                await msg.ShowAsync();
                //if DB saving went ok -- Need to create here DB Connection!!
                var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
                localSettings.Values["Username"] = usr.Text;
                localSettings.Values["Logged"] = true;        
                Frame.Navigate(typeof(MainPage));
                Frame.BackStack.Clear();  
            }
        }
    }
}

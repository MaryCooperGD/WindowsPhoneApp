using Parse;
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
    public sealed partial class LoginPage : Page
    {
        public LoginPage()
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

        private async void enter_login(object sender, RoutedEventArgs e)
        {

            
            //TODO: controllo se esiste già l'ID e in caso bisogna mettere takenID.visibility=visible e non navigare verso un'altra pagina
            if (string.IsNullOrEmpty(UserID.Text) || string.IsNullOrEmpty(UserPSW.Password))
            {
                MessageDialog msg = new MessageDialog("Please, fill all the fields.");
                await msg.ShowAsync();
            }
            else
            {
                

                var query = from account in ParseObject.GetQuery(RegistrationManager.ParseName)
                            where account.Get<string>("username") == UserID.Text
                            select account;
                IEnumerable<ParseObject> results = await query.FindAsync();

                if (results.Count() != 0)
                {
                    

                    ParseObject account = results.ElementAt(0);

                    if(account.Get<string>("username") != UserID.Text ||
                        (account.Get<string>("username") == UserID.Text && account.Get<string>("password") != UserPSW.Password))
                    {
                        MessageDialog msg = new MessageDialog("Incorrect Username or password.");
                        await msg.ShowAsync();
                    }
                    else 
                    {
                        RegistrationManager manager = RegistrationManager.getInstance();
                        manager.reset(account);
                        //set manually the parse account;
                        manager.currAccount = account;
                        var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
                        localSettings.Values["Username"] = manager.username;
                        localSettings.Values["Logged"] = true;

                        Frame.Navigate(typeof(ProfilePage));
                        Frame.BackStack.RemoveAt(Frame.BackStack.Count() - 1);
                    }

                    
                }
                else
                {
                    MessageDialog msg = new MessageDialog("No username found.");
                    await msg.ShowAsync();
                }

                
            }
            
        }
    }
}

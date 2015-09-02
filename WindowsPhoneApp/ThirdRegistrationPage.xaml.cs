using Parse;
using System;
using System.Diagnostics;
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
                RegistrationManager.getInstance().username = usr.Text;
                RegistrationManager.getInstance().password = psw.Password;
                //if DB saving went ok -- Need to create here DB Connection!!

                //RIFARE! username e password son da salvare sul db + salva manager con tutte le altre informazioni
               /* var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
                localSettings.Values["Username"] = usr.Text;
                localSettings.Values["Logged"] = true;        */

                //prima controllo se non c'è gia quell'username!
                var query = from account in ParseObject.GetQuery(RegistrationManager.ParseName)
                            where account.Get<string>("username") == usr.Text
                            select account;
                IEnumerable<ParseObject> results = await query.FindAsync();

                if (results.Count() != 0)
                {
                    msg = new MessageDialog("Account already exists, please select another username.");
                    usr.Text = "";
                    psw.Password = "";
                    await msg.ShowAsync();
                }
                else
                {
                    //msg = new MessageDialog("Thank you for your registration!");
                    //await msg.ShowAsync();

                    try
                    {
                        ParseObject obj = RegistrationManager.getInstance().getParseObject();
                        await obj.SaveAsync();
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message); //qui accade l'exception, unable to encode DayTimespan
                        //probabilmente dovuto al fatto che Parse lavora solo con alcuni tipi di dato.
                        //Necessario trovare un modo per Codificare i Timespan in ParseObjects(Leggere guida)
                        
                    }

                    Frame.Navigate(typeof(MainPage));
                    Frame.BackStack.Clear(); 
                }



                 
            }
        }
    }
}

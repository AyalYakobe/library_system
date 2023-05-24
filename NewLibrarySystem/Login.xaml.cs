using System;
using BL_n;
using Models_n;
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
using Windows.UI.Popups;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace NewLibrarySystem
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Login : Page
    {
        public Login()
        {
            this.InitializeComponent();
        }

        //A simple login that requires a username and password.
        private void btnEnterID_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtBoxUserName.Text == "Ayal_Yakobe" && txtBoxPassword.Password == "ayaliscool123")
                {
                    this.Frame.Navigate(typeof(MainPage), checkBoxGuest);
                }
                else if (checkBoxGuest.IsChecked == true)
                {
                    this.Frame.Navigate(typeof(GuestPage));
                }
                else
                {
                    throw new IncorrectUsernameOrPasswordException();
                }

            }
            catch (IncorrectUsernameOrPasswordException)
            {

                Alert("Incorrect username or password.");
            }
            catch (Exception)
            {
                Alert("An error has occured.");
            }
        }

        //A simple pop-up.
        private async void Alert(string text)
        {
            MessageDialog msg = new MessageDialog(text);
            await msg.ShowAsync();
        }
    }
}

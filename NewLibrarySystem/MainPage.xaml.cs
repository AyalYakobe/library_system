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
using NewLibrarySystem.VIEWS;
using Windows.UI.Popups;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace NewLibrarySystem
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        BookService bookService;

        public MainPage()
        {
            this.InitializeComponent();
            bookService = new BookService();
        }

        //An interactive pop-up that verfies a user's intentions.
        private async void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            MessageDialog dlg = new MessageDialog("Are you sure you want to delete this item from the library?\nPlay again?", " Book Store");
            UICommand yesCommand = new UICommand("Yes", DeleteBook);
            UICommand noCommand = new UICommand("No");
            dlg.Commands.Add(yesCommand);
            dlg.Commands.Add(noCommand);
            await dlg.ShowAsync();
        }

        //Allows the user to buy a book.
        private async void btnBuy_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await bookService.BuyBook(txtBoxBooks.Text);
                Present();
            }
            catch(BlankTextException)
            {
                Alert("Text is unfilled");
            }
            catch (NotInStockException)
            {
                Alert("Out of stock!");
            }
            catch (NoSuchBookException)
            {
                Alert("The desired book doesn't exist in our directory");
            }
            catch (Exception)
            {
                Alert("An error has occured, our team is looking into this issue.");
            }
        }

        //Loads books fromt the data base onto the list box. The function appears twice so as to compensate for the async lag.
        private async void Present()
        {
            listBoxBooks.ItemsSource = await bookService.ShowAll();
            listBoxBooks.ItemsSource = await bookService.ShowAll();
        }

        //Deletes a book from the data base.
        private async void DeleteBook(IUICommand command)
        {
            try
            {
                await bookService.DeleteBook(txtBoxBooks.Text);
                Present();
                txtBoxBooks.Text = "";
            }
            catch (NoSuchBookException)
            {
                Alert("This book doesn't exist in our system.");
            }
            catch (Exception)
            {
                Alert("An error has occured, our team is working to fix it.");
            }
        }

        //Directs the user to the update, add, and discount page.
        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(AddBook), bookService);
        }

        //Shows all items in the data base.
        private async void btnShowAll_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                var books = await bookService.ShowAll();
                foreach (AbstractItem item in books)
                {
                    item._price = double.Parse(String.Format("{0:0.00}", item._price));

                }
                listBoxBooks.ItemsSource = books;
            }
            catch(NoSuchBookException)
            {
                Alert("The desired book doesn't exist in out system.");
            }
            catch (Exception ex)
            {

                Alert(ex.Message);
            }
        }
        
        //A simple pop-up function.
        private async void Alert(string alert)
        {
            MessageDialog msg = new MessageDialog(alert);
            await msg.ShowAsync();
        }

        //Allows the user to search by author, title, isbn, or genre.
        private async void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            ClearListBox();

            try
            {
                await bookService.SearchBook(txtBoxBooks.Text, listBoxBooks);
            }
            catch(BlankTextException)
            {
                Alert("Space is empty.");
            }
            catch (NoSuchBookException)
            {
                Alert("This book doesn't exist in our system.");
            }
            catch (Exception)
            {
                Alert("An error has occured, our team is working to fix it.");
            }
        }

        //An event that clears everything from the list box.
        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            ClearListBox();
        }

        //The actual function that clears items from the list box.
        private void ClearListBox()
        {
            listBoxBooks.ItemsSource = null;
            listBoxBooks.Items.Clear();
        }
    }
}

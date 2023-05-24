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
    public sealed partial class GuestPage : Page
    {
        readonly BookService bookService;
        public GuestPage()
        {
            bookService = new BookService();
            this.InitializeComponent();
        }

        //Allows the user to buy a book.
        private async void btnBuy_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await bookService.BuyBook(txtBoxBooks.Text);
                Present();
            }
            catch (BlankTextException)
            {
                Alert("Space is blank.");
            }
            catch (NotInStockException)
            {
                Alert("Out of stock!");
            }
            catch (NoSuchBookException)
            {
                Alert("The desired doesn't exist in our directory");
            }
            catch (Exception)
            {
                Alert("An error has occured, our team is looking into this issue.");
            }
        }

        //Shows all books in the data base.
        private async void btnShowAll_Click(object sender, RoutedEventArgs e)
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
            catch (Exception ex)
            {

                Alert(ex.Message);
            }
        }

        //Loads books fromt the data base onto the list box. The function appears twice so as to compensate for the async lag.
        private async void Present()
        {
            listBoxBooks.ItemsSource = await bookService.ShowAll();
            listBoxBooks.ItemsSource = await bookService.ShowAll();
        }

        //A simple pop-up function.
        private async void Alert(string alert)
        {
            MessageDialog msg = new MessageDialog(alert);
            await msg.ShowAsync();
        }

        //Searches the data base for a specific book, author, genre, or isbn.
        private async void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            ClearListBox();

            try
            {
                await bookService.SearchBook(txtBoxBooks.Text, listBoxBooks);
            }
            catch(BlankTextException)
            {
                Alert("Space is blank.");
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

        //An ecent that clears elements on the list box.
        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            ClearListBox();
        }

        //The actual function that clears elements from a list box.
        private void ClearListBox()
        {
            listBoxBooks.ItemsSource = null;
            listBoxBooks.Items.Clear();
        }
    }
}

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

namespace NewLibrarySystem.VIEWS
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AddBook : Page
    {
        BookService bookService;
        List<TextBox> textBoxList = new List<TextBox>();
        public AddBook()
        {
            this.InitializeComponent();
            radioBtnFiction.IsChecked = true;

            //Determines the visibility of the textboxes at the outset.
            txtBoxContributingAuthors.Visibility = Visibility.Collapsed;
            txtBoxNonFictionAwards.Visibility = Visibility.Collapsed;
            txtBoxGenre.Visibility = Visibility.Collapsed;
            txtBoxTitle.Visibility = Visibility.Collapsed;
            txtBoxPrice.Visibility = Visibility.Collapsed;
            txtBoxAuthor.Visibility = Visibility.Collapsed;
            txtBoxQuantity.Visibility = Visibility.Collapsed;
            txtBoxMovies.Visibility = Visibility.Collapsed;
            txtBoxRealistic.Visibility = Visibility.Collapsed;
        }

        //Transfers the bookService instance from the MainPage without creating a new instance.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            bookService = e.Parameter as BookService;
        }

        //A button that allows the user to return to the previous page.
        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }

        //Adds books to the data base.
        private async void btnEnter_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                if (radioBtnFiction.IsChecked == true)
                {
                    await bookService.AddBookFic(txtBoxTitle.Text, txtBoxAuthor.Text, txtBoxGenre.Text, txtBoxPrice.Text, txtBoxQuantity.Text, txtBoxMovies.Text, txtBoxRealistic.Text);
                    Alert($"{txtBoxTitle.Text} has been added to the inventory.");
                }
                else if (radioBtnNonFiction.IsChecked == true)
                {
                    await bookService.AddBookNonFic(txtBoxTitle.Text, txtBoxAuthor.Text, txtBoxGenre.Text, txtBoxPrice.Text, txtBoxQuantity.Text, txtBoxContributingAuthors.Text, txtBoxNonFictionAwards.Text);
                    Alert($"{txtBoxTitle.Text} has been added to the inventory.");
                }
                BlankText(ref textBoxList);
            }
            catch (BadInputException)
            {
                Alert("Improper input");
            }
            catch (BlankTextException)
            {
                Alert("Not all the required fields were filled out.");

            }
            catch(BookAlreadyExistsException)
            {
                Alert("The book you are trying to add already exists in our systems.");
                BlankText(ref textBoxList);
            }
            catch(Exception)
            {
                Alert("An unusual error has occured, our team is working to resolve the issue.");
            }
        }

        //Updates books in the data base. All relevant fields must be filled for this to work.
        private async void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (radioBtnFiction.IsChecked == true)
                {
                  await bookService.UpdateBookFic(txtBoxTitle.Text, txtBoxAuthor.Text, txtBoxGenre.Text, txtBoxPrice.Text, txtBoxQuantity.Text,
                    txtBoxMovies.Text, txtBoxRealistic.Text);
                    Alert($"The book has been updated");

                }
                else if (radioBtnNonFiction.IsChecked == true)
                {
                    await bookService.UpdateBookNonFic(txtBoxTitle.Text, txtBoxAuthor.Text, txtBoxGenre.Text, txtBoxPrice.Text, txtBoxQuantity.Text,
                    txtBoxContributingAuthors.Text, txtBoxNonFictionAwards.Text);
                    Alert($"The book has been updated");
                }
                LoadTextBox();
                BlankText(ref textBoxList);
            }
            catch (BadInputException)
            {
                Alert("Improper input");
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

        //Executes discounts.
        private async void btnDiscount_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await bookService.Discount(txtBoxDiscountCategory.Text, double.Parse(txtBoxDiscount.Text));
                Alert("Discount executed");
            }
            catch (NoSuchBookException)
            {
                Alert("No such book, isbn, genre, or author");
            }
            catch(BadInputException)
            {
                Alert("Improper input");
            }
            catch(BlankTextException)
            {
                Alert("Text is blank");
            }
            catch(BookDiscountedException)
            {
                Alert("The book is already amply discount.");
            }
            catch (Exception)
            {
                Alert("An unexpected error has occurred in the system. Our team is working to resolve the issue.");
            }
        }

        //Loads the text boxes into a list.
        private void LoadTextBox()
        {
            foreach (var item in grid3.Children)
            {
                TextBox box = item as TextBox;
                if (box != null)
                {
                    textBoxList.Add(box);
                }
            }
        }
        //Sets the value  of the loaded text boxes to "".
        private void BlankText(ref List<TextBox> text)
        {
            foreach (var item in grid3.Children)
            {
                TextBox box = item as TextBox;
                if (box != null)
                {
                    textBoxList.Add(box);
                }
            }

            foreach (TextBox box in text)
            {
                box.Text = "";
            }
        }

        //A simple pop-up function.
        private async void Alert(string alert)
        {
            MessageDialog msg = new MessageDialog(alert);
            await msg.ShowAsync();
        }

        //The following events all allow the user to control the visibility of a given text box.
        private void checkAuthor_Click(object sender, RoutedEventArgs e)
        {
            if (checkAuthor.IsChecked == true)
            {
                txtBoxAuthor.Visibility = Visibility.Visible;
            }
            else if (checkAuthor.IsChecked == false)
            {
                txtBoxAuthor.Visibility = Visibility.Collapsed;
                string.IsNullOrEmpty(txtBoxAuthor.Text);
            }
        }
        private void checkGenre_Click(object sender, RoutedEventArgs e)
        {
            if (checkGenre.IsChecked == true)
            {
                txtBoxGenre.Visibility = Visibility.Visible;
            }
            else if(checkGenre.IsChecked == false)
            {
                txtBoxGenre.Visibility = Visibility.Collapsed;
                string.IsNullOrEmpty(txtBoxGenre.Text);
            }
        }
        private void checkPrice_Click(object sender, RoutedEventArgs e)
        {
            if (checkPrice.IsChecked == true)
            {
                txtBoxPrice.Visibility = Visibility.Visible;
            }
            else if(checkPrice.IsChecked == false)
            {
                txtBoxPrice.Visibility = Visibility.Collapsed;
                string.IsNullOrEmpty(txtBoxPrice.Text);
            }
        }
        private void checkQuantity_Click(object sender, RoutedEventArgs e)
        {
            if (checkQuantity.IsChecked == true)
            {
                txtBoxQuantity.Visibility = Visibility.Visible;
            }
            else if(checkQuantity.IsChecked == false)
            {
                txtBoxQuantity.Visibility = Visibility.Collapsed;
                string.IsNullOrEmpty(txtBoxQuantity.Text);
            }           
        }
        private void checkMovie_Click(object sender, RoutedEventArgs e)
        {
            if (checkMovie.IsChecked == true)
            {
                txtBoxMovies.Visibility = Visibility.Visible;
            }
            else if(checkMovie.IsChecked == false)
            {
                txtBoxMovies.Visibility = Visibility.Collapsed;
                string.IsNullOrEmpty(txtBoxMovies.Text);
            }          
        }
        private void checkRealistic_Click(object sender, RoutedEventArgs e)
        {
            if (checkRealistic.IsChecked == true)
            {
                txtBoxRealistic.Visibility = Visibility.Visible;
            }
            else if(checkRealistic.IsChecked == false)
            {
                txtBoxRealistic.Visibility = Visibility.Collapsed;
                string.IsNullOrEmpty(txtBoxRealistic.Text);
            }       
        }
        private void checkContributing_Click(object sender, RoutedEventArgs e)
        {
            if (checkContributing.IsChecked == true)
            {
                txtBoxContributingAuthors.Visibility = Visibility.Visible;
            }
            else if(checkContributing.IsChecked == false)
            {
                txtBoxContributingAuthors.Visibility = Visibility.Collapsed;
                string.IsNullOrEmpty(txtBoxContributingAuthors.Text);
            }     
        }
        private void checkAwards_Click(object sender, RoutedEventArgs e)
        {
            if (checkAwards.IsChecked == true)
            {
                txtBoxNonFictionAwards.Visibility = Visibility.Visible;
            }
            else if(checkAwards.IsChecked == false)
            {
                txtBoxNonFictionAwards.Visibility = Visibility.Collapsed;
                string.IsNullOrEmpty(txtBoxNonFictionAwards.Text);
            }     
        }
        private void checkTitle_Click(object sender, RoutedEventArgs e)
        {
            if (checkTitle.IsChecked == true)
            {
                txtBoxTitle.Visibility = Visibility.Visible;
            }
            else if(checkTitle.IsChecked == false)
            {
                txtBoxTitle.Visibility = Visibility.Collapsed;
                string.IsNullOrEmpty(txtBoxTitle.Text);
            }         
        }
    }
}

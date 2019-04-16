using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{

    public class Person
    {
        public string Name { get; set; }
        public string Age { get; set; }
        public ImageSource Image { get; set; }
    }
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        public ObservableCollection<Person> Persons { get; set; }

        public MainWindow()
        {

            InitializeComponent();
            Persons = new ObservableCollection<Person>(); 
            DataContext = this;
            listBox.ItemsSource = Persons;
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                if (!String.IsNullOrEmpty(nameText.Text) && !String.IsNullOrEmpty(ageText.Text))
                {
                    Int32.Parse(ageText.Text);
                    Persons.Add(new Person { Name = nameText.Text, Age = ageText.Text, Image = image.Source}); 
                    nameText.Clear();
                    ageText.Clear();
                    image.Source = null;
                } 
                else
                {
                    MessageBox.Show("Please enter your credentials");
                }
            } 
            catch(FormatException)
            {
                MessageBox.Show("Age must be a number!");
            }
            
        }

        private void AddImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Title = "Select a picture";
            open.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
              "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
              "Portable Network Graphic (*.png)|*.png";
            if (open.ShowDialog() == true)
            {
                image.Source = new BitmapImage(new Uri(open.FileName)); 
            }
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            image.Source = Persons[listBox.SelectedIndex].Image;
        }

        private async void FetchDataBtn_Click(object sender, RoutedEventArgs e)
        {
            // fetchTextBox.Text = await getWebPageAsync();
            await task();
        }


        public Task task()
        {
            return Task.Run(() =>
            {
               string MY_API_KEY = "94972b2da817e3ee66b676143c07e31a";
                while (true)
                {
                    string responseText;
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://apilayer.net/api/live?format=1&currencies=EUR&access_key=" + MY_API_KEY);

                    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                    {
                        using (StreamReader responseStream = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8")))
                        {
                            responseText = responseStream.ReadToEnd();
                            Dispatcher.Invoke(() => fetchTextBox.Text = responseText);
                        }
                    }
                    Thread.Sleep(3000);
                    Dispatcher.Invoke(() => fetchTextBox.Clear());
                }
                
            });
        }
    }
}

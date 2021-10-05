using System.Windows;
using System.Windows.Controls;
using MainApp.Pages;

namespace MainApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            BtnFibonacci.IsChecked = true;
        }

        private readonly FibonacciPage _fibonacciPage = new();
        private readonly ListPage _listPage = new();
        private readonly APIPage _apiPage = new();

        private void BtnFibonacci_OnChecked(object sender, RoutedEventArgs e)
        {
            NavigateTo(_fibonacciPage);
        }

        private void BtnList_OnChecked(object sender, RoutedEventArgs e)
        {
            NavigateTo(_listPage);
        }

        private void BtnApi_OnChecked(object sender, RoutedEventArgs e)
        {
            NavigateTo(_apiPage);
        }

        private void NavigateTo(Page page)
        {
            Title = page.Title;
            Navigator.Navigate(page);
        }
    }
}

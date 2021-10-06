using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MainApp.Pages
{
    public partial class FibonacciPage : Page
    {
        public FibonacciPage()
        {
            InitializeComponent();
            BoxDelay.Text = Delay.ToString();
        }

        private static int Delay = 1000;

        private void Delay_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && int.TryParse(BoxDelay.Text, out var nextDelay) && nextDelay != Delay)
            {
                Interlocked.Exchange(ref Delay, nextDelay);
            }
        }

        private void BrnStart_OnClick(object sender, RoutedEventArgs e)
        {
            Output.Text = string.Empty;
            if (!int.TryParse(BoxNumber.Text, out var number) && number < 0) return;
            Thread fibonacciThread = new Thread(() => PositiveFibonacci(number));
            fibonacciThread.Start();
        }

        private void PositiveFibonacci(int number)
        {
            int num1 = 0;
            int num2 = 1;
            for (int i = 0; i <= number; i++)
            {
                int fibonacci = i;
                if (i > 1)
                {
                    fibonacci = num1 + num2;
                    num1 = num2;
                    num2 = fibonacci;
                }


                Display(fibonacci);
                Thread.Sleep(Delay);
            }
        }

        private void Display(int toDisplay)
        {
            Application.Current.Dispatcher.Invoke(() => Output.Text += $"{toDisplay} ");
        }
    }
}

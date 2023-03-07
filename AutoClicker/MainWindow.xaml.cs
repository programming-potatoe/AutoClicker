using Microsoft.UI.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Threading;
using Windows.UI.Core;
using Windows.UI.Input.Preview.Injection;
using System.Diagnostics;
using System.Linq;
using Microsoft.UI.Xaml.Input;
using Windows.System;

namespace AutoClicker
{
    public sealed partial class MainWindow : Window
    {
        private readonly InputInjector injector = InputInjector.TryCreate();
        private Timer timer;
        private bool isRunning = false;
        private int start_delay = 1;
        private int time_between_clicks = 1;

        public MainWindow()
        {
            this.InitializeComponent();
            this.Title = "AutoClicker";
        }


        private async void set_start_delay(object sender, RoutedEventArgs e)
        {
            if (start_delay_textbox.Text.All(char.IsNumber))
            {
                start_delay = int.Parse(start_delay_textbox.Text);
                Debug.WriteLine("delay is now " + start_delay.ToString());
            }
        }

        private async void set_time_between_clicks(object sender, RoutedEventArgs e)
        {
            if (time_between_clicks_textbox.Text.All(char.IsNumber))
            {
                time_between_clicks = int.Parse(time_between_clicks_textbox.Text);
                Debug.WriteLine("time between clicks is now " + time_between_clicks.ToString());
            }
        }

        private async void startButton_Click(object sender, RoutedEventArgs e)
        {
            if (isRunning)
            {
                Debug.WriteLine("stopping the timer");
                timer.Dispose();
                start_button.Content = "Start";
            }
            else {
                start_button.Content = "Stop";
                System.Threading.Thread.Sleep(start_delay*1000);
                Debug.WriteLine("starting the timer");
                timer = new Timer(TimerCallback, null, TimeSpan.Zero, TimeSpan.FromSeconds(time_between_clicks));
            }
            isRunning = !isRunning;
        }

        private async void TimerCallback(object state)
        {
            var input = new InjectedInputMouseInfo();
            input.MouseOptions = InjectedInputMouseOptions.LeftDown;
            injector.InjectMouseInput(new[] { input });

            input.MouseOptions = InjectedInputMouseOptions.LeftUp;
            injector.InjectMouseInput(new[] { input });
            Debug.WriteLine("click");
        }
    }
}
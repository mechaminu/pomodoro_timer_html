using System;
using System.Windows;
using System.Windows.Threading;

namespace PomodoroTimer
{
    public partial class MainWindow : Window
    {
        private readonly TimerService _timer = new TimerService();
        private readonly DispatcherTimer _uiTimer = new DispatcherTimer();

        public MainWindow()
        {
            InitializeComponent();
            TimerText.Text = _timer.GetState().Remaining.ToString(@"mm\:ss");
            _uiTimer.Interval = TimeSpan.FromMilliseconds(250);
            _uiTimer.Tick += (_, _) =>
            {
                var state = _timer.GetState();
                TimerText.Text = state.Remaining.ToString(@"mm\:ss");
            };
            _timer.OnComplete += mode =>
            {
                Dispatcher.Invoke(() =>
                    MessageBox.Show($"{mode} session complete!"));
            };
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            _timer.Start();
            _uiTimer.Start();
        }

        private void Pause_Click(object sender, RoutedEventArgs e)
        {
            _timer.Pause();
            _uiTimer.Stop();
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            _timer.Reset();
            TimerText.Text = _timer.GetState().Remaining.ToString(@"mm\:ss");
        }
    }
}

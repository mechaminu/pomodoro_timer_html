using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace PomodoroTimer
{
    public partial class MainWindow : Window
    {
        private readonly TimerService _timer = new TimerService();
        private readonly DispatcherTimer _uiTimer = new DispatcherTimer();
        private readonly KeyboardMonitor _keyboard = new KeyboardMonitor();
        private readonly List<double> _rateHistory = new();
        private int _lastSecond;
        private DateTime _sessionStart;
        private readonly FocusTracker _focusTracker = new FocusTracker();
        private readonly AfkDetector _afk = new AfkDetector();

        public MainWindow()
        {
            InitializeComponent();
            TimerText.Text = _timer.GetState().Remaining.ToString(@"mm\:ss");
            TypingRateText.Text = "0 keys/min";
            _uiTimer.Interval = TimeSpan.FromMilliseconds(250);
            _uiTimer.Tick += (_, _) =>
            {
                var state = _timer.GetState();
                TimerText.Text = state.Remaining.ToString(@"mm\:ss");
                UpdateTypingRate();
            };
            _timer.OnComplete += mode =>
            {
                Dispatcher.Invoke(() =>
                {
                    if (mode == "work")
                    {
                        _keyboard.Stop();
                        var duration = DateTime.Now - _sessionStart;
                        var avg = _keyboard.TotalKeyPresses / Math.Max(1, duration.TotalMinutes);
                        MessageBox.Show($"Work session complete!\nAvg typing rate: {avg:F1} keys/min");
                    }
                    else
                    {
                        MessageBox.Show($"{mode} session complete!");
                    }
                    _rateHistory.Clear();
                    TypingPolyline.Points.Clear();
                    _keyboard.Reset();
                });
            };
            _afk.AfkStatusChanged += afk =>
            {
                Dispatcher.Invoke(() =>
                    Title = afk ? "Pomodoro Timer (AFK)" : "Pomodoro Timer");
            };
            _afk.Start();
            _focusTracker.Start();
            _lastSecond = DateTime.Now.Second;
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            _timer.Start();
            _uiTimer.Start();
            _keyboard.Reset();
            _keyboard.Start();
            _rateHistory.Clear();
            TypingPolyline.Points.Clear();
            _sessionStart = DateTime.Now;
        }

        private void Pause_Click(object sender, RoutedEventArgs e)
        {
            _timer.Pause();
            _uiTimer.Stop();
            _keyboard.Stop();
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            _timer.Reset();
            TimerText.Text = _timer.GetState().Remaining.ToString(@"mm\:ss");
            _keyboard.Reset();
            _rateHistory.Clear();
            TypingPolyline.Points.Clear();
            TypingRateText.Text = "0 keys/min";
        }

        private void UpdateTypingRate()
        {
            var rate = _keyboard.GetRatePerMinute();
            TypingRateText.Text = $"{rate:F1} keys/min";
            var sec = DateTime.Now.Second;
            if (sec != _lastSecond)
            {
                _lastSecond = sec;
                _rateHistory.Add(rate);
                var totalSeconds = _timer.GetState().Total.TotalSeconds;
                if (totalSeconds <= 0) totalSeconds = 1;
                var x = (_rateHistory.Count / totalSeconds) * TypingGraph.ActualWidth;
                var maxRate = 200.0;
                var y = TypingGraph.Height - Math.Min(rate, maxRate) / maxRate * TypingGraph.Height;
                TypingPolyline.Points.Add(new Point(x, y));
            }
        }
    }
}

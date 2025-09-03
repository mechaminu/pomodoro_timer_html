using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using PomodoroTimer.Metrics;
using PomodoroTimer.Metrics.AES;

namespace PomodoroTimer
{
    public partial class MainWindow : Window
    {
        private readonly TimerService _timer = new TimerService();
        private readonly DispatcherTimer _uiTimer = new DispatcherTimer();
        private readonly KeyboardMonitor _keyboard = new KeyboardMonitor();
        private IActivityMetric _metric = new KeysPerMinuteMetric(TimeSpan.FromSeconds(10));

        private readonly List<double> _metricHistory = new();
        private int _lastSecond;
        private DateTime _sessionStart;
        private readonly FocusTracker _focusTracker = new FocusTracker();
        private readonly AfkDetector _afk = new AfkDetector();
        private bool _pausedByAfk;
        private readonly AppSettings _settings = new();

        public MainWindow()
        {
            InitializeComponent();
            TimerText.Text = _timer.GetState().Remaining.ToString(@"mm\:ss");
            MetricText.Text = "0";
            _uiTimer.Interval = TimeSpan.FromMilliseconds(250);
            _uiTimer.Tick += (_, _) =>
            {
                var state = _timer.GetState();
                TimerText.Text = state.Remaining.ToString(@"mm\:ss");
                UpdateMetric();
                UpdateRing();
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
                        MessageBox.Show($"Work session complete!\nAvg: {avg:F1}");
                    }
                    else
                    {
                        MessageBox.Show($"{mode} session complete!");
                    }
                    _metricHistory.Clear();
                    MetricPolyline.Points.Clear();
                    _keyboard.Reset();
                });
            };
            _afk.AfkStatusChanged += OnAfkChanged;
            _keyboard.KeyPressed += () => _metric.RecordEvent(DateTime.Now);

            _afk.Start();
            _focusTracker.Start();
            _lastSecond = DateTime.Now.Second;
            ApplySettings();
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            _timer.Start();
            _uiTimer.Start();
            _keyboard.Reset();
            _metric.Reset();
            _keyboard.Start();
            _metricHistory.Clear();
            MetricPolyline.Points.Clear();
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
            _metric.Reset();

            _metricHistory.Clear();
            MetricPolyline.Points.Clear();
            MetricText.Text = "0";
        }
        
        private void UpdateMetric()
        {
            _metric.Tick(DateTime.Now);
            var value = _metric.CurrentValue;
            MetricText.Text = _settings.Metric == "ActiveEngagementScore" ? $"AES: {value:F1}" : $"{value:F1} keys/min";
            var sec = DateTime.Now.Second;
            if (sec != _lastSecond)
            {
                _lastSecond = sec;
                _metricHistory.Add(value);
                var totalSeconds = _timer.GetState().Total.TotalSeconds;
                if (totalSeconds <= 0) totalSeconds = 1;
                var x = (_metricHistory.Count / totalSeconds) * MetricGraph.ActualWidth;
                var max = _settings.Metric == "ActiveEngagementScore" ? 100.0 : 200.0;
                var y = MetricGraph.Height - Math.Min(value, max) / max * MetricGraph.Height;
                MetricPolyline.Points.Add(new Point(x, y));
            }

            if (value < _settings.LowThreshold)
            {
                MetricText.Foreground = Brushes.Red;
                if (!_pausedByAfk)
                    Title = "Pomodoro Timer (Low Performance)";
            }
            else
            {
                MetricText.Foreground = Brushes.Black;
                if (!_pausedByAfk)
                    Title = "Pomodoro Timer";
            }
        }

        private void UpdateRing()
        {
            var state = _timer.GetState();
            var progress = state.Total.TotalSeconds > 0 ? (state.Total - state.Remaining).TotalSeconds / state.Total.TotalSeconds : 0;
            double angle = progress * 360;
            double radians = Math.PI / 180 * angle;
            double r = 90;
            double cx = 90;
            double cy = 90;
            double x = cx + r * Math.Sin(radians);
            double y = cy - r * Math.Cos(radians);
            bool largeArc = angle > 180;

            var figure = new PathFigure { StartPoint = new Point(cx, cy - r) };
            figure.Segments.Add(new ArcSegment(new Point(x, y), new Size(r, r), 0, largeArc, SweepDirection.Clockwise, true));
            var geometry = new PathGeometry();
            geometry.Figures.Add(figure);
            ProgressPath.Data = geometry;
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            var win = new SettingsWindow(_settings);
            if (win.ShowDialog() == true)
                ApplySettings();
        }

        private void ApplySettings()
        {
            _metric = _settings.Metric == "ActiveEngagementScore" ? new AesMetric() : new KeysPerMinuteMetric(TimeSpan.FromSeconds(10));
            _afk.SetThreshold(TimeSpan.FromSeconds(_settings.AfkThresholdSeconds));
        }

        private void OnAfkChanged(bool afk)
        {
            Dispatcher.Invoke(() =>
            {
                if (afk)
                {
                    if (_timer.GetState().Remaining > TimeSpan.Zero)
                    {
                        _timer.Pause();
                        _uiTimer.Stop();
                        _keyboard.Stop();
                        _pausedByAfk = true;
                    }
                    Title = "Pomodoro Timer (AFK)";
                }
                else
                {
                    if (_pausedByAfk)
                    {
                        _timer.Start();
                        _uiTimer.Start();
                        _keyboard.Start();
                        _pausedByAfk = false;
                    }
                    Title = "Pomodoro Timer";
                }
            });
        }
    }
}

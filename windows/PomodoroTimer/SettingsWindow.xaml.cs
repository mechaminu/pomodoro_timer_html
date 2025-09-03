using System;
using System.Windows;

namespace PomodoroTimer
{
    public partial class SettingsWindow : Window
    {
        private readonly AppSettings _settings;

        public SettingsWindow(AppSettings settings)
        {
            InitializeComponent();
            _settings = settings;
            MetricCombo.SelectedIndex = _settings.Metric == "ActiveEngagementScore" ? 1 : 0;
            ThresholdBox.Text = _settings.LowThreshold.ToString();
            AfkBox.Text = _settings.AfkThresholdSeconds.ToString();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            _settings.Metric = MetricCombo.SelectedIndex == 1 ? "ActiveEngagementScore" : "KeysPerMinute";
            if (double.TryParse(ThresholdBox.Text, out var t))
                _settings.LowThreshold = t;
            if (double.TryParse(AfkBox.Text, out var a))
                _settings.AfkThresholdSeconds = a;
            DialogResult = true;
        }
    }
}

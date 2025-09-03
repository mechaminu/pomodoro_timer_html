namespace PomodoroTimer
{
    public class AppSettings
    {
        public string Metric { get; set; } = "KeysPerMinute";
        public double LowThreshold { get; set; } = 30;
        public double AfkThresholdSeconds { get; set; } = 60;
    }
}

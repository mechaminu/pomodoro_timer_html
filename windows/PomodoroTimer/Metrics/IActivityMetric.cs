namespace PomodoroTimer.Metrics
{
    public interface IActivityMetric
    {
        void RecordEvent(System.DateTime timestamp);
        void Tick(System.DateTime timestamp);
        double CurrentValue { get; }
        void Reset();
    }
}

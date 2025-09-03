using System;
using PomodoroTimer.Metrics;

namespace PomodoroTimer.Tests;

public class KeysPerMinuteMetricTests
{
    [Fact]
    public void CalculatesKeysPerMinuteWithinWindow()
    {
        var metric = new KeysPerMinuteMetric(TimeSpan.FromSeconds(10));
        var now = DateTime.UtcNow;
        for (int i = 0; i < 5; i++)
        {
            metric.RecordEvent(now - TimeSpan.FromSeconds(5 - i));
        }

        var kpm = metric.CurrentValue;
        Assert.Equal(30, kpm, 5); // 5 keys in 10-second window -> 30 KPM
    }

    [Fact]
    public void PrunesEventsOutsideWindow()
    {
        var metric = new KeysPerMinuteMetric(TimeSpan.FromSeconds(10));
        var now = DateTime.UtcNow;
        metric.RecordEvent(now - TimeSpan.FromSeconds(11));
        metric.RecordEvent(now);

        var kpm = metric.CurrentValue;
        Assert.Equal(6, kpm, 5); // only latest key counted
    }

    [Fact]
    public void ResetClearsState()
    {
        var metric = new KeysPerMinuteMetric(TimeSpan.FromSeconds(10));
        metric.RecordEvent(DateTime.UtcNow);
        Assert.True(metric.CurrentValue > 0);
        metric.Reset();
        Assert.Equal(0, metric.CurrentValue);
    }
}

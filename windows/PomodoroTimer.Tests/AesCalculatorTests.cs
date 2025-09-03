using System;
using PomodoroTimer.Metrics.AES;

namespace PomodoroTimer.Tests;

public class AesCalculatorTests
{
    [Fact]
    public void ComputesAesAndDetectsFocusSpan()
    {
        var calc = new AesCalculator();
        var detector = new FocusSpanDetector();
        var start = DateTime.UtcNow;

        for (int i = 0; i < 30; i++)
        {
            var sample = new SignalSample
            {
                KCommit = 10,
                KEdit = 0,
                MMovePx = 0,
                MClick = 0,
                MScrollNotches = 0,
                ForegroundAppId = "vscode",
                MicroIdle = false,
                MacroIdle = false,
                NotificationsCount = 0
            };
            var ts = start.AddSeconds(i);
            var result = calc.Process(ts, sample);
            detector.AddSample(ts, result, sample);
        }

        // feed a period of inactivity to close the span
        for (int i = 30; i < 60; i++)
        {
            var sample = new SignalSample
            {
                KCommit = 0,
                KEdit = 0,
                MMovePx = 0,
                MClick = 0,
                MScrollNotches = 0,
                ForegroundAppId = "vscode",
                MicroIdle = true,
                MacroIdle = true,
                NotificationsCount = 0
            };
            var ts = start.AddSeconds(i);
            var result = calc.Process(ts, sample);
            detector.AddSample(ts, result, sample);
        }

        var span = Assert.Single(detector.Spans);
        Assert.True(span.DurationSeconds >= 12);
        Assert.True(span.AvgAes > 60);
    }
}

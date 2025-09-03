using System;
using System.Threading.Tasks;
using PomodoroTimer;

namespace PomodoroTimer.Tests;

public class TimerServiceTests
{
    [Fact]
    public async Task CompletesWorkSessionAndSwitchesToBreak()
    {
        var service = new TimerService();
        service.SetDurations(TimeSpan.FromMilliseconds(200), TimeSpan.FromMilliseconds(200));

        var tcs = new TaskCompletionSource<string>();
        service.OnComplete += mode => tcs.TrySetResult(mode);

        service.Start();
        var completed = await Task.WhenAny(tcs.Task, Task.Delay(2000));
        Assert.True(completed == tcs.Task, "Timer did not complete in time");

        var result = await tcs.Task;
        Assert.Equal("work", result);
        await Task.Delay(100); // allow mode switch
        var state = service.GetState();
        Assert.Equal("break", state.Mode);
        service.Pause();
    }
}

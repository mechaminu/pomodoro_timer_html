# Core Timer

This module replicates and extends the web timer functionality within native frameworks.

## Requirements
- Configurable Pomodoro and break durations stored locally.
- Timer states: running, paused, completed.
- Notifications when sessions end.

## Windows Implementation (WinUI 3)
- Use `DispatcherTimer` for timing.
- Persist settings with `ApplicationData.Current.LocalSettings`.
- Trigger toast notifications using Windows Notifications API.

## React Native Implementation
- Utilize `react-native-background-timer` for reliable timing.
- Store settings with `AsyncStorage`.
- Use local notifications via `react-native-push-notification`.

[Back to AGENTS](../AGENTS.md)

# Background Execution

[Back to overview](../AGENTS.md)

## Objectives
- Ensure timer runs when app minimized or unfocused.
- Provide user feedback via notifications or tray icon.

## Windows: WPF / WinUI 3
- Implement tray application using `NotifyIcon`.
- Use background tasks or services to keep timer running.
- Handle system suspend/resume events.

## Cross-platform: React Native
- Use `react-native-background-fetch` or `react-native-background-timer`.
- Configure headless JS tasks for Android; background fetch API for iOS.
- Display persistent notifications to indicate active timer.

## Tasks
1. Prototype background timer execution on each platform.
2. Validate battery and OS policies for long-running tasks.
3. Integrate notification controls for start/pause/stop.

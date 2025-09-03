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
=======
Refer to [AGENTS.md](../AGENTS.md) for the high‑level plan. This document details how the timer continues running when the app is minimized or unfocused.

## Objectives
- Support background services and notifications.
- Maintain timer progress without UI focus.

## Action Steps
1. **Windows:** Implement a tray application or Windows background task that keeps the timer active.
2. **React Native:** Integrate `react-native-background-fetch` or similar library to schedule periodic tasks.
3. Use local notifications to alert users when sessions end.
4. Persist session state to recover from process termination.

## Challenges
- Platform limitations on background execution frequency.
- Balancing power consumption with timely updates.
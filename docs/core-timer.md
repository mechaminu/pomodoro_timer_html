# Core Timer

[Back to overview](../AGENTS.md)

## Objectives
- Provide configurable Pomodoro and break durations.
- Persist settings locally on device.
- Maintain accurate timer state across app restarts.

## Windows: WPF / WinUI 3
- Use `DispatcherTimer` or `ThreadPoolTimer` for high-resolution timing.
- Store durations and state using `ApplicationData` or local JSON file.
- Integrate Windows toast notifications for session transitions.

## Cross-platform: React Native
- Implement timer with `setInterval` wrapped in native modules for precision.
- Store settings via `AsyncStorage`.
- Use `react-native-push-notification` for alerts on iOS/Android.

## Tasks
1. Define shared configuration schema.
2. Build timer service module encapsulating start/pause/reset logic.
3. Expose settings UI and persistence layer.
=======
Part of the [AGENTS plan](../AGENTS.md), this document outlines how to implement the Pomodoro and break timers natively.

## Objectives
- Store configurable Pomodoro and break durations locally.
- Provide start, pause, and reset controls.
- Persist timer state so sessions survive app restarts.

## Action Steps
1. Define timer models and persistence (local storage or SQLite).
2. Implement a foreground timer loop with notification hooks.
3. Expose timer state through a view‑model or context for UI components.
4. Add unit tests for timing accuracy and state transitions.

## Challenges
- Ensuring accuracy across sleep or background states.
- Handling user‑initiated changes mid‑session without losing progress.
# Background Execution

This document outlines strategies for keeping the timer active when the application is not in the foreground.

## Windows
- Use a Windows Background Task or tray application to maintain the timer.
- Employ `DispatcherTimer` or `ThreadPoolTimer` for low-power timing.
- Display toast notifications upon session completion.

## React Native
- Utilize `react-native-background-fetch` to wake the app periodically.
- Combine with local notifications to alert users.

## Next Steps
1. Prototype background task on each platform.
2. Verify battery and performance impact.
3. Expose settings to enable/disable background behavior.

[Back to AGENTS](../AGENTS.md)

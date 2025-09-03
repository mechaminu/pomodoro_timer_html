# Background Execution

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

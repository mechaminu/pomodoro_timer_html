# Framework Selection

This document compares target frameworks for porting the Pomodoro timer into native environments.

## Windows-First: WPF / WinUI 3 (.NET)
- Tight integration with Windows APIs and desktop capabilities.
- Supports background services and global input hooks.
- Small distribution size.

## Cross-Platform: React Native
- Reuses JavaScript/TypeScript and React knowledge.
- Targets iOS, Android, Web, and Desktop with shared code.
- Community packages such as `react-native-background-fetch` support background work.

## Decision
- Develop a Windows desktop app first with WinUI 3 to validate native features.
- Follow with a React Native implementation for broader platform coverage.

[Back to AGENTS](../AGENTS.md)

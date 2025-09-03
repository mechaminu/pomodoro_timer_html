# Windows Platform Plan

This document summarizes key considerations for a Windows-first native port using WPF or WinUI 3.

## Technology
- **Framework**: WinUI 3 with .NET 8 for modern UI and long-term support.
- **Packaging**: MSIX installer for easy distribution and automatic updates.
- **Interop**: Access Win32 APIs for system tray integration and global input hooks.

## Background Execution
- Implement a background service to keep timers running when the window is minimized.
- Provide system tray controls for quick start/stop and status display.

## Data Storage
- Store settings and session history in the user's AppData folder using `System.Text.Json`.

## Next Steps
1. Scaffold WinUI 3 project and establish MVVM structure.
2. Port timer logic into a shared .NET class library.
3. Implement tray icon and toast notifications.

[Back to AGENTS](../AGENTS.md)

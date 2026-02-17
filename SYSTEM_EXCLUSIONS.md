# System Application Exclusions

This document describes the default system applications that are excluded from the window list.

## Overview

By default, WindowTabsFree excludes common system applications and UI components to keep the window list clean and focused on user applications. These exclusions are configured in the `ExcludedApplications` list in the settings.

## Default Excluded Applications

### Windows System Processes

| Process Name | Description |
|--------------|-------------|
| `explorer` | Windows Explorer/File Explorer |
| `taskmgr` | Windows Task Manager |
| `dwm` | Desktop Window Manager |
| `SearchUI` | Windows Search UI |
| `SearchApp` | Windows Search Application |
| `ShellExperienceHost` | Windows Shell Experience Host |
| `ApplicationFrameHost` | Universal Windows Platform (UWP) frame host |
| `TextInputHost` | Windows Text Input Host |
| `LockApp` | Windows Lock Screen |
| `StartMenuExperienceHost` | Windows Start Menu |
| `SystemSettings` | Windows Settings App |

### macOS System Processes

| Process Name | Description |
|--------------|-------------|
| `Dock` | macOS Dock |
| `Control Center` / `ControlCenter` | macOS Control Center |
| `NotificationCenter` / `Notification Center` | macOS Notification Center |
| `SystemUIServer` | macOS System UI Server |
| `WindowServer` | macOS Window Server |
| `loginwindow` | macOS Login Window |
| `CoreServicesUIAgent` | macOS Core Services UI Agent |
| `UserEventAgent` | macOS User Event Agent |
| `Problem Reporter` / `ProblemReporter` | macOS Problem Reporter |
| `Spotlight` | macOS Spotlight Search |

### Linux System Processes

| Process Name | Description |
|--------------|-------------|
| `gnome-shell` | GNOME Desktop Shell |
| `plasmashell` | KDE Plasma Desktop Shell |
| `xfce4-panel` | XFCE Desktop Panel |
| `lxpanel` | LXDE Desktop Panel |
| `mate-panel` | MATE Desktop Panel |
| `cinnamon` | Cinnamon Desktop Environment |

## Customizing Exclusions

You can customize the list of excluded applications by editing the `settings.json` file located in:

- **Windows**: `%APPDATA%\WindowTabsFree\settings.json`
- **macOS**: `~/Library/Application Support/WindowTabsFree/settings.json`
- **Linux**: `~/.config/WindowTabsFree/settings.json`

### Example Configuration

```json
{
  "ExcludedApplications": [
    "explorer",
    "taskmgr",
    "Dock",
    "Control Center",
    "gnome-shell",
    "MyCustomApp"
  ]
}
```

### Adding Custom Exclusions

To exclude additional applications:

1. Find the process name of the application you want to exclude
   - **Windows**: Use Task Manager (Details tab)
   - **macOS**: Use Activity Monitor
   - **Linux**: Use `ps aux` or System Monitor
2. Add the process name (without .exe extension) to the `ExcludedApplications` array
3. Save the file
4. Restart WindowTabsFree for changes to take effect

### Removing Exclusions

To make a system application visible in the window list:

1. Open `settings.json`
2. Remove the process name from the `ExcludedApplications` array
3. Save the file
4. Restart WindowTabsFree

## Why Exclude System Applications?

System applications are typically excluded because:

1. **Cluttered UI**: System processes don't need window management
2. **Stability**: Manipulating system windows can cause unexpected behavior
3. **User Focus**: Most users want to manage only their own application windows
4. **Performance**: Fewer windows to enumerate and display

## Notes

- Process name matching is **case-insensitive**
- Exclusions apply to all window enumeration operations
- Both exact matches and spaces/no-spaces variants are included for compatibility
- The exclusion list is loaded when the application starts

## Troubleshooting

### System Application Still Appears

If a system application still appears in the window list:

1. Verify the exact process name using your system's process monitor
2. Check for case sensitivity issues (though matching is case-insensitive)
3. Ensure the process name matches exactly (some processes may have different names on different OS versions)
4. Restart WindowTabsFree after making changes

### User Application is Excluded

If a user application is being excluded:

1. Check if its process name matches any in the exclusion list
2. Remove it from the `ExcludedApplications` array if needed
3. Be careful with common names (e.g., "explorer" could be a file manager or web browser)

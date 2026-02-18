# Global Hotkey Support

This document describes the global hotkey feature that allows switching between windows even when the application is not focused.

## Overview

The application now supports global hotkeys that work system-wide, allowing you to switch between windows without having to first focus on the WindowTabsFree application.

## Default Hotkeys

The following default hotkeys are configured:

- **Next Window**: `Ctrl+Alt+Right` - Cycles to the next manageable window
- **Previous Window**: `Ctrl+Alt+Left` - Cycles to the previous manageable window

## Configuring Hotkeys

### Via Configuration File

Edit the `settings.json` file (typically located in the application's configuration directory):

```json
{
  "HotKeys": {
    "NextWindow": "Ctrl+Alt+Right",
    "PreviousWindow": "Ctrl+Alt+Left"
  }
}
```

### Via UI (if available)

1. Open the application
2. Look for the "Configure Hotkeys" option in the Floating Window Manager
3. Click on the text box for the hotkey you want to configure
4. Press the desired key combination
5. Click "Save"

## Supported Key Combinations

The hotkey parser supports the following modifiers:
- `Ctrl` - Control key
- `Alt` - Alt key
- `Shift` - Shift key
- `Win` - Windows/Super key

And the following keys:
- Letters: A-Z
- Numbers: 0-9
- Function keys: F1-F12
- Arrow keys: Left, Right, Up, Down
- Special keys: Space, Enter, Esc, Tab, Backspace, Delete, Insert, Home, End, PageUp, PageDown
- NumPad keys: NumPad0-NumPad9, NumPad+, NumPad-, NumPad*, NumPad/

### Examples

- `Ctrl+Alt+A`
- `Ctrl+Shift+T`
- `Win+R`
- `Alt+F4`
- `Ctrl+F1`

## How It Works

1. **Windows**: Uses the Windows API `RegisterHotKey` function to register global hotkeys
2. **macOS**: Not yet implemented (placeholder exists)
3. **Linux**: Not yet implemented (placeholder exists)

### Technical Details

The hotkey service:
- Runs in a background thread with a message-only window (Windows)
- Registers hotkeys globally at the OS level
- Invokes callbacks when hotkeys are pressed
- Automatically unregisters hotkeys when the application closes

### Window Cycling

When you press a hotkey:
1. The application gets the list of all manageable windows (excluding system windows and configured exclusions)
2. It identifies the currently focused window
3. It switches to the next/previous window in the list
4. The list wraps around (after the last window, it goes to the first)

## Troubleshooting

### Hotkeys Don't Work

1. **Check if hotkey is already registered**: Another application might be using the same hotkey combination. Try a different combination.
2. **Windows only**: The full implementation currently only works on Windows. macOS and Linux support will be added in future updates.
3. **Check console output**: The application logs when hotkeys are registered. Check for error messages.
4. **Restart the application**: Changes to hotkey configuration require an application restart to take effect.

### Conflicts with Other Applications

If your hotkey doesn't work, it might be conflicting with:
- System hotkeys (e.g., Win+L, Ctrl+Alt+Delete)
- Other applications that have registered the same hotkey
- Try using a more unique combination with multiple modifiers

## Platform Support

| Platform | Status | Notes |
|----------|--------|-------|
| Windows | ✅ Fully Supported | Uses RegisterHotKey API |
| macOS | ⚠️ Placeholder | Requires Carbon/Cocoa EventTap implementation |
| Linux | ⚠️ Placeholder | Requires X11 XGrabKey or DBus implementation |

## Future Enhancements

- Hot-reload of hotkey configuration without restart
- More granular window selection (by application, by group, etc.)
- Hotkey conflict detection and warnings
- Visual feedback when hotkey is pressed
- Full macOS and Linux support

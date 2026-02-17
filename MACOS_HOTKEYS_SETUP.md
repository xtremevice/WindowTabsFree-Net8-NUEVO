# macOS Hotkeys Setup Guide

## Overview

WindowTabsFree uses global hotkeys to switch between windows. On macOS, global hotkeys require **Accessibility permissions** to function properly.

## Common Error

If you see this error in the console:
```
[macOS] Attempting to register hotkey: Ctrl+Z
[macOS] RegisterEventHotKey failed with code: -50
[macOS] Note: Global hotkeys may require Accessibility permissions on modern macOS
```

This means the application needs Accessibility permissions.

## How to Enable Accessibility Permissions

### Step 1: Open System Settings

1. Click the Apple menu () in the top-left corner
2. Select **System Settings** (or **System Preferences** on older macOS versions)

### Step 2: Navigate to Privacy & Security

1. In System Settings, click **Privacy & Security** in the sidebar
2. Scroll down and click **Accessibility**

### Step 3: Grant Permission to WindowTabsFree

1. Click the lock icon 🔒 at the bottom to make changes (you may need to enter your password)
2. Click the **+** button to add an application
3. Navigate to where WindowTabsFree is installed (usually `/Applications` or your user folder)
4. Select **WindowTabsFree** and click **Open**
5. Ensure the checkbox next to WindowTabsFree is **enabled** ✓

### Step 4: Restart WindowTabsFree

1. Quit WindowTabsFree completely
2. Restart the application
3. The hotkeys should now work!

## Alternative: Using Command Line

If you're comfortable with the terminal, you can check if WindowTabsFree has accessibility permissions:

```bash
# Check if app has accessibility permissions
sqlite3 /Library/Application\ Support/com.apple.TCC/TCC.db \
  "SELECT * FROM access WHERE service='kTCCServiceAccessibility';"
```

## Error Code Reference

| Error Code | Meaning | Solution |
|------------|---------|----------|
| -50 | Parameter error / Missing permissions | Enable Accessibility permissions |
| -9999 | Hotkey already registered | Try a different key combination |

## Troubleshooting

### Hotkeys Still Not Working?

1. **Check key combination**: Ensure your hotkey doesn't conflict with system shortcuts
2. **Try different keys**: Use combinations like:
   - `Ctrl+Alt+Right` (Next window)
   - `Ctrl+Alt+Left` (Previous window)
   - `Cmd+Shift+Right` (macOS style)
3. **Restart your Mac**: Sometimes permissions need a full restart to take effect
4. **Remove and re-add**: In Accessibility settings, remove WindowTabsFree and add it again

### macOS Ventura (13.0+) and Later

On newer macOS versions, you might need to:
1. Grant **Full Disk Access** in addition to Accessibility
2. Allow the app in **Security & Privacy** → **Privacy** → **Input Monitoring**

## Default Hotkeys

The default hotkey configuration:
- **Next Window**: `Ctrl+Alt+Right`
- **Previous Window**: `Ctrl+Alt+Left`

You can change these in the application settings.

## System Limitations

- Some system applications (like System Settings, Finder) may not respond to window switching
- Applications from `/System/Library/CoreServices/` are automatically excluded from management
- Background processes and menu bar apps typically don't have manageable windows

## Need Help?

If you continue experiencing issues:
1. Check the application console for detailed error messages
2. Report the issue on GitHub with the error code and macOS version
3. Include screenshots of your Privacy & Security settings

## Related Documentation

- [Main README](README.md)
- [macOS System Apps Exclusions](SYSTEM_EXCLUSIONS.md)
- [Hotkeys General Guide](HOTKEYS.md)

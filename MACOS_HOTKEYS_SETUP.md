# macOS Hotkeys Setup Guide

## Overview

WindowTabsFree uses global hotkeys to switch between windows. On macOS, global hotkeys require **Accessibility permissions** to function properly.

## Permission Check on Launch

When you launch WindowTabsFree on macOS, it will automatically check if it has Accessibility permissions:

- **If permissions are granted:** ✓ Hotkeys will work immediately
- **If permissions are NOT granted:** ✗ You'll see instructions in the console on how to enable them

**Important:** Unlike some apps, WindowTabsFree cannot automatically show the system permission dialog. You must manually enable permissions in System Settings.

## How to Enable Accessibility Permissions

If you see this message in the console:
```
[macOS] ✗ Accessibility permissions NOT granted
[macOS] HOTKEYS WILL NOT WORK without Accessibility permissions!
```

Follow these steps:

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

### ⚠️ Error -50 When Running from Terminal

**Symptom:** You see the error:
```
[macOS] RegisterEventHotKey failed with code: -50
[macOS] Error -50 typically means missing Accessibility permissions
```

And in System Settings → Accessibility you see "Terminal" or "iTerm2" instead of WindowTabsFree.

**Explanation:**
When you run the application using `dotnet run` or `dotnet exec` from Terminal, macOS sees the parent process as Terminal or the `dotnet` executable. Even if you've granted permissions to Terminal, the actual application needs its own permissions.

**Solution 1 (Recommended): Run as Native App**

1. Build the application as a macOS bundle:
```bash
cd /path/to/WindowTabsFree-Net8-NUEVO
dotnet publish -c Release -r osx-arm64 --self-contained src/WindowTabsFree.UI/WindowTabsFree.UI.csproj
```

2. The compiled app will be in:
```
src/WindowTabsFree.UI/bin/Release/net8.0/osx-arm64/publish/
```

3. Run the app from Finder (not Terminal):
   - Navigate to the folder in Finder
   - Double-click `WindowTabsFree.UI`
   - ⚠️ If macOS says it can't open it for security:
     - Go to System Settings → Privacy & Security
     - You'll see a message about WindowTabsFree.UI being blocked
     - Click "Open Anyway"

4. Now WindowTabsFree.UI will appear in Accessibility (not Terminal)
5. Enable Accessibility permissions for WindowTabsFree.UI
6. Hotkeys should work!

**Solution 2 (If you must run from Terminal):**

If you need to run from Terminal for development or other reasons:

1. Open **System Settings** → **Privacy & Security** → **Accessibility**
2. Click the lock icon 🔒 and enter your password
3. Click the **+** button to add an application
4. Navigate to where `dotnet` is installed:
   - Common location: `/usr/local/share/dotnet/dotnet`
   - Or find the location with: `which dotnet` in Terminal
5. Add `dotnet` to the list
6. Make sure the checkbox next to `dotnet` is **enabled** ✓
7. **Also** add Terminal/iTerm2 if it's not already there
8. Restart the application from Terminal

**Why does this happen?**

- Terminal runs → `dotnet` runs → WindowTabsFree.UI
- macOS checks permissions at each level of the chain
- If `dotnet` doesn't have permissions, WindowTabsFree.UI can't use them either
- That's why giving permissions to Terminal alone isn't enough

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

# Linux Hotkey Setup Guide

## Overview

WindowTabsFree on Linux uses **evdev** (event device interface) to capture global keyboard hotkeys. This requires special permissions to read from `/dev/input` devices.

## Why evdev?

- **Works on X11 and Wayland**: Unlike X11-specific solutions, evdev works on both display servers
- **Low-level**: Reads keyboard events directly from the kernel
- **Reliable**: No dependency on desktop environment or window manager

## Permission Requirements

To use global hotkeys on Linux, you need read access to `/dev/input/event*` devices.

### Option 1: Add User to 'input' Group (Recommended)

This is the recommended approach as it doesn't require root privileges to run the application.

1. **Add your user to the input group:**
   ```bash
   sudo usermod -a -G input $USER
   ```

2. **Log out and log back in** (or reboot)
   - This is required for the group membership to take effect
   - Simply opening a new terminal won't work

3. **Verify group membership:**
   ```bash
   groups
   ```
   You should see `input` in the list

4. **Run WindowTabsFree:**
   ```bash
   dotnet run
   ```
   Or run the published executable normally

### Option 2: Run with sudo (Not Recommended)

Running the entire application with sudo is a security risk and not recommended:

```bash
sudo dotnet run
```

⚠️ **Security Warning**: Running GUI applications with sudo can be dangerous and is generally discouraged.

### Option 3: Create udev Rule (Advanced)

For more fine-grained control, you can create a udev rule:

1. **Create udev rule file:**
   ```bash
   sudo nano /etc/udev/rules.d/99-input.rules
   ```

2. **Add this line:**
   ```
   KERNEL=="event*", SUBSYSTEM=="input", MODE="0660", GROUP="input"
   ```

3. **Reload udev rules:**
   ```bash
   sudo udevadm control --reload-rules
   sudo udevadm trigger
   ```

4. **Add your user to input group** (as in Option 1)

## Troubleshooting

### Hotkeys Not Working

**1. Check if you're in the input group:**
```bash
groups | grep input
```

**2. Check /dev/input permissions:**
```bash
ls -l /dev/input/event*
```

You should see something like:
```
crw-rw---- 1 root input 13, 64 Feb 17 10:00 /dev/input/event0
```

The `input` group should have read permissions (rw-).

**3. Check console output:**

The application logs helpful messages:
```
[Linux/evdev] Starting keyboard event monitoring
[Linux/evdev] Found 2 keyboard device(s)
[Linux/evdev] Monitoring: /dev/input/event2
```

**If you see permission errors:**
```
[Linux/evdev] ⚠️  No keyboard devices found or no permission to access them
[Linux/evdev] Make sure you're in the 'input' group (logout/login required after adding)
```

This means you need to add yourself to the input group and log out/in.

### Finding Keyboard Devices

The service automatically searches for keyboard devices in:
1. `/dev/input/by-id/` - Devices with "kbd" or "keyboard" in the name
2. `/dev/input/event*` - All event devices

You can manually check which devices are keyboards:
```bash
cat /proc/bus/input/devices | grep -A 5 keyboard
```

### Testing Hotkey Registration

When you configure hotkeys in the application, you should see:
```
[Linux/evdev] Attempting to register hotkey: Ctrl+Alt+Right
[Linux/evdev] Successfully registered hotkey: Ctrl+Alt+Right
```

When you press the hotkey:
```
[Linux/evdev] Hotkey triggered: Ctrl+Alt+Right
```

## Supported Keys

### Modifiers
- **Ctrl** / **Control**
- **Alt**
- **Shift**
- **Win** / **Super** / **Meta** (Windows/Super key)

### Letter Keys
- A-Z (case insensitive)

### Number Keys
- 0-9 (top row)

### Function Keys
- F1-F12

### Arrow Keys
- Left, Right, Up, Down

### Special Keys
- Space
- Enter
- Esc / Escape
- Tab
- Backspace

### Examples

Valid hotkey combinations:
- `Ctrl+Alt+Right` - Ctrl + Alt + Right Arrow
- `Ctrl+Shift+A` - Ctrl + Shift + A
- `Win+F1` - Windows/Super + F1
- `Alt+Tab` - Alt + Tab

## How It Works

1. **Device Discovery**: The service finds keyboard devices in `/dev/input`
2. **Event Reading**: Opens device files and reads raw input events
3. **Event Parsing**: Parses evdev `input_event` structures (24 bytes each)
4. **Key Tracking**: Tracks which keys are currently pressed
5. **Hotkey Matching**: When a key is released, checks if the combination matches any registered hotkey
6. **Callback Execution**: Executes the callback for matching hotkeys

## Security Considerations

- The input group grants read access to **all** input devices (keyboard, mouse, touchpad)
- This means the application can read all keyboard input system-wide
- Only add trusted users to the input group
- The application only reads events when hotkeys are registered
- No keylogging or logging of non-hotkey keypresses occurs

## Platform Notes

### Works On
- ✅ Ubuntu / Debian
- ✅ Fedora / RHEL / CentOS
- ✅ Arch Linux
- ✅ Most modern Linux distributions
- ✅ X11 and Wayland

### Requirements
- Linux kernel with evdev support (virtually all modern kernels)
- Read access to `/dev/input` devices
- .NET 8.0 runtime

## Alternative Solutions

If you cannot or do not want to grant input device access:

1. **Use UI Buttons**: The application provides UI buttons for all hotkey actions
2. **Desktop Environment Shortcuts**: Configure your DE to run commands:
   ```bash
   # Example for GNOME/KDE custom shortcuts
   dotnet run --project WindowTabsFree.UI -- --next-window
   ```
3. **X11 XGrabKey**: We could implement X11-specific hotkeys (X11 only, not Wayland)

## Future Improvements

Potential enhancements:
- Support for more key codes
- Configurable device filtering
- Virtual keyboard detection/exclusion
- Key repeat handling
- Multi-key chord support

## Getting Help

If you're still having issues:

1. Check console output for error messages
2. Verify group membership: `groups | grep input`
3. Check device permissions: `ls -l /dev/input/event*`
4. Try running with sudo once to test (just to verify it works with permissions)
5. Report issues with console output included

## References

- [Linux Input Subsystem](https://www.kernel.org/doc/html/latest/input/input.html)
- [evdev API](https://www.kernel.org/doc/html/latest/input/event-codes.html)
- [Input Device Permissions](https://wiki.archlinux.org/title/Users_and_groups#Pre-systemd_groups)

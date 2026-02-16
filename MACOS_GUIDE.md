# WindowTabsFree - macOS Support Guide

## 🍎 macOS Compatibility

WindowTabsFree now supports macOS! This guide explains how to use the application on macOS and the specific requirements and limitations.

**📱 Mac Apple Silicon Users (M1/M2/M3):**  
See the complete step-by-step guide: [**🍎 Guía Mac Apple Silicon (Español)**](GUIA_MAC_APPLE_SILICON.md)

---

## 📋 Requirements

### System Requirements
- **macOS Version**: 10.15 (Catalina) or later
- **.NET 8 Runtime**: Install from [dot.net](https://dotnet.microsoft.com/download)
- **Accessibility Permissions**: Required for window management

### Permissions

macOS requires explicit permission to control other applications. You'll need to grant accessibility permissions:

1. Open **System Preferences** → **Security & Privacy** → **Privacy** tab
2. Select **Accessibility** from the left sidebar
3. Click the lock icon and enter your password
4. Add **WindowTabsFree** (or **Terminal** if running from command line)
5. Check the box to enable permission

**Important**: Without accessibility permissions, the app can enumerate windows but cannot control them (focus, minimize, maximize, close).

---

## 🚀 Installation & Running

### Quick Start

```bash
# Clone the repository
git clone https://github.com/xtremevice/WindowTabsFree.git
cd WindowTabsFree

# Checkout the feature branch
git checkout copilot/implement-window-service-functionality

# Run the application
dotnet run --project src/WindowTabsFree.UI/WindowTabsFree.UI.csproj
```

### Build Standalone Executable

```bash
# Build self-contained macOS app
dotnet publish src/WindowTabsFree.UI/WindowTabsFree.UI.csproj \
  -c Release \
  -r osx-x64 \
  --self-contained true \
  -o publish/macos-x64

# Run the standalone app
./publish/macos-x64/WindowTabsFree.UI
```

For Apple Silicon (M1/M2/M3):
```bash
# Build for ARM64
dotnet publish src/WindowTabsFree.UI/WindowTabsFree.UI.csproj \
  -c Release \
  -r osx-arm64 \
  --self-contained true \
  -o publish/macos-arm64

# Run the standalone app
./publish/macos-arm64/WindowTabsFree.UI
```

---

## 🎯 Features on macOS

### Supported Features

✅ **Window Enumeration**
- Lists all visible windows from all applications
- Shows application name, window title, and window bounds
- Auto-refresh every 2 seconds

✅ **Window Information**
- Process name (application name)
- Window title
- Window position and size
- Process ID

✅ **Window Control**
- **Focus**: Brings window to front
- **Minimize**: Minimizes window to Dock
- **Maximize**: Enters full-screen mode
- **Restore**: Restores from minimized or full-screen
- **Close**: Closes the window

✅ **Tab Groups**
- Manual grouping
- Auto-grouping by application
- Tab navigation (Next/Previous)

### macOS-Specific Behavior

**Full-Screen vs Maximize**:
- macOS doesn't have a "maximize" button like Windows
- The "Maximize" action enters full-screen mode (`AXFullScreen`)
- This is the macOS equivalent of maximizing

**Window Closing**:
- Clicking the close button (button 1) on macOS
- Some apps may show confirmation dialogs
- Unsaved documents may prompt to save

**Spaces (Virtual Desktops)**:
- Windows on different Spaces are still enumerated
- Focusing a window on another Space will switch to that Space
- This is standard macOS behavior

---

## 🔧 Technical Details

### APIs Used

**Core Graphics**:
- `CGWindowListCopyWindowInfo()` - Enumerates all windows
- Provides window IDs, titles, bounds, visibility, process info

**AppleScript**:
- Used for window manipulation (focus, minimize, maximize, restore, close)
- More reliable than direct API calls for some operations
- Requires accessibility permissions

**System Events**:
- AppleScript's "System Events" provides window control
- Uses Accessibility attributes (AXMinimized, AXFullScreen, etc.)

### Implementation Notes

- **Window IDs**: macOS uses `CGWindowID` (integer) mapped to `IntPtr`
- **Z-Order**: Windows are returned in front-to-back order by default
- **AppleScript Timeout**: 5 seconds for window operations
- **Best-Effort Operations**: Window control operations fail silently if they can't complete

---

## 🐛 Troubleshooting

### Issue: "No windows are showing"

**Solution**:
1. Grant accessibility permissions (see Requirements section above)
2. Restart the application after granting permissions
3. Ensure windows are on the active Space

### Issue: "Window operations don't work"

**Cause**: Missing accessibility permissions

**Solution**:
1. Open System Preferences → Security & Privacy → Privacy → Accessibility
2. Add WindowTabsFree to the list
3. Enable the checkbox
4. Restart the app

### Issue: "Some windows are missing"

**Possible causes**:
- Windows on other Spaces (virtual desktops)
- Windows without titles (intentionally filtered out)
- Background processes without visible windows
- System windows (Dock, Menu Bar) are intentionally excluded

**To see all windows** including those on other Spaces:
- The app already shows windows from all Spaces
- Click on a window to switch to its Space

### Issue: "Application crashes on startup"

**Possible causes**:
- .NET 8 not installed
- Running on macOS older than 10.15

**Solution**:
1. Install .NET 8 Runtime: `brew install dotnet-sdk`
2. Update macOS to 10.15 or later

### Issue: "Permission dialogs keep appearing"

This is normal macOS behavior when:
- Running from Terminal (Terminal needs permission)
- First time running the app
- After updating the app

**Solution**: Grant permission once, it will be remembered.

---

## 📊 Comparison with Other Platforms

| Feature | Windows | Linux | macOS |
|---------|---------|-------|-------|
| Window Enumeration | ✅ Win32 API | ✅ X11/Xlib | ✅ Core Graphics |
| Window Control | ✅ Native | ✅ X11 | ✅ AppleScript |
| Minimize | ✅ | ✅ | ✅ |
| Maximize | ✅ | ✅ | ✅ (Full-Screen) |
| Restore | ✅ | ✅ | ✅ |
| Close | ✅ | ✅ | ✅ |
| Focus | ✅ | ✅ | ✅ |
| Auto-Grouping | ✅ | ✅ | ✅ |
| Tab Navigation | ✅ | ✅ | ✅ |
| Permissions Required | ❌ | ❌ | ✅ (Accessibility) |

---

## 💡 Tips for macOS Users

1. **Grant Permissions Early**: Do this before testing window operations

2. **Use Keyboard Shortcuts**: macOS users are used to keyboard navigation
   - The app supports Next/Prev tab buttons
   - Global hotkeys coming in future update

3. **Spaces Awareness**: If a window seems to "disappear" after focusing, check other Spaces

4. **Mission Control**: You can still use Mission Control (F3) to see all windows across Spaces

5. **Dock Integration**: Minimized windows go to the Dock (right side if app is in Dock)

---

## 🔮 Future Enhancements

Planned improvements for macOS:

- [ ] Native macOS app bundle (.app format)
- [ ] Menu bar integration
- [ ] Global keyboard shortcuts
- [ ] Notification Center integration
- [ ] Spotlight integration for quick window access
- [ ] Support for Stage Manager (macOS 13+)
- [ ] Better full-screen handling

---

## 📚 Related Documentation

- [Main README](README_NET8.md) - General installation and features
- [Quick Start Guide](QUICKSTART.md) - How to run quickly
- [Grouping Guide (Spanish)](GUIA_AGRUPACION.md) - Auto-grouping and tab navigation

---

## 🆘 Getting Help

**Permission Issues**: See the Troubleshooting section above

**Other Issues**: 
- Check existing [GitHub Issues](https://github.com/xtremevice/WindowTabsFree/issues)
- Create a new issue with:
  - macOS version
  - .NET version (`dotnet --version`)
  - Error message or unexpected behavior
  - Steps to reproduce

---

## 🎉 Enjoy WindowTabsFree on macOS!

macOS support is now fully functional. The application uses native macOS APIs (Core Graphics, AppleScript) to provide seamless window management.

**First time on macOS?** Remember to grant accessibility permissions!

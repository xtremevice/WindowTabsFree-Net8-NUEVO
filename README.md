# WindowTabsFree - .NET 8 Edition

**Cross-platform window manager with tab grouping functionality for Windows, Linux, and macOS.**

## 🚀 Quick Start

### Prerequisites
- .NET 8 SDK or Runtime
- **Windows**: Windows 7+
- **Linux**: X11 display server + libX11.so.6
- **macOS**: macOS 10.15+ (Catalina or later)

### Run Immediately
```bash
dotnet run --project src/WindowTabsFree.UI/WindowTabsFree.UI.csproj
```

### Build Standalone Executable

**Linux:**
```bash
./publish-linux.sh
./publish/linux-x64/WindowTabsFree.UI
```

**macOS (Apple Silicon):**
```bash
./publish-macos-arm64.sh
./publish/macos-arm64/WindowTabsFree.UI
```

**Windows:**
```bash
dotnet publish src/WindowTabsFree.UI/WindowTabsFree.UI.csproj -c Release -r win-x64 --self-contained -o publish/win-x64
```

## ✨ Features

### Window Management
- **Enumerate all open windows** from the operating system
- **Control windows**: Focus, Minimize, Maximize, Restore, Close
- **Real-time updates** every 2 seconds
- **Cross-platform**: Works on Windows, Linux (X11), and macOS

### Tab Grouping
- **Auto-grouping by application**: Check the "Auto-group" checkbox per application
- **Manual groups**: Create custom groups for project organization
- **Tab navigation**: Previous/Next tab with visual indicators
- **Active tab highlighting**: Green for active, white/blue for inactive
- **Persistent configuration**: Groups saved in JSON

### Floating Window Manager
- **Compact floating window** (450x600) with all tab information
- **Always-on-top option** within floating window
- **Auto-minimize main window** when floating opens
- **Full tab visualization**: See all windows in each group
- **Focus tabs directly** from floating window

### UI Features
- **Detailed window information**: App name, title, window count, state
- **Per-application auto-grouping**: Individual control over which apps to group
- **Visual feedback**: Color-coded tabs, status indicators
- **Responsive design**: Works on various screen sizes

## 📖 Documentation

### Getting Started
- **[README_NET8.md](README_NET8.md)** - Full feature documentation
- **[QUICKSTART.md](QUICKSTART.md)** - Quick start guide (English)
- **[QUICKSTART_ES.md](QUICKSTART_ES.md)** - Guía rápida (Español)

### Feature Guides
- **[GUIA_AGRUPACION.md](GUIA_AGRUPACION.md)** - Auto-grouping and tab navigation (Spanish)
- **[GUIA_USO_MEJORADA.md](GUIA_USO_MEJORADA.md)** - Enhanced features guide (Spanish)
- **[AUTO_AGRUPACION_Y_TABS.md](AUTO_AGRUPACION_Y_TABS.md)** - Tab system documentation

### Platform-Specific
- **[MACOS_GUIDE.md](MACOS_GUIDE.md)** - macOS setup and usage
- **[GUIA_MAC_APPLE_SILICON.md](GUIA_MAC_APPLE_SILICON.md)** - Mac M1/M2/M3 guide (Spanish)
- **[MACOS_DETECCION_FIX.md](MACOS_DETECCION_FIX.md)** - macOS window detection fixes

### Technical
- **[LIMITACION_TABS_OVERLAY.md](LIMITACION_TABS_OVERLAY.md)** - Tab overlay limitations explained
- **[RESUMEN_CAMBIOS_AGRUPACION.md](RESUMEN_CAMBIOS_AGRUPACION.md)** - Grouping changes summary
- **[RESUMEN_MEJORAS.md](RESUMEN_MEJORAS.md)** - Implementation improvements

## 🏗️ Project Structure

```
WindowTabsFree-Net8/
├── src/
│   ├── WindowTabsFree.Common/      # Shared models and interfaces
│   │   ├── Models/
│   │   │   ├── WindowInfo.cs       # Window information model
│   │   │   ├── TabGroup.cs         # Tab group model
│   │   │   ├── AppSettings.cs      # Application settings
│   │   │   └── ApplicationGroupSetting.cs
│   │   └── Interfaces/
│   │       └── IWindowService.cs   # Window service interface
│   ├── WindowTabsFree.Core/        # Business logic
│   │   └── Services/
│   │       ├── WindowManagerService.cs
│   │       └── ConfigurationService.cs
│   ├── WindowTabsFree.Services/    # Platform-specific implementations
│   │   ├── Windows/
│   │   │   └── WindowsWindowService.cs  # Win32 P/Invoke
│   │   ├── Linux/
│   │   │   └── LinuxWindowService.cs    # X11 P/Invoke
│   │   ├── macOS/
│   │   │   └── MacOSWindowService.cs    # Core Graphics/AppleScript
│   │   └── WindowServiceFactory.cs
│   ├── WindowTabsFree.UI/          # Avalonia UI
│   │   ├── Views/
│   │   │   ├── MainWindow.axaml
│   │   │   └── FloatingWindowManager.axaml
│   │   ├── ViewModels/
│   │   │   └── MainWindowViewModel.cs
│   │   ├── App.axaml
│   │   └── Program.cs
│   └── WindowTabsFree.TestConsole/ # Testing utility
├── WindowTabsFreeNet.sln           # Solution file
├── publish-linux.sh                # Linux build script
├── publish-macos-arm64.sh          # macOS build script
└── check-macos-requirements.sh     # macOS prerequisites check
```

## 🎯 Usage

### 1. Group Windows by Application
1. Open multiple windows of the same application (e.g., 3 Chrome windows)
2. Check the **"Auto-group"** checkbox next to the application
3. A group is created automatically with all windows as tabs

### 2. Navigate Between Tabs
- Click **"◄ Prev Tab"** or **"Next Tab ►"** buttons
- Visual indicator shows active tab (green background)
- Tab counter shows position (e.g., "Tab 1 of 3")

### 3. Use Floating Window Manager
1. Click **"🪟 Floating Window"** button
2. Main window minimizes to dock
3. Floating window shows all groups with full tab information
4. Enable **"📌 Pin On Top"** to keep it always visible
5. Click **"Focus This Tab"** on any tab to switch to that window

### 4. Manual Groups
1. Click **"📁 New Manual Group"** 
2. Enter group name
3. Select windows and add them to the group

## ⚙️ Configuration

Settings are stored in JSON:
- **Linux**: `~/.config/WindowTabsFree/settings.json`
- **macOS**: `~/Library/Application Support/WindowTabsFree/settings.json`
- **Windows**: `%APPDATA%\WindowTabsFree\settings.json`

Example configuration:
```json
{
  "TabGroups": [
    {
      "Id": "group-123",
      "Name": "Chrome Windows",
      "WindowHandles": [12345, 12346, 12347],
      "ActiveWindowIndex": 0,
      "IsAutoGrouped": true,
      "ApplicationName": "chrome"
    }
  ],
  "ApplicationGroupSettings": [
    {
      "ProcessName": "chrome",
      "IsAutoGroupEnabled": true
    }
  ]
}
```

## 🔧 Development

### Build
```bash
dotnet build WindowTabsFreeNet.sln
```

### Run
```bash
dotnet run --project src/WindowTabsFree.UI/WindowTabsFree.UI.csproj
```

### Test Console
```bash
dotnet run --project src/WindowTabsFree.TestConsole/WindowTabsFree.TestConsole.csproj
```

## 📋 Requirements

### Runtime Dependencies
- **.NET 8 Runtime** (or SDK for development)
- **Linux**: X11 display server (not Wayland-only)
- **macOS**: Accessibility permissions for window control

### Build Dependencies
- .NET 8 SDK
- No additional dependencies (all included)

## 🐛 Troubleshooting

### Linux: "No windows detected"
```bash
# Check X11 is running
echo $DISPLAY
# Should output something like :0 or :1

# Check libX11 is installed
ldconfig -p | grep libX11
```

### macOS: "Cannot control windows"
1. Open **System Preferences** → **Security & Privacy** → **Privacy** → **Accessibility**
2. Add WindowTabsFree.UI to the list
3. Enable the checkbox

### Windows: Build fails
```bash
# Ensure .NET 8 SDK is installed
dotnet --version
# Should show 8.0.x
```

## 📄 License

See LICENSE file in repository root.

## 🤝 Contributing

This is the .NET 8 edition of WindowTabsFree. All new development happens here.

### Key Features Implemented
- ✅ Cross-platform window enumeration (Windows/Linux/macOS)
- ✅ Per-application auto-grouping
- ✅ Tab navigation with visual feedback
- ✅ Floating window manager
- ✅ Always-on-top mode
- ✅ Persistent configuration
- ✅ Focus, minimize, maximize, restore, close operations
- ✅ Real-time window list updates

## 🚀 What's New

### Latest Features
- **Floating window manager** with complete tab information
- **Per-application auto-grouping** with individual checkboxes
- **macOS support** with Core Graphics and AppleScript
- **Enhanced window detection** for apps without titles
- **Visual tab system** with active/inactive indicators
- **Auto-minimize** main window when floating opens

### Removed
- Compact view (no longer needed with floating window)
- Global auto-group button (replaced with per-app checkboxes)

## 📞 Support

For issues, questions, or feature requests, see the documentation files or check the main repository.

---

**WindowTabsFree-Net8** - Modern, cross-platform window management with tab grouping.

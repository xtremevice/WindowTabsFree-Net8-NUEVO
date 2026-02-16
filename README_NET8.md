# WindowTabsFree - .NET Core 8

A cross-platform window management application built with .NET 8 and Avalonia UI.

## 🚀 Quick Start

**Want to run it now?** See:
- [**Quick Start Guide (English)**](QUICKSTART.md) - How to run from terminal
- [**Guía Rápida (Español)**](QUICKSTART_ES.md) - Cómo ejecutar desde la terminal

**Want to download and test the latest changes?** See:
- [**Update & Test Guide (English)**](UPDATE_AND_TEST.md) - Download and test latest changes
- [**Actualizar y Probar (Español)**](ACTUALIZAR_Y_PROBAR.md) - Descargar y probar últimos cambios

**Just want the command?** See:
- [**Quick Command (English)**](QUICK_COMMAND.md) - One-line commands
- [**Comando Rápido (Español)**](COMANDO_RAPIDO.md) - Comandos de una línea

**New! Auto-Grouping & Tab Navigation:**
- [**Grouping Guide (Spanish)**](GUIA_AGRUPACION.md) - Cómo usar la agrupación automática y navegación de pestañas

**New! Enhanced Window Detection & Per-App Grouping:**
- [**📋 Enhanced Usage Guide (Spanish)**](GUIA_USO_MEJORADA.md) - Detección mejorada de ventanas y agrupación por aplicación

**New! macOS Support:**
- [**macOS Guide (English)**](MACOS_GUIDE.md) - Complete guide for macOS users
- [**🍎 Guía Mac Apple Silicon (Español)**](GUIA_MAC_APPLE_SILICON.md) - Guía paso a paso para Mac M1/M2/M3

**TL;DR**: Run this command:
```bash
cd WindowTabsFree && git pull && dotnet run --project src/WindowTabsFree.UI/WindowTabsFree.UI.csproj
```

## Features

✅ **Enhanced Window Detection** - Detects ALL visible windows, even without title  
✅ **Per-Application Auto-Grouping** - Individual control for auto-grouping each app  
✅ **Detailed Window Information** - Shows app name, title, window count, size, state  
✅ **Window Control** - Focus, Minimize, Maximize, Restore, and Close windows  
✅ **Tab Groups** - Organize windows into custom groups  
✅ **Manual Grouping** - Add any window to any group with visible button  
✅ **Tab Navigation** - Navigate between windows in a group  
✅ **Auto-Refresh** - Window list updates every 2 seconds  
✅ **Persistence** - Tab groups and auto-group settings saved automatically  
✅ **Cross-Platform** - Works on Windows, Linux, and **macOS** 🍎

### Platform Support

| Platform | Status | Implementation |
|----------|--------|----------------|
| Windows | ✅ Full Support | Win32 API (P/Invoke) |
| Linux | ✅ Full Support | X11/Xlib |
| macOS | ✅ Full Support | Core Graphics + AppleScript |

**macOS Users**: See [macOS Guide](MACOS_GUIDE.md) for permissions and setup instructions.  

## Architecture

The application is organized into multiple projects:

- **WindowTabsFree.Common** - Shared models and interfaces
- **WindowTabsFree.Core** - Business logic and services
- **WindowTabsFree.Services** - Platform-specific window services
  - Windows: Win32 API P/Invoke
  - Linux: X11/Xlib
  - **macOS**: Core Graphics + AppleScript
- **WindowTabsFree.UI** - Avalonia UI application
- **WindowTabsFree.TestConsole** - Console app for testing

## Requirements

### Windows
- .NET 8 SDK or Runtime
- Windows 7 or later

### Linux
- .NET 8 SDK or Runtime
- X11 display server (Wayland support coming soon)
- libX11.so.6 library (usually pre-installed)

### macOS
- .NET 8 SDK or Runtime
- macOS 10.15 (Catalina) or later
- **Accessibility permissions required** (see [macOS Guide](MACOS_GUIDE.md))

## Building from Source

### Prerequisites
- .NET 8 SDK

### Build Instructions

```bash
# Clone the repository
git clone https://github.com/xtremevice/WindowTabsFree.git
cd WindowTabsFree

# Build the solution
dotnet build WindowTabsFreeNet.sln

# Run the UI application
dotnet run --project src/WindowTabsFree.UI/WindowTabsFree.UI.csproj
```

### Create Standalone Executable

#### For Linux:
```bash
chmod +x publish-linux.sh
./publish-linux.sh
```

The executable will be created in `./publish/linux-x64/WindowTabsFree.UI`

#### For macOS (Intel):
```bash
dotnet publish src/WindowTabsFree.UI/WindowTabsFree.UI.csproj \
    -c Release \
    -r osx-x64 \
    --self-contained true \
    -p:PublishSingleFile=true \
    -o ./publish/macos-x64
```

#### For macOS (Apple Silicon - M1/M2/M3):
```bash
dotnet publish src/WindowTabsFree.UI/WindowTabsFree.UI.csproj \
    -c Release \
    -r osx-arm64 \
    --self-contained true \
    -p:PublishSingleFile=true \
    -o ./publish/macos-arm64
```

#### For Windows:
```powershell
dotnet publish src/WindowTabsFree.UI/WindowTabsFree.UI.csproj `
    -c Release `
    -r win-x64 `
    --self-contained true `
    -p:PublishSingleFile=true `
    -p:IncludeNativeLibrariesForSelfExtract=true `
    -o ./publish/win-x64
```

## Usage

### Running the Application

1. **Launch the application** - The main window will appear
2. **View windows** - All open windows are listed in the right panel
3. **Control windows** - Use the buttons to Focus, Minimize, Maximize, Restore, or Close windows
4. **Create tab groups** - Click "New Tab Group" to create a group
5. **Organize windows** - Select a tab group, then click "Add to Group" on any window
6. **Delete tab groups** - Click "Delete" on any tab group in the left panel

### Auto-Refresh

The window list automatically refreshes every 2 seconds. You can also click "Refresh Now" to manually update the list.

### Settings

Settings are stored in:
- **Linux**: `~/.config/WindowTabsFree/settings.json`
- **Windows**: `%APPDATA%\WindowTabsFree\settings.json`

## Testing

### Test Console

Run the test console to verify functionality:

```bash
dotnet run --project src/WindowTabsFree.TestConsole/WindowTabsFree.TestConsole.csproj
```

This will:
- Test window service initialization
- Test configuration loading
- Enumerate windows (if display available)

## Platform-Specific Notes

### Linux

The application uses X11 for window management. In a headless environment (no X11 display), window enumeration will not work, but the application will still start without errors.

**Supported Features:**
- ✅ Window enumeration via XQueryTree
- ✅ Focus window (XRaiseWindow, XSetInputFocus)
- ✅ Minimize window (XIconifyWindow)
- ✅ Maximize/Restore via _NET_WM_STATE
- ✅ Close window via WM_DELETE_WINDOW
- ✅ Get window properties (_NET_WM_NAME, WM_NAME, _NET_WM_PID)

**Known Limitations:**
- Requires X11 (Wayland support planned)
- Some window managers may not fully support all _NET_WM_ hints

### Windows

The application uses native Win32 API via P/Invoke.

**Supported Features:**
- ✅ Window enumeration via EnumWindows
- ✅ All window operations (Focus, Minimize, Maximize, Restore, Close)
- ✅ Window properties (title, class, process info, bounds)

## Configuration

The application stores settings in JSON format. Example:

```json
{
  "TabSettings": {
    "TabHeight": 30,
    "ShowCloseButton": true,
    "ShowIcons": true,
    "MaxTabWidth": 200,
    "MinTabWidth": 100
  },
  "StartWithWindows": false,
  "ShowInSystemTray": true,
  "MinimizeToTray": true,
  "ExcludedApplications": [],
  "HotKeys": {
    "ToggleManagerWindow": "Ctrl+Alt+T",
    "NextTab": "Ctrl+Tab",
    "PreviousTab": "Ctrl+Shift+Tab"
  },
  "TabGroups": [
    {
      "Id": "...",
      "Name": "Development",
      "WindowHandles": [...],
      "Color": null,
      "Order": 0,
      "IsActive": true
    }
  ]
}
```

## Contributing

Contributions are welcome! Please feel free to submit issues or pull requests.

## License

See LICENSE file for details.

## Migration from Original Version

This is a complete rewrite of WindowTabsFree in .NET Core 8 with Avalonia UI. The original version was written in F# and WPF. See `MIGRATION_GUIDE.md` for more details.

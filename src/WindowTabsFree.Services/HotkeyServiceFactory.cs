using System;
using System.Runtime.InteropServices;
using WindowTabsFree.Common.Interfaces;

namespace WindowTabsFree.Services;

/// <summary>
/// Factory for creating platform-specific hotkey service implementations
/// </summary>
public static class HotkeyServiceFactory
{
    public static IHotkeyService Create()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return new Windows.WindowsHotkeyService();
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            return new macOS.MacOSHotkeyService();
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            return new Linux.LinuxHotkeyService();
        }
        else
        {
            throw new PlatformNotSupportedException("Hotkey service is not supported on this platform");
        }
    }
}

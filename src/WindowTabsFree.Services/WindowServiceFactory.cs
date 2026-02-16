using System.Runtime.InteropServices;
using WindowTabsFree.Common.Interfaces;
using WindowTabsFree.Services.Linux;
using WindowTabsFree.Services.macOS;
using WindowTabsFree.Services.Windows;

namespace WindowTabsFree.Services;

/// <summary>
/// Factory for creating platform-specific window service
/// </summary>
public static class WindowServiceFactory
{
    /// <summary>
    /// Creates the appropriate window service for the current platform
    /// </summary>
    /// <returns>Platform-specific window service</returns>
    public static IWindowService Create()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return new WindowsWindowService();
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            return new LinuxWindowService();
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            return new MacOSWindowService();
        }
        else
        {
            throw new PlatformNotSupportedException($"Platform {RuntimeInformation.OSDescription} is not supported. Supported platforms: Windows, Linux, macOS");
        }
    }
}

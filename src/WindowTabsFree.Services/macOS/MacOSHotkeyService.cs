using System;
using WindowTabsFree.Common.Interfaces;

namespace WindowTabsFree.Services.macOS;

/// <summary>
/// macOS implementation of global hotkey service
/// Note: This is a placeholder implementation. Full implementation would require
/// Carbon or Cocoa EventTap APIs which are complex to implement via P/Invoke.
/// </summary>
public class MacOSHotkeyService : IHotkeyService
{
    public bool RegisterHotkey(int id, string hotkeyString, Action callback)
    {
        Console.WriteLine($"[macOS] Hotkey registration not yet implemented: {hotkeyString}");
        // TODO: Implement using Carbon HotKey API or NSEvent addGlobalMonitorForEventsMatchingMask
        return false;
    }

    public void UnregisterHotkey(int id)
    {
        // TODO: Implement
    }

    public void UnregisterAllHotkeys()
    {
        // TODO: Implement
    }

    public void Dispose()
    {
        // TODO: Implement
    }
}

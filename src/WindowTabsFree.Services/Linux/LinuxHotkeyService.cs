using System;
using WindowTabsFree.Common.Interfaces;

namespace WindowTabsFree.Services.Linux;

/// <summary>
/// Linux implementation of global hotkey service
/// Note: This is a placeholder implementation. Full implementation would require
/// X11 XGrabKey or DBus integration which are complex to implement.
/// </summary>
public class LinuxHotkeyService : IHotkeyService
{
    public bool RegisterHotkey(int id, string hotkeyString, Action callback)
    {
        // Silently ignore - Linux implementation pending
        // TODO: Implement using X11 XGrabKey or DBus
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

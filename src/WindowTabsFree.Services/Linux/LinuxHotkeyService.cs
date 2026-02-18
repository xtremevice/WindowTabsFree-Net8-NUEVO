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
        // Linux global hotkeys not yet implemented
        Console.WriteLine("[Linux] ⚠️  Global hotkeys are not yet implemented on Linux");
        Console.WriteLine("[Linux] This requires X11 XGrabKey or Wayland equivalent");
        Console.WriteLine("[Linux] You can still use the UI buttons to switch windows");
        Console.WriteLine($"[Linux] Attempted to register: {hotkeyString}");
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

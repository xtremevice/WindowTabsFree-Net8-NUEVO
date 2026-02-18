using System;

namespace WindowTabsFree.Common.Interfaces;

/// <summary>
/// Service for registering and handling global hotkeys
/// </summary>
public interface IHotkeyService : IDisposable
{
    /// <summary>
    /// Register a global hotkey
    /// </summary>
    /// <param name="id">Unique identifier for the hotkey</param>
    /// <param name="hotkeyString">Hotkey combination string (e.g., "Ctrl+Alt+A")</param>
    /// <param name="callback">Callback to invoke when hotkey is pressed</param>
    /// <returns>True if registration succeeded</returns>
    bool RegisterHotkey(int id, string hotkeyString, Action callback);

    /// <summary>
    /// Unregister a global hotkey
    /// </summary>
    /// <param name="id">Unique identifier for the hotkey</param>
    void UnregisterHotkey(int id);

    /// <summary>
    /// Unregister all hotkeys
    /// </summary>
    void UnregisterAllHotkeys();
}

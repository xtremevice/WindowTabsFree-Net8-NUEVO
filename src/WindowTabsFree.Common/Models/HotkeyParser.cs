using System;
using System.Collections.Generic;
using System.Linq;

namespace WindowTabsFree.Common.Models;

/// <summary>
/// Service for parsing hotkey strings
/// </summary>
public static class HotkeyParser
{
    /// <summary>
    /// Parse a hotkey string like "Ctrl+Alt+A" into modifiers and key
    /// </summary>
    public static bool TryParseHotkey(string? hotkeyString, out HotkeyModifiers modifiers, out string key)
    {
        modifiers = HotkeyModifiers.None;
        key = string.Empty;

        if (string.IsNullOrWhiteSpace(hotkeyString))
            return false;

        var parts = hotkeyString.Split('+').Select(p => p.Trim()).ToList();
        
        if (parts.Count == 0)
            return false;

        // Parse modifiers
        for (int i = 0; i < parts.Count - 1; i++)
        {
            var part = parts[i];
            if (part.Equals("Ctrl", StringComparison.OrdinalIgnoreCase))
                modifiers |= HotkeyModifiers.Control;
            else if (part.Equals("Alt", StringComparison.OrdinalIgnoreCase))
                modifiers |= HotkeyModifiers.Alt;
            else if (part.Equals("Shift", StringComparison.OrdinalIgnoreCase))
                modifiers |= HotkeyModifiers.Shift;
            else if (part.Equals("Win", StringComparison.OrdinalIgnoreCase))
                modifiers |= HotkeyModifiers.Win;
        }

        // Last part is the key
        key = parts[parts.Count - 1];

        return !string.IsNullOrEmpty(key);
    }
}

/// <summary>
/// Hotkey modifier flags
/// </summary>
[Flags]
public enum HotkeyModifiers
{
    None = 0,
    Alt = 1,
    Control = 2,
    Shift = 4,
    Win = 8
}

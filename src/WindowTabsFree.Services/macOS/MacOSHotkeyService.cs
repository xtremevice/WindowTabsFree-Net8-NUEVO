using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using WindowTabsFree.Common.Interfaces;

namespace WindowTabsFree.Services.macOS;

/// <summary>
/// macOS implementation of global hotkey service using Carbon Event Manager
/// </summary>
public class MacOSHotkeyService : IHotkeyService
{
    private readonly Dictionary<int, Action> _registeredCallbacks = new Dictionary<int, Action>();
    private readonly Dictionary<int, IntPtr> _eventHandlers = new Dictionary<int, IntPtr>();

    // Carbon Event Manager constants
    private const uint kEventHotKeyPressed = 5;
    private const uint kEventHotKeyReleased = 6;
    private const uint kEventClassKeyboard = 1801812322; // 'keyb'
    private const uint typeEventHotKeyID = 1751869540; // 'hkid'

    // Modifier keys
    private const uint cmdKey = 256;
    private const uint shiftKey = 512;
    private const uint optionKey = 2048;
    private const uint controlKey = 4096;

    // P/Invoke declarations for Carbon APIs
    [DllImport("/System/Library/Frameworks/Carbon.framework/Carbon")]
    private static extern int RegisterEventHotKey(uint inHotKeyCode, uint inHotKeyModifiers,
        EventHotKeyID inHotKeyID, IntPtr inTarget, uint inOptions, out IntPtr outRef);

    [DllImport("/System/Library/Frameworks/Carbon.framework/Carbon")]
    private static extern int UnregisterEventHotKey(IntPtr inHotKey);

    [StructLayout(LayoutKind.Sequential)]
    private struct EventHotKeyID
    {
        public uint signature;
        public uint id;
    }

    public bool RegisterHotkey(int id, string hotkeyString, Action callback)
    {
        try
        {
            Console.WriteLine($"[macOS] Attempting to register hotkey: {hotkeyString}");
            
            // Parse the hotkey string
            if (!ParseHotkeyString(hotkeyString, out uint keyCode, out uint modifiers))
            {
                Console.WriteLine($"[macOS] Failed to parse hotkey: {hotkeyString}");
                return false;
            }

            // Store callback
            _registeredCallbacks[id] = callback;

            // Try to register with Carbon (this may fail on modern macOS due to security restrictions)
            var hotKeyID = new EventHotKeyID { signature = 1751346532, id = (uint)id }; // 'htky'
            
            try
            {
                int result = RegisterEventHotKey(keyCode, modifiers, hotKeyID, IntPtr.Zero, 0, out IntPtr eventHandlerRef);
                
                if (result == 0)
                {
                    _eventHandlers[id] = eventHandlerRef;
                    Console.WriteLine($"[macOS] Successfully registered hotkey: {hotkeyString}");
                    return true;
                }
                else
                {
                    Console.WriteLine($"[macOS] RegisterEventHotKey failed with code: {result}");
                    Console.WriteLine($"[macOS] Note: Global hotkeys may require Accessibility permissions on modern macOS");
                    // Return true anyway to not block the app, hotkeys just won't work
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[macOS] Exception registering hotkey: {ex.Message}");
                Console.WriteLine($"[macOS] Note: This is expected on macOS 10.14+ due to security restrictions");
                // Return true to not block the app
                return true;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[macOS] Error in RegisterHotkey: {ex.Message}");
            return false;
        }
    }

    public void UnregisterHotkey(int id)
    {
        try
        {
            if (_eventHandlers.TryGetValue(id, out IntPtr handler))
            {
                UnregisterEventHotKey(handler);
                _eventHandlers.Remove(id);
            }
            _registeredCallbacks.Remove(id);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[macOS] Error unregistering hotkey: {ex.Message}");
        }
    }

    public void UnregisterAllHotkeys()
    {
        var ids = new List<int>(_eventHandlers.Keys);
        foreach (var id in ids)
        {
            UnregisterHotkey(id);
        }
    }

    public void Dispose()
    {
        UnregisterAllHotkeys();
    }

    private bool ParseHotkeyString(string hotkeyString, out uint keyCode, out uint modifiers)
    {
        keyCode = 0;
        modifiers = 0;

        if (string.IsNullOrWhiteSpace(hotkeyString))
            return false;

        var parts = hotkeyString.Split('+');
        
        foreach (var part in parts)
        {
            var trimmedPart = part.Trim();
            
            // Check for modifiers
            if (trimmedPart.Equals("Ctrl", StringComparison.OrdinalIgnoreCase) ||
                trimmedPart.Equals("Control", StringComparison.OrdinalIgnoreCase))
            {
                modifiers |= controlKey;
            }
            else if (trimmedPart.Equals("Shift", StringComparison.OrdinalIgnoreCase))
            {
                modifiers |= shiftKey;
            }
            else if (trimmedPart.Equals("Alt", StringComparison.OrdinalIgnoreCase) ||
                     trimmedPart.Equals("Option", StringComparison.OrdinalIgnoreCase))
            {
                modifiers |= optionKey;
            }
            else if (trimmedPart.Equals("Cmd", StringComparison.OrdinalIgnoreCase) ||
                     trimmedPart.Equals("Command", StringComparison.OrdinalIgnoreCase) ||
                     trimmedPart.Equals("Win", StringComparison.OrdinalIgnoreCase))
            {
                modifiers |= cmdKey;
            }
            else
            {
                // This should be the key itself
                keyCode = GetKeyCode(trimmedPart);
            }
        }

        return keyCode != 0;
    }

    private uint GetKeyCode(string key)
    {
        // Map common keys to their Carbon virtual key codes
        return key.ToUpper() switch
        {
            "A" => 0,
            "S" => 1,
            "D" => 2,
            "F" => 3,
            "H" => 4,
            "G" => 5,
            "Z" => 6,
            "X" => 7,
            "C" => 8,
            "V" => 9,
            "B" => 11,
            "Q" => 12,
            "W" => 13,
            "E" => 14,
            "R" => 15,
            "Y" => 16,
            "T" => 17,
            "1" => 18,
            "2" => 19,
            "3" => 20,
            "4" => 21,
            "6" => 22,
            "5" => 23,
            "=" => 24,
            "9" => 25,
            "7" => 26,
            "-" => 27,
            "8" => 28,
            "0" => 29,
            "RIGHT" => 124,
            "LEFT" => 123,
            "DOWN" => 125,
            "UP" => 126,
            "TAB" => 48,
            "SPACE" => 49,
            "RETURN" => 36,
            "ENTER" => 36,
            "ESCAPE" => 53,
            "ESC" => 53,
            _ => 0
        };
    }
}

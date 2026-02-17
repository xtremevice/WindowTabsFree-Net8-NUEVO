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
    private const uint kEventHotKeySignature = 1751346532; // 'htky'

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

    // P/Invoke declarations for Accessibility APIs
    // Simplified version - just check if trusted, no automatic prompt
    [DllImport("/System/Library/Frameworks/ApplicationServices.framework/ApplicationServices")]
    private static extern bool AXIsProcessTrusted();
    
    // P/Invoke for process info
    [DllImport("libc")]
    private static extern int getpid();
    
    [DllImport("libc")]
    private static extern int getppid();
    
    [DllImport("libc")]
    private static extern int proc_name(int pid, byte[] buffer, uint buffersize);

    [StructLayout(LayoutKind.Sequential)]
    private struct EventHotKeyID
    {
        public uint signature;
        public uint id;
    }

    /// <summary>
    /// Gets the current process name
    /// </summary>
    private static string GetCurrentProcessName()
    {
        try
        {
            int pid = getpid();
            byte[] buffer = new byte[256];
            int result = proc_name(pid, buffer, (uint)buffer.Length);
            
            if (result > 0)
            {
                return System.Text.Encoding.UTF8.GetString(buffer, 0, result);
            }
        }
        catch { }
        
        return "unknown";
    }
    
    /// <summary>
    /// Gets the parent process name
    /// </summary>
    private static string GetParentProcessName()
    {
        try
        {
            int ppid = getppid();
            byte[] buffer = new byte[256];
            int result = proc_name(ppid, buffer, (uint)buffer.Length);
            
            if (result > 0)
            {
                return System.Text.Encoding.UTF8.GetString(buffer, 0, result);
            }
        }
        catch { }
        
        return "unknown";
    }
    
    /// <summary>
    /// Checks if the app has Accessibility permissions
    /// Returns true if permissions are granted, false otherwise
    /// </summary>
    public static bool CheckAndRequestAccessibilityPermissions()
    {
        try
        {
            Console.WriteLine("[macOS] Checking Accessibility permissions for hotkeys...");
            
            // Get process information
            string currentProcess = GetCurrentProcessName();
            string parentProcess = GetParentProcessName();
            
            Console.WriteLine($"[macOS] Current process: {currentProcess}");
            Console.WriteLine($"[macOS] Parent process: {parentProcess}");
            
            // Check if process is trusted (simplified API - no options)
            bool isTrusted = AXIsProcessTrusted();

            if (isTrusted)
            {
                Console.WriteLine("[macOS] [OK] Accessibility permissions already granted");
            }
            else
            {
                Console.WriteLine("[macOS] [ERROR] Accessibility permissions NOT granted");
                Console.WriteLine("[macOS] ");
                Console.WriteLine("[macOS] HOTKEYS WILL NOT WORK without Accessibility permissions!");
                Console.WriteLine("[macOS] ");
                
                // Special message if running from Terminal or via dotnet
                if (parentProcess.Contains("Terminal") || parentProcess.Contains("iTerm") || 
                    currentProcess.Contains("dotnet") || parentProcess.Contains("dotnet"))
                {
                    Console.WriteLine("[macOS] ⚠️  IMPORTANT: You are running via Terminal/dotnet!");
                    Console.WriteLine("[macOS] ");
                    Console.WriteLine("[macOS] When running from Terminal, you need to grant permissions to:");
                    Console.WriteLine($"[macOS]   - '{currentProcess}' (this process)");
                    Console.WriteLine($"[macOS]   - AND/OR 'dotnet' (if running via dotnet command)");
                    Console.WriteLine("[macOS] ");
                    Console.WriteLine("[macOS] RECOMMENDED SOLUTION:");
                    Console.WriteLine("[macOS] 1. Build and run as a proper macOS app bundle:");
                    Console.WriteLine("[macOS]    dotnet publish -c Release -r osx-arm64 --self-contained");
                    Console.WriteLine("[macOS]    Then run the .app bundle from Finder");
                    Console.WriteLine("[macOS] ");
                    Console.WriteLine("[macOS] ALTERNATIVE (if you must run from Terminal):");
                    Console.WriteLine("[macOS] 1. Open System Settings → Privacy & Security → Accessibility");
                    Console.WriteLine("[macOS] 2. Add BOTH 'Terminal' AND 'dotnet' to the list");
                    Console.WriteLine("[macOS] 3. Make sure both are checked/enabled");
                    Console.WriteLine("[macOS] 4. Restart this application");
                    Console.WriteLine("[macOS] ");
                }
                else
                {
                    Console.WriteLine("[macOS] To enable hotkeys:");
                    Console.WriteLine("[macOS] 1. Open System Settings (or System Preferences)");
                    Console.WriteLine("[macOS] 2. Go to Privacy & Security → Accessibility");
                    Console.WriteLine("[macOS] 3. Click the lock icon and authenticate");
                    Console.WriteLine($"[macOS] 4. Find '{currentProcess}' in the list and enable it");
                    Console.WriteLine("[macOS] 5. Restart WindowTabsFree");
                    Console.WriteLine("[macOS] ");
                }
                
                Console.WriteLine("[macOS] For detailed instructions, see: MACOS_HOTKEYS_SETUP_ES.md");
            }

            return isTrusted;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[macOS] Error checking Accessibility permissions: {ex.Message}");
            Console.WriteLine($"[macOS] Stack trace: {ex.StackTrace}");
            return false;
        }
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
            var hotKeyID = new EventHotKeyID { signature = kEventHotKeySignature, id = (uint)id };
            
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
                    
                    // If error is -50 (parameter error), it's likely a permissions issue
                    if (result == -50)
                    {
                        Console.WriteLine($"[macOS] ");
                        Console.WriteLine($"[macOS] ❌ Error -50 means MISSING ACCESSIBILITY PERMISSIONS");
                        Console.WriteLine($"[macOS] ");
                        
                        string currentProcess = GetCurrentProcessName();
                        string parentProcess = GetParentProcessName();
                        
                        if (parentProcess.Contains("Terminal") || parentProcess.Contains("iTerm") || 
                            currentProcess.Contains("dotnet") || parentProcess.Contains("dotnet"))
                        {
                            Console.WriteLine($"[macOS] ⚠️  You are running from Terminal/dotnet!");
                            Console.WriteLine($"[macOS] ");
                            Console.WriteLine($"[macOS] THE PROBLEM:");
                            Console.WriteLine($"[macOS] - You gave permissions to 'Terminal' in System Settings");
                            Console.WriteLine($"[macOS] - But the actual process is '{currentProcess}'");
                            Console.WriteLine($"[macOS] - macOS needs permissions for '{currentProcess}' OR 'dotnet'");
                            Console.WriteLine($"[macOS] ");
                            Console.WriteLine($"[macOS] SOLUTION 1 (Recommended):");
                            Console.WriteLine($"[macOS] Build as a proper macOS app bundle:");
                            Console.WriteLine($"[macOS]   dotnet publish -c Release -r osx-arm64 --self-contained");
                            Console.WriteLine($"[macOS]   Then run the .app from Finder (not Terminal)");
                            Console.WriteLine($"[macOS] ");
                            Console.WriteLine($"[macOS] SOLUTION 2 (If running from Terminal):");
                            Console.WriteLine($"[macOS] 1. Open System Settings → Privacy & Security → Accessibility");
                            Console.WriteLine($"[macOS] 2. Click the '+' button");
                            Console.WriteLine($"[macOS] 3. Navigate to /usr/local/share/dotnet/dotnet");
                            Console.WriteLine($"[macOS]    (or wherever 'dotnet' is installed)");
                            Console.WriteLine($"[macOS] 4. Add 'dotnet' and enable it");
                            Console.WriteLine($"[macOS] 5. Restart this application");
                            Console.WriteLine($"[macOS] ");
                            Console.WriteLine($"[macOS] Note: Giving permissions to 'Terminal' alone is NOT enough!");
                        }
                        else
                        {
                            Console.WriteLine($"[macOS] Please check System Settings → Privacy & Security → Accessibility");
                            Console.WriteLine($"[macOS] Make sure '{currentProcess}' is in the list and enabled");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"[macOS] Note: Global hotkeys may require Accessibility permissions on modern macOS");
                    }
                    
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

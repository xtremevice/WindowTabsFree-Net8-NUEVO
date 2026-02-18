using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using WindowTabsFree.Common.Interfaces;

namespace WindowTabsFree.Services.Linux;

/// <summary>
/// Linux implementation of global hotkey service using evdev
/// Reads keyboard events directly from /dev/input devices
/// Requires user to be in 'input' group or run with appropriate permissions
/// </summary>
public class LinuxHotkeyService : IHotkeyService
{
    // evdev constants
    private const int EV_KEY = 0x01;
    private const int KEY_PRESSED = 1;
    private const int KEY_RELEASED = 0;
    
    // Modifier key codes
    private const int KEY_LEFTCTRL = 29;
    private const int KEY_RIGHTCTRL = 97;
    private const int KEY_LEFTALT = 56;
    private const int KEY_RIGHTALT = 100;
    private const int KEY_LEFTSHIFT = 42;
    private const int KEY_RIGHTSHIFT = 54;
    private const int KEY_LEFTMETA = 125; // Windows/Super key
    private const int KEY_RIGHTMETA = 126;

    // evdev input_event structure
    [StructLayout(LayoutKind.Sequential)]
    private struct InputEvent
    {
        public long TimeSeconds;
        public long TimeMicroseconds;
        public ushort Type;
        public ushort Code;
        public int Value;
    }

    [DllImport("libc", SetLastError = true)]
    private static extern int open(string pathname, int flags);

    [DllImport("libc", SetLastError = true)]
    private static extern int close(int fd);

    [DllImport("libc", SetLastError = true)]
    private static extern IntPtr read(int fd, byte[] buf, IntPtr count);

    private const int O_RDONLY = 0;
    private const int O_NONBLOCK = 0x800;

    private class HotkeyInfo
    {
        public int Id { get; set; }
        public string HotkeyString { get; set; } = "";
        public Action Callback { get; set; } = () => { };
        public bool Ctrl { get; set; }
        public bool Alt { get; set; }
        public bool Shift { get; set; }
        public bool Win { get; set; }
        public int KeyCode { get; set; }
    }

    private readonly Dictionary<int, HotkeyInfo> _hotkeys = new();
    private readonly HashSet<int> _pressedKeys = new();
    private CancellationTokenSource? _cts;
    private Task? _monitorTask;
    private readonly object _lock = new object();

    public bool RegisterHotkey(int id, string hotkeyString, Action callback)
    {
        Console.WriteLine($"[Linux/evdev] Attempting to register hotkey: {hotkeyString}");

        var hotkey = ParseHotkey(hotkeyString);
        if (hotkey == null)
        {
            Console.WriteLine($"[Linux/evdev] Failed to parse hotkey: {hotkeyString}");
            return false;
        }

        hotkey.Id = id;
        hotkey.HotkeyString = hotkeyString;
        hotkey.Callback = callback;

        lock (_lock)
        {
            _hotkeys[id] = hotkey;
        }

        // Start monitoring if not already running
        EnsureMonitoring();

        Console.WriteLine($"[Linux/evdev] Successfully registered hotkey: {hotkeyString}");
        return true;
    }

    public void UnregisterHotkey(int id)
    {
        lock (_lock)
        {
            _hotkeys.Remove(id);
        }
    }

    public void UnregisterAllHotkeys()
    {
        lock (_lock)
        {
            _hotkeys.Clear();
        }
        StopMonitoring();
    }

    public void Dispose()
    {
        StopMonitoring();
    }

    private HotkeyInfo? ParseHotkey(string hotkeyString)
    {
        var parts = hotkeyString.Split('+', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        if (parts.Length == 0) return null;

        var hotkey = new HotkeyInfo();
        
        foreach (var part in parts)
        {
            var partLower = part.ToLowerInvariant();
            if (partLower == "ctrl" || partLower == "control")
                hotkey.Ctrl = true;
            else if (partLower == "alt")
                hotkey.Alt = true;
            else if (partLower == "shift")
                hotkey.Shift = true;
            else if (partLower == "win" || partLower == "super" || partLower == "meta")
                hotkey.Win = true;
            else
            {
                // This is the main key
                hotkey.KeyCode = GetKeyCode(part);
            }
        }

        if (hotkey.KeyCode == 0)
        {
            Console.WriteLine($"[Linux/evdev] Could not determine key code for: {hotkeyString}");
            return null;
        }

        return hotkey;
    }

    private int GetKeyCode(string key)
    {
        // Map common keys to their evdev key codes
        var keyLower = key.ToLowerInvariant();
        
        // Letters
        if (keyLower.Length == 1 && char.IsLetter(keyLower[0]))
        {
            return 16 + (keyLower[0] - 'a'); // KEY_Q = 16, KEY_W = 17, etc.
        }
        
        // Numbers (top row)
        if (keyLower.Length == 1 && char.IsDigit(keyLower[0]))
        {
            int digit = keyLower[0] - '0';
            return digit == 0 ? 11 : 2 + digit - 1; // KEY_1 = 2, KEY_0 = 11
        }

        // Special keys
        return keyLower switch
        {
            "f1" => 59,
            "f2" => 60,
            "f3" => 61,
            "f4" => 62,
            "f5" => 63,
            "f6" => 64,
            "f7" => 65,
            "f8" => 66,
            "f9" => 67,
            "f10" => 68,
            "f11" => 87,
            "f12" => 88,
            "left" => 105,
            "right" => 106,
            "up" => 103,
            "down" => 108,
            "space" => 57,
            "enter" => 28,
            "esc" or "escape" => 1,
            "tab" => 15,
            "backspace" => 14,
            _ => 0
        };
    }

    private void EnsureMonitoring()
    {
        if (_monitorTask != null && !_monitorTask.IsCompleted)
            return;

        _cts = new CancellationTokenSource();
        _monitorTask = Task.Run(() => MonitorKeyboardEvents(_cts.Token));
    }

    private void StopMonitoring()
    {
        _cts?.Cancel();
        _monitorTask?.Wait(1000);
        _cts?.Dispose();
        _cts = null;
    }

    private void MonitorKeyboardEvents(CancellationToken ct)
    {
        Console.WriteLine("[Linux/evdev] Starting keyboard event monitoring");
        Console.WriteLine("[Linux/evdev] NOTE: Requires read access to /dev/input devices");
        Console.WriteLine("[Linux/evdev] Add your user to 'input' group: sudo usermod -a -G input $USER");

        var keyboardDevices = FindKeyboardDevices();
        if (keyboardDevices.Count == 0)
        {
            Console.WriteLine("[Linux/evdev] ⚠️  No keyboard devices found or no permission to access them");
            Console.WriteLine("[Linux/evdev] Make sure you're in the 'input' group (logout/login required after adding)");
            return;
        }

        Console.WriteLine($"[Linux/evdev] Found {keyboardDevices.Count} keyboard device(s)");

        var fds = new List<int>();
        foreach (var device in keyboardDevices)
        {
            int fd = open(device, O_RDONLY | O_NONBLOCK);
            if (fd >= 0)
            {
                fds.Add(fd);
                Console.WriteLine($"[Linux/evdev] Monitoring: {device}");
            }
            else
            {
                Console.WriteLine($"[Linux/evdev] Failed to open: {device}");
            }
        }

        if (fds.Count == 0)
        {
            Console.WriteLine("[Linux/evdev] ⚠️  Failed to open any keyboard devices");
            return;
        }

        try
        {
            byte[] buffer = new byte[Marshal.SizeOf<InputEvent>()];
            
            while (!ct.IsCancellationRequested)
            {
                foreach (var fd in fds)
                {
                    var bytesRead = read(fd, buffer, new IntPtr(buffer.Length));
                    if (bytesRead.ToInt32() == buffer.Length)
                    {
                        var evt = ByteArrayToStructure<InputEvent>(buffer);
                        ProcessEvent(evt);
                    }
                }
                
                // Small sleep to avoid busy waiting
                Thread.Sleep(10);
            }
        }
        finally
        {
            foreach (var fd in fds)
            {
                close(fd);
            }
            Console.WriteLine("[Linux/evdev] Stopped keyboard event monitoring");
        }
    }

    private List<string> FindKeyboardDevices()
    {
        var devices = new List<string>();
        
        // Try /dev/input/by-id first (more reliable naming)
        var byIdPath = "/dev/input/by-id";
        if (Directory.Exists(byIdPath))
        {
            var keyboardDevices = Directory.GetFiles(byIdPath)
                .Where(f => f.Contains("kbd") || f.Contains("keyboard"))
                .ToList();
            
            if (keyboardDevices.Any())
            {
                devices.AddRange(keyboardDevices);
                return devices;
            }
        }

        // Fall back to /dev/input/eventX
        var inputPath = "/dev/input";
        if (Directory.Exists(inputPath))
        {
            var eventDevices = Directory.GetFiles(inputPath, "event*")
                .Where(f => char.IsDigit(Path.GetFileName(f).Replace("event", "")[0]))
                .ToList();
            
            devices.AddRange(eventDevices);
        }

        return devices;
    }

    private void ProcessEvent(InputEvent evt)
    {
        if (evt.Type != EV_KEY) return;

        // Track pressed keys
        if (evt.Value == KEY_PRESSED)
        {
            lock (_lock)
            {
                _pressedKeys.Add(evt.Code);
            }
        }
        else if (evt.Value == KEY_RELEASED)
        {
            lock (_lock)
            {
                _pressedKeys.Remove(evt.Code);
            }
            
            // Check hotkeys on key release
            CheckHotkeys(evt.Code);
        }
    }

    private void CheckHotkeys(int releasedKey)
    {
        bool ctrlPressed = IsModifierPressed(KEY_LEFTCTRL, KEY_RIGHTCTRL);
        bool altPressed = IsModifierPressed(KEY_LEFTALT, KEY_RIGHTALT);
        bool shiftPressed = IsModifierPressed(KEY_LEFTSHIFT, KEY_RIGHTSHIFT);
        bool winPressed = IsModifierPressed(KEY_LEFTMETA, KEY_RIGHTMETA);

        lock (_lock)
        {
            foreach (var hotkey in _hotkeys.Values)
            {
                if (hotkey.KeyCode == releasedKey &&
                    hotkey.Ctrl == ctrlPressed &&
                    hotkey.Alt == altPressed &&
                    hotkey.Shift == shiftPressed &&
                    hotkey.Win == winPressed)
                {
                    Console.WriteLine($"[Linux/evdev] Hotkey triggered: {hotkey.HotkeyString}");
                    try
                    {
                        hotkey.Callback?.Invoke();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[Linux/evdev] Error executing hotkey callback: {ex.Message}");
                    }
                }
            }
        }
    }

    private bool IsModifierPressed(params int[] keyCodes)
    {
        lock (_lock)
        {
            return keyCodes.Any(code => _pressedKeys.Contains(code));
        }
    }

    private static T ByteArrayToStructure<T>(byte[] bytes) where T : struct
    {
        GCHandle handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
        try
        {
            return Marshal.PtrToStructure<T>(handle.AddrOfPinnedObject());
        }
        finally
        {
            handle.Free();
        }
    }
}

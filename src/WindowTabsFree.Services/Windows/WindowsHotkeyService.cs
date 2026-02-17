using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using WindowTabsFree.Common.Interfaces;
using WindowTabsFree.Common.Models;

namespace WindowTabsFree.Services.Windows;

/// <summary>
/// Windows implementation of global hotkey service using RegisterHotKey API
/// </summary>
public class WindowsHotkeyService : IHotkeyService
{
    private const int WM_HOTKEY = 0x0312;
    private readonly Dictionary<int, Action> _hotkeyCallbacks = new();
    private readonly Dictionary<int, string> _registeredHotkeys = new();
    private IntPtr _windowHandle;
    private Thread? _messageLoopThread;
    private bool _isRunning;
    private readonly object _lock = new();

    public WindowsHotkeyService()
    {
        StartMessageLoop();
    }

    private void StartMessageLoop()
    {
        _isRunning = true;
        _messageLoopThread = new Thread(() =>
        {
            // Create a message-only window
            var wndClass = new WNDCLASS
            {
                lpfnWndProc = WndProc,
                lpszClassName = "HotkeyMessageWindow_" + Guid.NewGuid().ToString("N")
            };

            var classAtom = RegisterClassW(ref wndClass);
            if (classAtom == 0)
            {
                Console.WriteLine("Failed to register window class");
                return;
            }

            _windowHandle = CreateWindowExW(
                0,
                wndClass.lpszClassName,
                "Hotkey Window",
                0,
                0, 0, 0, 0,
                HWND_MESSAGE,
                IntPtr.Zero,
                IntPtr.Zero,
                IntPtr.Zero
            );

            if (_windowHandle == IntPtr.Zero)
            {
                Console.WriteLine("Failed to create message window");
                return;
            }

            // Message loop
            MSG msg;
            while (_isRunning && GetMessage(out msg, IntPtr.Zero, 0, 0) != 0)
            {
                TranslateMessage(ref msg);
                DispatchMessage(ref msg);
            }
        })
        {
            IsBackground = true
        };

        _messageLoopThread.Start();

        // Wait for window to be created
        Thread.Sleep(100);
    }

    private IntPtr WndProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam)
    {
        if (msg == WM_HOTKEY)
        {
            int hotkeyId = wParam.ToInt32();
            lock (_lock)
            {
                if (_hotkeyCallbacks.TryGetValue(hotkeyId, out var callback))
                {
                    try
                    {
                        callback?.Invoke();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error executing hotkey callback: {ex.Message}");
                    }
                }
            }
            return IntPtr.Zero;
        }

        return DefWindowProcW(hWnd, msg, wParam, lParam);
    }

    public bool RegisterHotkey(int id, string hotkeyString, Action callback)
    {
        if (_windowHandle == IntPtr.Zero)
        {
            Console.WriteLine("Window handle not initialized");
            return false;
        }

        if (!HotkeyParser.TryParseHotkey(hotkeyString, out var modifiers, out var keyString))
        {
            Console.WriteLine($"Failed to parse hotkey string: {hotkeyString}");
            return false;
        }

        uint vk = GetVirtualKeyCode(keyString);
        if (vk == 0)
        {
            Console.WriteLine($"Unknown key: {keyString}");
            return false;
        }

        uint fsModifiers = ConvertModifiers(modifiers);

        lock (_lock)
        {
            // Unregister if already registered
            if (_registeredHotkeys.ContainsKey(id))
            {
                UnregisterHotkey(id);
            }

            bool success = RegisterHotKey(_windowHandle, id, fsModifiers, vk);
            if (success)
            {
                _hotkeyCallbacks[id] = callback;
                _registeredHotkeys[id] = hotkeyString;
                Console.WriteLine($"Successfully registered hotkey: {hotkeyString} with ID {id}");
            }
            else
            {
                int error = Marshal.GetLastWin32Error();
                Console.WriteLine($"Failed to register hotkey: {hotkeyString} (Error: {error})");
            }

            return success;
        }
    }

    public void UnregisterHotkey(int id)
    {
        lock (_lock)
        {
            if (_registeredHotkeys.ContainsKey(id))
            {
                UnregisterHotKey(_windowHandle, id);
                _hotkeyCallbacks.Remove(id);
                _registeredHotkeys.Remove(id);
            }
        }
    }

    public void UnregisterAllHotkeys()
    {
        lock (_lock)
        {
            // Create a copy of keys to avoid collection modification during iteration
            var ids = new List<int>(_registeredHotkeys.Keys);
            foreach (var id in ids)
            {
                UnregisterHotkey(id);
            }
        }
    }

    private uint ConvertModifiers(HotkeyModifiers modifiers)
    {
        uint result = 0;
        if (modifiers.HasFlag(HotkeyModifiers.Alt))
            result |= MOD_ALT;
        if (modifiers.HasFlag(HotkeyModifiers.Control))
            result |= MOD_CONTROL;
        if (modifiers.HasFlag(HotkeyModifiers.Shift))
            result |= MOD_SHIFT;
        if (modifiers.HasFlag(HotkeyModifiers.Win))
            result |= MOD_WIN;
        return result;
    }

    private uint GetVirtualKeyCode(string key)
    {
        // Convert key string to virtual key code
        return key.ToUpperInvariant() switch
        {
            // Letters
            "A" => 0x41, "B" => 0x42, "C" => 0x43, "D" => 0x44, "E" => 0x45,
            "F" => 0x46, "G" => 0x47, "H" => 0x48, "I" => 0x49, "J" => 0x4A,
            "K" => 0x4B, "L" => 0x4C, "M" => 0x4D, "N" => 0x4E, "O" => 0x4F,
            "P" => 0x50, "Q" => 0x51, "R" => 0x52, "S" => 0x53, "T" => 0x54,
            "U" => 0x55, "V" => 0x56, "W" => 0x57, "X" => 0x58, "Y" => 0x59,
            "Z" => 0x5A,
            
            // Numbers
            "0" => 0x30, "1" => 0x31, "2" => 0x32, "3" => 0x33, "4" => 0x34,
            "5" => 0x35, "6" => 0x36, "7" => 0x37, "8" => 0x38, "9" => 0x39,
            
            // Function keys
            "F1" => 0x70, "F2" => 0x71, "F3" => 0x72, "F4" => 0x73, "F5" => 0x74,
            "F6" => 0x75, "F7" => 0x76, "F8" => 0x77, "F9" => 0x78, "F10" => 0x79,
            "F11" => 0x7A, "F12" => 0x7B,
            
            // NumPad
            "NUMPAD0" => 0x60, "NUMPAD1" => 0x61, "NUMPAD2" => 0x62, "NUMPAD3" => 0x63,
            "NUMPAD4" => 0x64, "NUMPAD5" => 0x65, "NUMPAD6" => 0x66, "NUMPAD7" => 0x67,
            "NUMPAD8" => 0x68, "NUMPAD9" => 0x69,
            "NUMPAD+" => 0x6B, "NUMPAD-" => 0x6D, "NUMPAD*" => 0x6A, "NUMPAD/" => 0x6F,
            
            // Special keys
            "SPACE" => 0x20,
            "ENTER" => 0x0D,
            "ESC" => 0x1B,
            "ESCAPE" => 0x1B,
            "TAB" => 0x09,
            "BACKSPACE" => 0x08,
            "DELETE" => 0x2E,
            "INSERT" => 0x2D,
            "HOME" => 0x24,
            "END" => 0x23,
            "PAGEUP" => 0x21,
            "PAGEDOWN" => 0x22,
            
            // Arrow keys
            "LEFT" => 0x25,
            "UP" => 0x26,
            "RIGHT" => 0x27,
            "DOWN" => 0x28,
            
            // Other
            "+" => 0xBB,
            "-" => 0xBD,
            "," => 0xBC,
            "." => 0xBE,
            
            _ => 0
        };
    }

    public void Dispose()
    {
        _isRunning = false;
        UnregisterAllHotkeys();

        if (_windowHandle != IntPtr.Zero)
        {
            DestroyWindow(_windowHandle);
            _windowHandle = IntPtr.Zero;
        }

        if (_messageLoopThread != null && _messageLoopThread.IsAlive)
        {
            _messageLoopThread.Join(1000);
        }
    }

    // Windows API constants
    private const uint MOD_ALT = 0x0001;
    private const uint MOD_CONTROL = 0x0002;
    private const uint MOD_SHIFT = 0x0004;
    private const uint MOD_WIN = 0x0008;
    private static readonly IntPtr HWND_MESSAGE = new IntPtr(-3);

    // Windows API imports
    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

    [DllImport("user32.dll")]
    private static extern IntPtr DefWindowProcW(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern ushort RegisterClassW([In] ref WNDCLASS lpWndClass);

    [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
    private static extern IntPtr CreateWindowExW(
        uint dwExStyle,
        string lpClassName,
        string lpWindowName,
        uint dwStyle,
        int x, int y, int nWidth, int nHeight,
        IntPtr hWndParent,
        IntPtr hMenu,
        IntPtr hInstance,
        IntPtr lpParam);

    [DllImport("user32.dll")]
    private static extern int GetMessage(out MSG lpMsg, IntPtr hWnd, uint wMsgFilterMin, uint wMsgFilterMax);

    [DllImport("user32.dll")]
    private static extern bool TranslateMessage([In] ref MSG lpMsg);

    [DllImport("user32.dll")]
    private static extern IntPtr DispatchMessage([In] ref MSG lpmsg);

    [DllImport("user32.dll")]
    private static extern bool DestroyWindow(IntPtr hWnd);

    // Structures
    [StructLayout(LayoutKind.Sequential)]
    private struct MSG
    {
        public IntPtr hwnd;
        public uint message;
        public IntPtr wParam;
        public IntPtr lParam;
        public uint time;
        public POINT pt;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct POINT
    {
        public int X;
        public int Y;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    private struct WNDCLASS
    {
        public uint style;
        public WndProcDelegate lpfnWndProc;
        public int cbClsExtra;
        public int cbWndExtra;
        public IntPtr hInstance;
        public IntPtr hIcon;
        public IntPtr hCursor;
        public IntPtr hbrBackground;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string? lpszMenuName;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string lpszClassName;
    }

    private delegate IntPtr WndProcDelegate(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);
}

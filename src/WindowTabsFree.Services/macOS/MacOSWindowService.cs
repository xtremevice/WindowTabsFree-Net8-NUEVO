using System.Runtime.InteropServices;
using System.Diagnostics;
using WindowTabsFree.Common.Interfaces;
using WindowTabsFree.Common.Models;

namespace WindowTabsFree.Services.macOS;

/// <summary>
/// macOS-specific window service using Cocoa/Accessibility APIs
/// </summary>
public class MacOSWindowService : IWindowService, IDisposable
{
    private bool _disposed = false;

    #region P/Invoke Declarations for macOS

    // Core Graphics - Window List
    [DllImport("/System/Library/Frameworks/CoreGraphics.framework/CoreGraphics")]
    private static extern IntPtr CGWindowListCopyWindowInfo(uint option, uint relativeToWindow);

    [DllImport("/System/Library/Frameworks/CoreFoundation.framework/CoreFoundation")]
    private static extern int CFArrayGetCount(IntPtr array);

    [DllImport("/System/Library/Frameworks/CoreFoundation.framework/CoreFoundation")]
    private static extern IntPtr CFArrayGetValueAtIndex(IntPtr array, int index);

    [DllImport("/System/Library/Frameworks/CoreFoundation.framework/CoreFoundation")]
    private static extern void CFRelease(IntPtr cf);

    [DllImport("/System/Library/Frameworks/CoreFoundation.framework/CoreFoundation")]
    private static extern IntPtr CFDictionaryGetValue(IntPtr dict, IntPtr key);

    [DllImport("/System/Library/Frameworks/CoreFoundation.framework/CoreFoundation")]
    private static extern IntPtr CFStringCreateWithCString(IntPtr allocator, string cStr, uint encoding);

    [DllImport("/System/Library/Frameworks/CoreFoundation.framework/CoreFoundation")]
    private static extern bool CFStringGetCString(IntPtr theString, byte[] buffer, int bufferSize, uint encoding);

    [DllImport("/System/Library/Frameworks/CoreFoundation.framework/CoreFoundation")]
    private static extern bool CFNumberGetValue(IntPtr number, int type, out int value);

    [DllImport("/System/Library/Frameworks/CoreFoundation.framework/CoreFoundation")]
    private static extern bool CFNumberGetValue(IntPtr number, int type, out long value);

    [DllImport("/System/Library/Frameworks/CoreFoundation.framework/CoreFoundation")]
    private static extern bool CFBooleanGetValue(IntPtr boolean);

    // libc - Process path
    [DllImport("libc")]
    private static extern int proc_pidpath(int pid, byte[] buffer, uint bufferSize);

    // AppKit - Application activation
    [DllImport("/System/Library/Frameworks/AppKit.framework/AppKit")]
    private static extern bool NSApplicationActivateIgnoringOtherApps(IntPtr app, bool flag);

    // Constants
    private const uint kCFStringEncodingUTF8 = 0x08000100;
    private const int kCFNumberIntType = 9;
    private const int kCFNumberLongType = 10;
    
    // CGWindowListOption values
    private const uint kCGWindowListOptionAll = 0;
    private const uint kCGWindowListOptionOnScreenOnly = 1;
    private const uint kCGWindowListExcludeDesktopElements = 16;

    #endregion

    /// <summary>
    /// Gets all windows from the system
    /// </summary>
    public IEnumerable<WindowInfo> GetAllWindows()
    {
        var windows = new List<WindowInfo>();

        try
        {
            // Get window list from Core Graphics
            uint options = kCGWindowListOptionOnScreenOnly | kCGWindowListExcludeDesktopElements;
            IntPtr windowList = CGWindowListCopyWindowInfo(options, 0);

            if (windowList == IntPtr.Zero)
            {
                return windows;
            }

            try
            {
                int count = CFArrayGetCount(windowList);

                for (int i = 0; i < count; i++)
                {
                    IntPtr windowDict = CFArrayGetValueAtIndex(windowList, i);
                    if (windowDict == IntPtr.Zero)
                        continue;

                    var windowInfo = ExtractWindowInfo(windowDict);
                    // Include window if it has a title OR a valid process name
                    // This ensures we detect apps like Terminal, Brave, GitHub Desktop, games, etc.
                    if (windowInfo != null && 
                        (!string.IsNullOrWhiteSpace(windowInfo.Title) || 
                         !string.IsNullOrWhiteSpace(windowInfo.ProcessName)))
                    {
                        windows.Add(windowInfo);
                        Debug.WriteLine($"macOS Window detected: ProcessName='{windowInfo.ProcessName}', Title='{windowInfo.Title}'");
                    }
                    else if (windowInfo != null)
                    {
                        Debug.WriteLine($"macOS Window filtered out: ProcessName='{windowInfo.ProcessName}', Title='{windowInfo.Title}'");
                    }
                }
            }
            finally
            {
                CFRelease(windowList);
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error enumerating macOS windows: {ex.Message}");
        }

        return windows;
    }

    /// <summary>
    /// Extracts window information from a CGWindow dictionary
    /// </summary>
    private WindowInfo? ExtractWindowInfo(IntPtr windowDict)
    {
        try
        {
            var windowInfo = new WindowInfo();

            // Get window number (ID)
            IntPtr windowNumberKey = CFStringCreateWithCString(IntPtr.Zero, "kCGWindowNumber", kCFStringEncodingUTF8);
            IntPtr windowNumberValue = CFDictionaryGetValue(windowDict, windowNumberKey);
            CFRelease(windowNumberKey);

            if (windowNumberValue != IntPtr.Zero)
            {
                CFNumberGetValue(windowNumberValue, kCFNumberIntType, out int windowId);
                windowInfo.Handle = new IntPtr(windowId);
            }

            // Get window name (title)
            IntPtr windowNameKey = CFStringCreateWithCString(IntPtr.Zero, "kCGWindowName", kCFStringEncodingUTF8);
            IntPtr windowNameValue = CFDictionaryGetValue(windowDict, windowNameKey);
            CFRelease(windowNameKey);

            if (windowNameValue != IntPtr.Zero)
            {
                byte[] buffer = new byte[1024];
                if (CFStringGetCString(windowNameValue, buffer, buffer.Length, kCFStringEncodingUTF8))
                {
                    int nullIndex = Array.IndexOf(buffer, (byte)0);
                    if (nullIndex >= 0)
                    {
                        windowInfo.Title = System.Text.Encoding.UTF8.GetString(buffer, 0, nullIndex);
                    }
                }
            }

            // Get owner name (application name)
            IntPtr ownerNameKey = CFStringCreateWithCString(IntPtr.Zero, "kCGWindowOwnerName", kCFStringEncodingUTF8);
            IntPtr ownerNameValue = CFDictionaryGetValue(windowDict, ownerNameKey);
            CFRelease(ownerNameKey);

            if (ownerNameValue != IntPtr.Zero)
            {
                byte[] buffer = new byte[256];
                if (CFStringGetCString(ownerNameValue, buffer, buffer.Length, kCFStringEncodingUTF8))
                {
                    int nullIndex = Array.IndexOf(buffer, (byte)0);
                    if (nullIndex >= 0)
                    {
                        windowInfo.ProcessName = System.Text.Encoding.UTF8.GetString(buffer, 0, nullIndex);
                        // Debug output to help identify system processes
                        if (!string.IsNullOrWhiteSpace(windowInfo.ProcessName))
                        {
                            System.Diagnostics.Debug.WriteLine($"[macOS] Detected process: '{windowInfo.ProcessName}'");
                        }
                    }
                }
            }

            // Get owner PID (process ID)
            IntPtr ownerPIDKey = CFStringCreateWithCString(IntPtr.Zero, "kCGWindowOwnerPID", kCFStringEncodingUTF8);
            IntPtr ownerPIDValue = CFDictionaryGetValue(windowDict, ownerPIDKey);
            CFRelease(ownerPIDKey);

            if (ownerPIDValue != IntPtr.Zero)
            {
                CFNumberGetValue(ownerPIDValue, kCFNumberIntType, out int pid);
                windowInfo.ProcessId = pid;
                
                // Get process path from PID
                if (pid > 0)
                {
                    byte[] pathBuffer = new byte[4096];
                    int ret = proc_pidpath(pid, pathBuffer, (uint)pathBuffer.Length);
                    if (ret > 0)
                    {
                        int nullIndex = Array.IndexOf(pathBuffer, (byte)0);
                        if (nullIndex > 0)
                        {
                            windowInfo.ProcessPath = System.Text.Encoding.UTF8.GetString(pathBuffer, 0, nullIndex);
                            System.Diagnostics.Debug.WriteLine($"[macOS] Process path: '{windowInfo.ProcessPath}'");
                        }
                    }
                }
            }

            // Get window layer
            IntPtr layerKey = CFStringCreateWithCString(IntPtr.Zero, "kCGWindowLayer", kCFStringEncodingUTF8);
            IntPtr layerValue = CFDictionaryGetValue(windowDict, layerKey);
            CFRelease(layerKey);

            int layer = 0;
            if (layerValue != IntPtr.Zero)
            {
                CFNumberGetValue(layerValue, kCFNumberIntType, out layer);
            }

            // Get window bounds
            IntPtr boundsKey = CFStringCreateWithCString(IntPtr.Zero, "kCGWindowBounds", kCFStringEncodingUTF8);
            IntPtr boundsValue = CFDictionaryGetValue(windowDict, boundsKey);
            CFRelease(boundsKey);

            if (boundsValue != IntPtr.Zero)
            {
                // Get X
                IntPtr xKey = CFStringCreateWithCString(IntPtr.Zero, "X", kCFStringEncodingUTF8);
                IntPtr xValue = CFDictionaryGetValue(boundsValue, xKey);
                CFRelease(xKey);
                if (xValue != IntPtr.Zero)
                {
                    CFNumberGetValue(xValue, kCFNumberIntType, out int x);
                    windowInfo.Bounds.X = x;
                }

                // Get Y
                IntPtr yKey = CFStringCreateWithCString(IntPtr.Zero, "Y", kCFStringEncodingUTF8);
                IntPtr yValue = CFDictionaryGetValue(boundsValue, yKey);
                CFRelease(yKey);
                if (yValue != IntPtr.Zero)
                {
                    CFNumberGetValue(yValue, kCFNumberIntType, out int y);
                    windowInfo.Bounds.Y = y;
                }

                // Get Width
                IntPtr widthKey = CFStringCreateWithCString(IntPtr.Zero, "Width", kCFStringEncodingUTF8);
                IntPtr widthValue = CFDictionaryGetValue(boundsValue, widthKey);
                CFRelease(widthKey);
                if (widthValue != IntPtr.Zero)
                {
                    CFNumberGetValue(widthValue, kCFNumberIntType, out int width);
                    windowInfo.Bounds.Width = width;
                }

                // Get Height
                IntPtr heightKey = CFStringCreateWithCString(IntPtr.Zero, "Height", kCFStringEncodingUTF8);
                IntPtr heightValue = CFDictionaryGetValue(boundsValue, heightKey);
                CFRelease(heightKey);
                if (heightValue != IntPtr.Zero)
                {
                    CFNumberGetValue(heightValue, kCFNumberIntType, out int height);
                    windowInfo.Bounds.Height = height;
                }
            }

            // Get on-screen status
            IntPtr isOnscreenKey = CFStringCreateWithCString(IntPtr.Zero, "kCGWindowIsOnscreen", kCFStringEncodingUTF8);
            IntPtr isOnscreenValue = CFDictionaryGetValue(windowDict, isOnscreenKey);
            CFRelease(isOnscreenKey);

            if (isOnscreenValue != IntPtr.Zero)
            {
                windowInfo.IsVisible = CFBooleanGetValue(isOnscreenValue);
            }

            // Layer 0 = normal windows, other layers are usually special windows
            windowInfo.IsVisible = windowInfo.IsVisible && layer == 0;

            return windowInfo;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error extracting window info: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// Gets windows in z-order (top to bottom)
    /// </summary>
    public IEnumerable<WindowInfo> GetWindowsInZOrder()
    {
        // On macOS, CGWindowListCopyWindowInfo already returns windows in z-order
        return GetAllWindows();
    }

    /// <summary>
    /// Sets focus to a window
    /// </summary>
    public void SetFocus(IntPtr handle)
    {
        try
        {
            // Get the window info to find the PID
            var windowInfo = GetWindowInfo(handle);
            if (windowInfo != null && windowInfo.ProcessId > 0)
            {
                // Use AppleScript as a more reliable method for focusing windows
                string script = $@"
                    tell application ""System Events""
                        set frontmost of first process whose unix id is {windowInfo.ProcessId} to true
                    end tell
                ";
                
                ExecuteAppleScript(script);
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error focusing window: {ex.Message}");
        }
    }

    /// <summary>
    /// Minimizes a window
    /// </summary>
    public void Minimize(IntPtr handle)
    {
        try
        {
            var windowInfo = GetWindowInfo(handle);
            if (windowInfo != null && windowInfo.ProcessId > 0 && !string.IsNullOrEmpty(windowInfo.Title))
            {
                string script = $@"
                    tell application ""System Events""
                        tell process ""{EscapeForAppleScript(windowInfo.ProcessName)}""
                            set windowName to ""{EscapeForAppleScript(windowInfo.Title)}""
                            set value of attribute ""AXMinimized"" of window windowName to true
                        end tell
                    end tell
                ";
                
                ExecuteAppleScript(script);
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error minimizing window: {ex.Message}");
        }
    }

    /// <summary>
    /// Maximizes a window (zoom on macOS)
    /// </summary>
    public void Maximize(IntPtr handle)
    {
        try
        {
            var windowInfo = GetWindowInfo(handle);
            if (windowInfo != null && windowInfo.ProcessId > 0 && !string.IsNullOrEmpty(windowInfo.Title))
            {
                // On macOS, we use "zoom" which is similar to maximize
                string script = $@"
                    tell application ""System Events""
                        tell process ""{EscapeForAppleScript(windowInfo.ProcessName)}""
                            set windowName to ""{EscapeForAppleScript(windowInfo.Title)}""
                            tell window windowName
                                set value of attribute ""AXFullScreen"" to true
                            end tell
                        end tell
                    end tell
                ";
                
                ExecuteAppleScript(script);
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error maximizing window: {ex.Message}");
        }
    }

    /// <summary>
    /// Restores a window to normal size
    /// </summary>
    public void Restore(IntPtr handle)
    {
        try
        {
            var windowInfo = GetWindowInfo(handle);
            if (windowInfo != null && windowInfo.ProcessId > 0 && !string.IsNullOrEmpty(windowInfo.Title))
            {
                string script = $@"
                    tell application ""System Events""
                        tell process ""{EscapeForAppleScript(windowInfo.ProcessName)}""
                            set windowName to ""{EscapeForAppleScript(windowInfo.Title)}""
                            tell window windowName
                                if value of attribute ""AXMinimized"" is true then
                                    set value of attribute ""AXMinimized"" to false
                                end if
                                if value of attribute ""AXFullScreen"" is true then
                                    set value of attribute ""AXFullScreen"" to false
                                end if
                            end tell
                        end tell
                    end tell
                ";
                
                ExecuteAppleScript(script);
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error restoring window: {ex.Message}");
        }
    }

    /// <summary>
    /// Closes a window
    /// </summary>
    public void Close(IntPtr handle)
    {
        try
        {
            var windowInfo = GetWindowInfo(handle);
            if (windowInfo != null && windowInfo.ProcessId > 0 && !string.IsNullOrEmpty(windowInfo.Title))
            {
                string script = $@"
                    tell application ""System Events""
                        tell process ""{EscapeForAppleScript(windowInfo.ProcessName)}""
                            set windowName to ""{EscapeForAppleScript(windowInfo.Title)}""
                            click button 1 of window windowName
                        end tell
                    end tell
                ";
                
                ExecuteAppleScript(script);
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error closing window: {ex.Message}");
        }
    }

    /// <summary>
    /// Gets window information by handle
    /// </summary>
    public WindowInfo? GetWindowInfo(IntPtr handle)
    {
        try
        {
            int windowId = handle.ToInt32();
            var allWindows = GetAllWindows();
            
            return allWindows.FirstOrDefault(w => w.Handle.ToInt32() == windowId);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error getting window info: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// Checks if a window is valid
    /// </summary>
    public bool IsWindowValid(IntPtr handle)
    {
        return GetWindowInfo(handle) != null;
    }

    /// <summary>
    /// Executes an AppleScript
    /// </summary>
    private void ExecuteAppleScript(string script)
    {
        try
        {
            var psi = new ProcessStartInfo
            {
                FileName = "/usr/bin/osascript",
                Arguments = $"-e \"{script.Replace("\"", "\\\"")}\"",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            using var process = Process.Start(psi);
            if (process != null)
            {
                process.WaitForExit(5000); // 5 second timeout
                
                if (process.ExitCode != 0)
                {
                    string error = process.StandardError.ReadToEnd();
                    Debug.WriteLine($"AppleScript error: {error}");
                }
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error executing AppleScript: {ex.Message}");
            // Silently fail - window operations are best-effort on macOS
        }
    }

    /// <summary>
    /// Escapes strings for AppleScript
    /// </summary>
    private string EscapeForAppleScript(string text)
    {
        if (string.IsNullOrEmpty(text))
            return "";
        
        return text.Replace("\\", "\\\\")
                   .Replace("\"", "\\\"")
                   .Replace("\n", "\\n")
                   .Replace("\r", "\\r");
    }

    /// <summary>
    /// Disposes resources
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Disposes resources
    /// </summary>
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                // Cleanup managed resources if any
            }
            
            _disposed = true;
        }
    }

    /// <summary>
    /// Finalizer
    /// </summary>
    ~MacOSWindowService()
    {
        Dispose(false);
    }

    public IntPtr GetForegroundWindow()
    {
        // TODO: Implement for macOS
        // Would need to use NSWorkspace.SharedWorkspace.FrontmostApplication
        return IntPtr.Zero;
    }
}

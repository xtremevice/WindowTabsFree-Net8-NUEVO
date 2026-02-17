using System.Runtime.InteropServices;
using System.Text;
using WindowTabsFree.Common.Interfaces;
using WindowTabsFree.Common.Models;

namespace WindowTabsFree.Services.Windows;

/// <summary>
/// Windows-specific window service using P/Invoke
/// </summary>
public class WindowsWindowService : IWindowService
{
    private const int SW_MINIMIZE = 6;
    private const int SW_MAXIMIZE = 3;
    private const int SW_RESTORE = 9;

    #region P/Invoke Declarations

    [DllImport("user32.dll")]
    private static extern bool EnumWindows(EnumWindowsProc enumProc, IntPtr lParam);

    [DllImport("user32.dll")]
    private static extern bool IsWindowVisible(IntPtr hWnd);

    [DllImport("user32.dll")]
    private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

    [DllImport("user32.dll")]
    private static extern int GetWindowTextLength(IntPtr hWnd);

    [DllImport("user32.dll")]
    private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

    [DllImport("user32.dll")]
    private static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

    [DllImport("user32.dll")]
    private static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

    [DllImport("user32.dll")]
    private static extern bool IsIconic(IntPtr hWnd);

    [DllImport("user32.dll")]
    private static extern bool IsZoomed(IntPtr hWnd);

    [DllImport("user32.dll")]
    private static extern bool SetForegroundWindow(IntPtr hWnd);

    [DllImport("user32.dll")]
    private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

    [DllImport("user32.dll")]
    private static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll")]
    private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

    [DllImport("user32.dll")]
    private static extern bool IsWindow(IntPtr hWnd);

    private const uint WM_CLOSE = 0x0010;

    [StructLayout(LayoutKind.Sequential)]
    private struct RECT
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
    }

    private delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

    #endregion

    public IEnumerable<WindowInfo> GetAllWindows()
    {
        var windows = new List<WindowInfo>();

        EnumWindows((hWnd, lParam) =>
        {
            if (IsWindowVisible(hWnd))
            {
                var windowInfo = GetWindowInfo(hWnd);
                if (windowInfo != null && !string.IsNullOrWhiteSpace(windowInfo.Title))
                {
                    windows.Add(windowInfo);
                }
            }
            return true;
        }, IntPtr.Zero);

        return windows;
    }

    public IEnumerable<WindowInfo> GetWindowsInZOrder()
    {
        // For now, return in enumeration order (which is roughly z-order)
        // A more sophisticated implementation would track actual z-order
        return GetAllWindows();
    }

    public void SetFocus(IntPtr handle)
    {
        if (!IsWindow(handle))
            return;

        SetForegroundWindow(handle);
        if (IsIconic(handle))
        {
            ShowWindow(handle, SW_RESTORE);
        }
    }

    public void Minimize(IntPtr handle)
    {
        if (!IsWindow(handle))
            return;

        ShowWindow(handle, SW_MINIMIZE);
    }

    public void Maximize(IntPtr handle)
    {
        if (!IsWindow(handle))
            return;

        ShowWindow(handle, SW_MAXIMIZE);
    }

    public void Restore(IntPtr handle)
    {
        if (!IsWindow(handle))
            return;

        ShowWindow(handle, SW_RESTORE);
    }

    public void Close(IntPtr handle)
    {
        if (!IsWindow(handle))
            return;

        SendMessage(handle, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
    }

    public WindowInfo? GetWindowInfo(IntPtr handle)
    {
        if (!IsWindow(handle))
            return null;

        var title = GetWindowTitle(handle);
        var className = GetWindowClassName(handle);
        
        GetWindowThreadProcessId(handle, out uint processId);
        
        string processName = string.Empty;
        try
        {
            var process = System.Diagnostics.Process.GetProcessById((int)processId);
            processName = process.ProcessName;
        }
        catch
        {
            // Process might have exited
        }

        GetWindowRect(handle, out RECT rect);

        return new WindowInfo
        {
            Handle = handle,
            Title = title,
            ProcessId = (int)processId,
            ProcessName = processName,
            ClassName = className,
            IsVisible = IsWindowVisible(handle),
            IsMinimized = IsIconic(handle),
            IsMaximized = IsZoomed(handle),
            Bounds = new WindowBounds
            {
                X = rect.Left,
                Y = rect.Top,
                Width = rect.Right - rect.Left,
                Height = rect.Bottom - rect.Top
            }
        };
    }

    public bool IsWindowValid(IntPtr handle)
    {
        return IsWindow(handle);
    }

    private string GetWindowTitle(IntPtr hWnd)
    {
        int length = GetWindowTextLength(hWnd);
        if (length == 0)
            return string.Empty;

        var builder = new StringBuilder(length + 1);
        GetWindowText(hWnd, builder, builder.Capacity);
        return builder.ToString();
    }

    private string GetWindowClassName(IntPtr hWnd)
    {
        var builder = new StringBuilder(256);
        GetClassName(hWnd, builder, builder.Capacity);
        return builder.ToString();
    }

    IntPtr IWindowService.GetForegroundWindow()
    {
        return GetForegroundWindow();
    }
}

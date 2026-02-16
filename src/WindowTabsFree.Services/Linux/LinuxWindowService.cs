using System.Runtime.InteropServices;
using System.Text;
using WindowTabsFree.Common.Interfaces;
using WindowTabsFree.Common.Models;

namespace WindowTabsFree.Services.Linux;

/// <summary>
/// Linux-specific window service using X11
/// </summary>
public class LinuxWindowService : IWindowService, IDisposable
{
    // X11 _NET_WM_STATE action constants
    private const int NET_WM_STATE_REMOVE = 0;
    private const int NET_WM_STATE_ADD = 1;
    private const int NET_WM_STATE_TOGGLE = 2;
    
    // X11 client message data capacity
    private const int CLIENT_MESSAGE_DATA_SIZE = 5;

    private IntPtr _display = IntPtr.Zero;
    private IntPtr _rootWindow = IntPtr.Zero;
    private bool _disposed = false;

    public LinuxWindowService()
    {
        try
        {
            // Try to open X11 display
            _display = XOpenDisplay(null);
            if (_display != IntPtr.Zero)
            {
                _rootWindow = XDefaultRootWindow(_display);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Warning: Could not initialize X11 display: {ex.Message}");
            Console.WriteLine("Window enumeration will not be available.");
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (_display != IntPtr.Zero)
            {
                try
                {
                    XCloseDisplay(_display);
                    _display = IntPtr.Zero;
                }
                catch (Exception ex)
                {
                    // Suppress exceptions during disposal to prevent process termination
                    System.Diagnostics.Debug.WriteLine($"Exception in LinuxWindowService disposal: {ex.Message}");
                }
            }
            _disposed = true;
        }
    }

    ~LinuxWindowService()
    {
        Dispose(false);
    }

    public IEnumerable<WindowInfo> GetAllWindows()
    {
        if (_display == IntPtr.Zero)
        {
            Console.WriteLine("X11 display not available");
            return new List<WindowInfo>();
        }

        var windows = new List<WindowInfo>();

        try
        {
            // Get all client windows
            var clientWindows = GetClientWindows(_rootWindow);
            
            foreach (var windowHandle in clientWindows)
            {
                var windowInfo = GetWindowInfo(windowHandle);
                // Include visible windows even without title, but must have a valid process
                if (windowInfo != null && windowInfo.IsVisible && 
                    (!string.IsNullOrWhiteSpace(windowInfo.Title) || 
                     !string.IsNullOrWhiteSpace(windowInfo.ProcessName)))
                {
                    windows.Add(windowInfo);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error enumerating windows: {ex.Message}");
        }

        return windows;
    }

    public IEnumerable<WindowInfo> GetWindowsInZOrder()
    {
        // X11 doesn't provide direct z-order, so we return enumeration order
        return GetAllWindows();
    }

    public void SetFocus(IntPtr handle)
    {
        if (_display == IntPtr.Zero || handle == IntPtr.Zero)
            return;

        try
        {
            XRaiseWindow(_display, handle);
            XSetInputFocus(_display, handle, 1, 0);
            XFlush(_display);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error setting focus: {ex.Message}");
        }
    }

    public void Minimize(IntPtr handle)
    {
        if (_display == IntPtr.Zero || handle == IntPtr.Zero)
            return;

        try
        {
            XIconifyWindow(_display, handle, 0);
            XFlush(_display);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error minimizing window: {ex.Message}");
        }
    }

    public void Maximize(IntPtr handle)
    {
        if (_display == IntPtr.Zero || handle == IntPtr.Zero)
            return;

        try
        {
            // Send _NET_WM_STATE event to maximize
            SendWMStateEvent(handle, NET_WM_STATE_ADD, "_NET_WM_STATE_MAXIMIZED_HORZ", "_NET_WM_STATE_MAXIMIZED_VERT");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error maximizing window: {ex.Message}");
        }
    }

    public void Restore(IntPtr handle)
    {
        if (_display == IntPtr.Zero || handle == IntPtr.Zero)
            return;

        try
        {
            // Remove maximized state
            SendWMStateEvent(handle, NET_WM_STATE_REMOVE, "_NET_WM_STATE_MAXIMIZED_HORZ", "_NET_WM_STATE_MAXIMIZED_VERT");
            // Map the window if it was minimized
            XMapWindow(_display, handle);
            XFlush(_display);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error restoring window: {ex.Message}");
        }
    }

    public void Close(IntPtr handle)
    {
        if (_display == IntPtr.Zero || handle == IntPtr.Zero)
            return;

        try
        {
            // Send WM_DELETE_WINDOW message
            SendClientMessage(handle, "WM_PROTOCOLS", "WM_DELETE_WINDOW");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error closing window: {ex.Message}");
        }
    }

    public WindowInfo? GetWindowInfo(IntPtr handle)
    {
        if (_display == IntPtr.Zero || handle == IntPtr.Zero)
            return null;

        try
        {
            var title = GetWindowProperty(handle, "_NET_WM_NAME") ?? GetWindowProperty(handle, "WM_NAME") ?? string.Empty;
            var className = GetWindowClass(handle);
            var pid = GetWindowPid(handle);
            
            string processName = string.Empty;
            if (pid > 0)
            {
                try
                {
                    var process = System.Diagnostics.Process.GetProcessById(pid);
                    processName = process.ProcessName;
                }
                catch (ArgumentException)
                {
                    // Process with given PID doesn't exist or has exited
                }
                catch (InvalidOperationException)
                {
                    // Process has exited
                }
                catch (System.ComponentModel.Win32Exception)
                {
                    // Access denied to process information
                }
            }

            // Get window attributes
            XGetWindowAttributes(_display, handle, out XWindowAttributes attrs);

            return new WindowInfo
            {
                Handle = handle,
                Title = title,
                ProcessId = pid,
                ProcessName = processName,
                ClassName = className,
                IsVisible = attrs.map_state == 2, // IsViewable
                IsMinimized = GetWindowState(handle, "_NET_WM_STATE_HIDDEN"),
                IsMaximized = GetWindowState(handle, "_NET_WM_STATE_MAXIMIZED_HORZ") || 
                             GetWindowState(handle, "_NET_WM_STATE_MAXIMIZED_VERT"),
                Bounds = new WindowBounds
                {
                    X = attrs.x,
                    Y = attrs.y,
                    Width = attrs.width,
                    Height = attrs.height
                }
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting window info: {ex.Message}");
            return null;
        }
    }

    public bool IsWindowValid(IntPtr handle)
    {
        if (_display == IntPtr.Zero || handle == IntPtr.Zero)
            return false;

        try
        {
            XGetWindowAttributes(_display, handle, out XWindowAttributes attrs);
            return attrs.map_state > 0;
        }
        catch
        {
            return false;
        }
    }

    #region Helper Methods

    private List<IntPtr> GetClientWindows(IntPtr window)
    {
        var windows = new List<IntPtr>();

        if (XQueryTree(_display, window, out IntPtr root, out IntPtr parent, out IntPtr children, out uint nChildren) != 0)
        {
            try
            {
                for (int i = 0; i < (int)nChildren; i++)
                {
                    IntPtr child = Marshal.ReadIntPtr(children, i * IntPtr.Size);
                    
                    // Check if this is a client window
                    XGetWindowAttributes(_display, child, out XWindowAttributes attrs);
                    if (attrs.map_state == 2) // IsViewable
                    {
                        windows.Add(child);
                    }

                    // Recursively get children
                    windows.AddRange(GetClientWindows(child));
                }
            }
            finally
            {
                if (children != IntPtr.Zero)
                    XFree(children);
            }
        }

        return windows;
    }

    private string? GetWindowProperty(IntPtr window, string propertyName)
    {
        try
        {
            IntPtr propertyAtom = XInternAtom(_display, propertyName, false);
            if (propertyAtom == IntPtr.Zero)
                return null;

            if (XGetWindowProperty(_display, window, propertyAtom, 0, 1024, false, 
                IntPtr.Zero, out IntPtr actualType, out int actualFormat, out ulong nItems, 
                out ulong bytesAfter, out IntPtr prop) == 0 && prop != IntPtr.Zero)
            {
                try
                {
                    if (actualFormat == 8) // String
                    {
                        return Marshal.PtrToStringAnsi(prop);
                    }
                }
                finally
                {
                    XFree(prop);
                }
            }
        }
        catch (Exception ex) when (ex is DllNotFoundException or EntryPointNotFoundException or SEHException)
        {
            // X11 library not available or X11 property doesn't exist - safe to ignore
            System.Diagnostics.Debug.WriteLine($"X11 property access failed: {ex.Message}");
        }

        return null;
    }

    private string GetWindowClass(IntPtr window)
    {
        try
        {
            if (XGetClassHint(_display, window, out XClassHint classHint) != 0)
            {
                try
                {
                    if (classHint.res_class != IntPtr.Zero)
                        return Marshal.PtrToStringAnsi(classHint.res_class) ?? string.Empty;
                }
                finally
                {
                    if (classHint.res_name != IntPtr.Zero)
                        XFree(classHint.res_name);
                    if (classHint.res_class != IntPtr.Zero)
                        XFree(classHint.res_class);
                }
            }
        }
        catch (Exception ex) when (ex is DllNotFoundException or EntryPointNotFoundException or SEHException)
        {
            // XGetClassHint may fail if window doesn't support this hint or X11 not available
            System.Diagnostics.Debug.WriteLine($"XGetClassHint failed: {ex.Message}");
        }

        return string.Empty;
    }

    private int GetWindowPid(IntPtr window)
    {
        try
        {
            IntPtr pidAtom = XInternAtom(_display, "_NET_WM_PID", false);
            if (pidAtom != IntPtr.Zero)
            {
                if (XGetWindowProperty(_display, window, pidAtom, 0, 1, false,
                    IntPtr.Zero, out IntPtr actualType, out int actualFormat, out ulong nItems,
                    out ulong bytesAfter, out IntPtr prop) == 0 && prop != IntPtr.Zero)
                {
                    try
                    {
                        if (actualFormat == 32 && nItems > 0)
                        {
                            return Marshal.ReadInt32(prop);
                        }
                    }
                    finally
                    {
                        XFree(prop);
                    }
                }
            }
        }
        catch (Exception ex) when (ex is DllNotFoundException or EntryPointNotFoundException or SEHException)
        {
            // _NET_WM_PID property might not be set or X11 not available
            System.Diagnostics.Debug.WriteLine($"_NET_WM_PID access failed: {ex.Message}");
        }

        return 0;
    }

    private bool GetWindowState(IntPtr window, string stateName)
    {
        try
        {
            IntPtr stateAtom = XInternAtom(_display, "_NET_WM_STATE", false);
            IntPtr targetAtom = XInternAtom(_display, stateName, false);

            if (stateAtom != IntPtr.Zero && targetAtom != IntPtr.Zero)
            {
                if (XGetWindowProperty(_display, window, stateAtom, 0, 1024, false,
                    IntPtr.Zero, out IntPtr actualType, out int actualFormat, out ulong nItems,
                    out ulong bytesAfter, out IntPtr prop) == 0 && prop != IntPtr.Zero)
                {
                    try
                    {
                        for (ulong i = 0; i < nItems; i++)
                        {
                            IntPtr atom = Marshal.ReadIntPtr(prop, (int)i * IntPtr.Size);
                            if (atom == targetAtom)
                                return true;
                        }
                    }
                    finally
                    {
                        XFree(prop);
                    }
                }
            }
        }
        catch (Exception ex) when (ex is DllNotFoundException or EntryPointNotFoundException or SEHException)
        {
            // _NET_WM_STATE property might not exist or X11 not available
            System.Diagnostics.Debug.WriteLine($"_NET_WM_STATE access failed: {ex.Message}");
        }

        return false;
    }

    private void SendWMStateEvent(IntPtr window, int action, string state1, string state2)
    {
        IntPtr stateAtom = XInternAtom(_display, "_NET_WM_STATE", false);
        IntPtr atom1 = XInternAtom(_display, state1, false);
        IntPtr atom2 = XInternAtom(_display, state2, false);

        XEvent xev = new XEvent
        {
            type = 33, // ClientMessage
            xclient = new XClientMessageEvent
            {
                type = 33,
                window = window,
                message_type = stateAtom,
                format = 32,
                data = new XClientMessageEvent.DataUnion
                {
                    l = new long[CLIENT_MESSAGE_DATA_SIZE]
                }
            }
        };

        xev.xclient.data.l[0] = action;
        xev.xclient.data.l[1] = atom1.ToInt64();
        xev.xclient.data.l[2] = atom2.ToInt64();
        xev.xclient.data.l[3] = 1; // Source indication: 1 = application

        XSendEvent(_display, _rootWindow, false, 
            (IntPtr)(1L << 20 | 1L << 19), ref xev); // SubstructureRedirectMask | SubstructureNotifyMask
        XFlush(_display);
    }

    private void SendClientMessage(IntPtr window, string messageType, string message)
    {
        IntPtr messageTypeAtom = XInternAtom(_display, messageType, false);
        IntPtr messageAtom = XInternAtom(_display, message, false);

        XEvent xev = new XEvent
        {
            type = 33, // ClientMessage
            xclient = new XClientMessageEvent
            {
                type = 33,
                window = window,
                message_type = messageTypeAtom,
                format = 32,
                data = new XClientMessageEvent.DataUnion
                {
                    l = new long[CLIENT_MESSAGE_DATA_SIZE]
                }
            }
        };

        xev.xclient.data.l[0] = messageAtom.ToInt64();

        XSendEvent(_display, window, false, IntPtr.Zero, ref xev);
        XFlush(_display);
    }

    #endregion

    #region X11 P/Invoke Declarations

    [DllImport("libX11.so.6")]
    private static extern IntPtr XOpenDisplay(string? display);

    [DllImport("libX11.so.6")]
    private static extern int XCloseDisplay(IntPtr display);

    [DllImport("libX11.so.6")]
    private static extern IntPtr XDefaultRootWindow(IntPtr display);

    [DllImport("libX11.so.6")]
    private static extern int XQueryTree(IntPtr display, IntPtr window, out IntPtr root, 
        out IntPtr parent, out IntPtr children, out uint nChildren);

    [DllImport("libX11.so.6")]
    private static extern int XFree(IntPtr data);

    [DllImport("libX11.so.6")]
    private static extern int XGetWindowAttributes(IntPtr display, IntPtr window, 
        out XWindowAttributes attributes);

    [DllImport("libX11.so.6")]
    private static extern IntPtr XInternAtom(IntPtr display, string atom_name, bool only_if_exists);

    [DllImport("libX11.so.6")]
    private static extern int XGetWindowProperty(IntPtr display, IntPtr window, IntPtr property,
        long long_offset, long long_length, bool delete, IntPtr req_type, out IntPtr actual_type_return,
        out int actual_format_return, out ulong nitems_return, out ulong bytes_after_return,
        out IntPtr prop_return);

    [DllImport("libX11.so.6")]
    private static extern int XGetClassHint(IntPtr display, IntPtr window, out XClassHint class_hint);

    [DllImport("libX11.so.6")]
    private static extern int XRaiseWindow(IntPtr display, IntPtr window);

    [DllImport("libX11.so.6")]
    private static extern int XSetInputFocus(IntPtr display, IntPtr window, int revert_to, long time);

    [DllImport("libX11.so.6")]
    private static extern int XFlush(IntPtr display);

    [DllImport("libX11.so.6")]
    private static extern int XIconifyWindow(IntPtr display, IntPtr window, int screen);

    [DllImport("libX11.so.6")]
    private static extern int XMapWindow(IntPtr display, IntPtr window);

    [DllImport("libX11.so.6")]
    private static extern int XSendEvent(IntPtr display, IntPtr window, bool propagate, 
        IntPtr event_mask, ref XEvent event_send);

    [StructLayout(LayoutKind.Sequential)]
    private struct XWindowAttributes
    {
        public int x, y;
        public int width, height;
        public int border_width;
        public int depth;
        public IntPtr visual;
        public IntPtr root;
        public int c_class;
        public int bit_gravity;
        public int win_gravity;
        public int backing_store;
        public ulong backing_planes;
        public ulong backing_pixel;
        public int save_under;
        public IntPtr colormap;
        public int map_installed;
        public int map_state;
        public long all_event_masks;
        public long your_event_mask;
        public long do_not_propagate_mask;
        public int override_redirect;
        public IntPtr screen;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct XClassHint
    {
        public IntPtr res_name;
        public IntPtr res_class;
    }

    [StructLayout(LayoutKind.Explicit, Size = 192)]
    private struct XEvent
    {
        [FieldOffset(0)]
        public int type;
        [FieldOffset(0)]
        public XClientMessageEvent xclient;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct XClientMessageEvent
    {
        public int type;
        public ulong serial;
        public int send_event;
        public IntPtr display;
        public IntPtr window;
        public IntPtr message_type;
        public int format;
        public DataUnion data;

        [StructLayout(LayoutKind.Explicit)]
        public struct DataUnion
        {
            [FieldOffset(0)]
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
            public byte[] b;
            [FieldOffset(0)]
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            public short[] s;
            [FieldOffset(0)]
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
            public long[] l;
        }
    }

    #endregion
}

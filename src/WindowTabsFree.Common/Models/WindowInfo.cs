namespace WindowTabsFree.Common.Models;

/// <summary>
/// Represents information about a system window
/// </summary>
public class WindowInfo
{
    /// <summary>
    /// Window handle (platform-specific)
    /// </summary>
    public IntPtr Handle { get; set; }

    /// <summary>
    /// Window title
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Process ID owning this window
    /// </summary>
    public int ProcessId { get; set; }

    /// <summary>
    /// Process name
    /// </summary>
    public string ProcessName { get; set; } = string.Empty;

    /// <summary>
    /// Window class name
    /// </summary>
    public string ClassName { get; set; } = string.Empty;

    /// <summary>
    /// Whether the window is visible
    /// </summary>
    public bool IsVisible { get; set; }

    /// <summary>
    /// Whether the window is minimized
    /// </summary>
    public bool IsMinimized { get; set; }

    /// <summary>
    /// Whether the window is maximized
    /// </summary>
    public bool IsMaximized { get; set; }

    /// <summary>
    /// Window bounds (X, Y, Width, Height)
    /// </summary>
    public WindowBounds Bounds { get; set; } = new();

    /// <summary>
    /// Count of windows from the same application (calculated at runtime)
    /// </summary>
    public int ApplicationWindowCount { get; set; }

    /// <summary>
    /// Whether auto-grouping is enabled for this application (calculated at runtime)
    /// </summary>
    public bool IsAutoGroupEnabled { get; set; }

    /// <summary>
    /// Whether this window is the active window in its group (calculated at runtime)
    /// </summary>
    public bool IsActiveInGroup { get; set; }

    /// <summary>
    /// Whether this window is NOT the active window in its group (for XAML binding)
    /// </summary>
    public bool IsInactive => !IsActiveInGroup;

    /// <summary>
    /// Display title - returns Title if available, otherwise ProcessName
    /// Useful for windows that don't have a title (Terminal, games, etc.)
    /// </summary>
    public string DisplayTitle => 
        !string.IsNullOrWhiteSpace(Title) ? Title : 
        !string.IsNullOrWhiteSpace(ProcessName) ? $"[{ProcessName}]" : 
        "[No Title]";
}

/// <summary>
/// Window position and size
/// </summary>
public class WindowBounds
{
    public int X { get; set; }
    public int Y { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
}

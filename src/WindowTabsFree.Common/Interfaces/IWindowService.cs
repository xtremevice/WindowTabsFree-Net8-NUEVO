using WindowTabsFree.Common.Models;

namespace WindowTabsFree.Common.Interfaces;

/// <summary>
/// Service for interacting with system windows
/// </summary>
public interface IWindowService
{
    /// <summary>
    /// Gets all windows from the system
    /// </summary>
    /// <returns>List of windows</returns>
    IEnumerable<WindowInfo> GetAllWindows();

    /// <summary>
    /// Gets windows in z-order (top to bottom)
    /// </summary>
    /// <returns>List of windows in z-order</returns>
    IEnumerable<WindowInfo> GetWindowsInZOrder();

    /// <summary>
    /// Sets focus to a window
    /// </summary>
    /// <param name="handle">Window handle</param>
    void SetFocus(IntPtr handle);

    /// <summary>
    /// Minimizes a window
    /// </summary>
    /// <param name="handle">Window handle</param>
    void Minimize(IntPtr handle);

    /// <summary>
    /// Maximizes a window
    /// </summary>
    /// <param name="handle">Window handle</param>
    void Maximize(IntPtr handle);

    /// <summary>
    /// Restores a window
    /// </summary>
    /// <param name="handle">Window handle</param>
    void Restore(IntPtr handle);

    /// <summary>
    /// Closes a window
    /// </summary>
    /// <param name="handle">Window handle</param>
    void Close(IntPtr handle);

    /// <summary>
    /// Gets window information
    /// </summary>
    /// <param name="handle">Window handle</param>
    /// <returns>Window information or null if not found</returns>
    WindowInfo? GetWindowInfo(IntPtr handle);

    /// <summary>
    /// Checks if a window is valid
    /// </summary>
    /// <param name="handle">Window handle</param>
    /// <returns>True if valid, false otherwise</returns>
    bool IsWindowValid(IntPtr handle);

    /// <summary>
    /// Gets the currently focused (foreground) window handle
    /// </summary>
    /// <returns>Handle of the foreground window</returns>
    IntPtr GetForegroundWindow();
}

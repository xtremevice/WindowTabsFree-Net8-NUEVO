namespace WindowTabsFree.Common.Interfaces;

/// <summary>
/// Service for system tray icon management
/// </summary>
public interface ISystemTrayService
{
    /// <summary>
    /// Shows the system tray icon
    /// </summary>
    void Show();

    /// <summary>
    /// Hides the system tray icon
    /// </summary>
    void Hide();

    /// <summary>
    /// Updates the system tray icon tooltip
    /// </summary>
    /// <param name="tooltip">Tooltip text</param>
    void UpdateTooltip(string tooltip);

    /// <summary>
    /// Event fired when the tray icon is clicked
    /// </summary>
    event EventHandler? TrayIconClicked;

    /// <summary>
    /// Event fired when exit is requested from tray
    /// </summary>
    event EventHandler? ExitRequested;
}

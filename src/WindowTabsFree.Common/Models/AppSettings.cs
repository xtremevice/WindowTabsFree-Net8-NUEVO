namespace WindowTabsFree.Common.Models;

/// <summary>
/// Application configuration settings
/// </summary>
public class AppSettings
{
    /// <summary>
    /// Tab configuration
    /// </summary>
    public TabSettings TabSettings { get; set; } = new();

    /// <summary>
    /// Whether to start with Windows
    /// </summary>
    public bool StartWithWindows { get; set; }

    /// <summary>
    /// Whether to show in system tray
    /// </summary>
    public bool ShowInSystemTray { get; set; } = true;

    /// <summary>
    /// Whether to minimize to tray
    /// </summary>
    public bool MinimizeToTray { get; set; } = true;

    /// <summary>
    /// List of excluded applications (process names)
    /// </summary>
    public List<string> ExcludedApplications { get; set; } = new();

    /// <summary>
    /// List of excluded application paths (for path-based filtering, e.g., /System/Library/)
    /// </summary>
    public List<string> ExcludedApplicationPaths { get; set; } = new();

    /// <summary>
    /// Hotkeys configuration
    /// </summary>
    public HotKeysSettings HotKeys { get; set; } = new();

    /// <summary>
    /// Tab groups for organizing windows
    /// </summary>
    public List<TabGroup> TabGroups { get; set; } = new();

    /// <summary>
    /// Auto-group settings for specific applications
    /// </summary>
    public List<ApplicationGroupSetting> ApplicationGroupSettings { get; set; } = new();

    /// <summary>
    /// Whether the window should stay on top of other windows
    /// </summary>
    public bool TopmostEnabled { get; set; } = false;
}

/// <summary>
/// Tab display and behavior settings
/// </summary>
public class TabSettings
{
    /// <summary>
    /// Tab height in pixels
    /// </summary>
    public int TabHeight { get; set; } = 30;

    /// <summary>
    /// Whether to show close button on tabs
    /// </summary>
    public bool ShowCloseButton { get; set; } = true;

    /// <summary>
    /// Whether to show icons on tabs
    /// </summary>
    public bool ShowIcons { get; set; } = true;

    /// <summary>
    /// Maximum tab width in pixels
    /// </summary>
    public int MaxTabWidth { get; set; } = 200;

    /// <summary>
    /// Minimum tab width in pixels
    /// </summary>
    public int MinTabWidth { get; set; } = 100;
}

/// <summary>
/// Hotkey settings
/// </summary>
public class HotKeysSettings
{
    /// <summary>
    /// Hotkey to show/hide manager window
    /// </summary>
    public string? ToggleManagerWindow { get; set; }

    /// <summary>
    /// Hotkey to switch to next tab
    /// </summary>
    public string? NextTab { get; set; }

    /// <summary>
    /// Hotkey to switch to previous tab
    /// </summary>
    public string? PreviousTab { get; set; }

    /// <summary>
    /// Hotkey to switch to next window (across all windows)
    /// </summary>
    public string? NextWindow { get; set; }

    /// <summary>
    /// Hotkey to switch to previous window (across all windows)
    /// </summary>
    public string? PreviousWindow { get; set; }

    /// <summary>
    /// Hotkey to focus window 1
    /// </summary>
    public string? FocusWindow1 { get; set; }

    /// <summary>
    /// Hotkey to focus window 2
    /// </summary>
    public string? FocusWindow2 { get; set; }

    /// <summary>
    /// Hotkey to focus window 3
    /// </summary>
    public string? FocusWindow3 { get; set; }

    /// <summary>
    /// Hotkey to focus window 4
    /// </summary>
    public string? FocusWindow4 { get; set; }

    /// <summary>
    /// Hotkey to focus window 5
    /// </summary>
    public string? FocusWindow5 { get; set; }
}

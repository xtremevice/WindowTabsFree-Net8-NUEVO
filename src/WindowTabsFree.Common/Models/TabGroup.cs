namespace WindowTabsFree.Common.Models;

/// <summary>
/// Represents a group of windows organized in a tab
/// </summary>
public class TabGroup
{
    /// <summary>
    /// Unique identifier for the tab group
    /// </summary>
    public string Id { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Display name of the tab group
    /// </summary>
    public string Name { get; set; } = "New Group";

    /// <summary>
    /// List of window handles in this group
    /// </summary>
    public List<IntPtr> WindowHandles { get; set; } = new();

    /// <summary>
    /// Index of the currently active window in this group
    /// </summary>
    public int ActiveWindowIndex { get; set; } = 0;

    /// <summary>
    /// Whether this group was created automatically by grouping same applications
    /// </summary>
    public bool IsAutoGrouped { get; set; } = false;

    /// <summary>
    /// Application name for auto-grouped windows (ProcessName)
    /// </summary>
    public string? ApplicationName { get; set; }

    /// <summary>
    /// Color for the tab (optional, for visual distinction)
    /// </summary>
    public string? Color { get; set; }

    /// <summary>
    /// Order/position of this tab group
    /// </summary>
    public int Order { get; set; }

    /// <summary>
    /// Whether this tab group is currently active
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Creation timestamp
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Last modified timestamp
    /// </summary>
    public DateTime LastModifiedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// List of window information for display (not persisted, populated at runtime)
    /// </summary>
    [System.Text.Json.Serialization.JsonIgnore]
    public List<WindowInfo>? WindowsInfo { get; set; }
}

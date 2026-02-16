namespace WindowTabsFree.Common.Models;

/// <summary>
/// Represents auto-group settings for a specific application
/// </summary>
public class ApplicationGroupSetting
{
    /// <summary>
    /// Process name of the application
    /// </summary>
    public string ProcessName { get; set; } = string.Empty;

    /// <summary>
    /// Whether auto-grouping is enabled for this application
    /// </summary>
    public bool IsAutoGroupEnabled { get; set; }

    /// <summary>
    /// When this setting was last modified
    /// </summary>
    public DateTime LastModifiedAt { get; set; } = DateTime.UtcNow;
}

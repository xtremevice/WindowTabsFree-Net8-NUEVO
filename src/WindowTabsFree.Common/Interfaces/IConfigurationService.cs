using WindowTabsFree.Common.Models;

namespace WindowTabsFree.Common.Interfaces;

/// <summary>
/// Service for managing application configuration
/// </summary>
public interface IConfigurationService
{
    /// <summary>
    /// Loads settings from storage
    /// </summary>
    /// <returns>Application settings</returns>
    AppSettings LoadSettings();

    /// <summary>
    /// Saves settings to storage
    /// </summary>
    /// <param name="settings">Settings to save</param>
    void SaveSettings(AppSettings settings);

    /// <summary>
    /// Gets the current settings
    /// </summary>
    AppSettings Settings { get; }
}

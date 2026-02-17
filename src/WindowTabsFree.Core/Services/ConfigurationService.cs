using Newtonsoft.Json;
using WindowTabsFree.Common.Interfaces;
using WindowTabsFree.Common.Models;

namespace WindowTabsFree.Core.Services;

/// <summary>
/// Configuration service using JSON file storage
/// </summary>
public class ConfigurationService : IConfigurationService
{
    private readonly string _configFilePath;
    private AppSettings _settings;

    public ConfigurationService()
    {
        // Get user's application data folder
        var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var appFolder = Path.Combine(appDataPath, "WindowTabsFree");
        
        // Create folder if it doesn't exist
        if (!Directory.Exists(appFolder))
        {
            Directory.CreateDirectory(appFolder);
        }

        _configFilePath = Path.Combine(appFolder, "settings.json");
        _settings = LoadSettings();
    }

    public AppSettings Settings => _settings;

    public AppSettings LoadSettings()
    {
        try
        {
            if (File.Exists(_configFilePath))
            {
                var json = File.ReadAllText(_configFilePath);
                var settings = JsonConvert.DeserializeObject<AppSettings>(json);
                _settings = settings ?? CreateDefaultSettings();
            }
            else
            {
                _settings = CreateDefaultSettings();
                SaveSettings(_settings);
            }
        }
        catch (Exception ex)
        {
            // Log error and return defaults
            Console.WriteLine($"Error loading settings: {ex.Message}");
            _settings = CreateDefaultSettings();
        }

        return _settings;
    }

    public void SaveSettings(AppSettings settings)
    {
        try
        {
            _settings = settings;
            var json = JsonConvert.SerializeObject(settings, Formatting.Indented);
            File.WriteAllText(_configFilePath, json);
        }
        catch (Exception ex)
        {
            // Log error
            Console.WriteLine($"Error saving settings: {ex.Message}");
        }
    }

    private static AppSettings CreateDefaultSettings()
    {
        return new AppSettings
        {
            TabSettings = new TabSettings
            {
                TabHeight = 30,
                ShowCloseButton = true,
                ShowIcons = true,
                MaxTabWidth = 200,
                MinTabWidth = 100
            },
            StartWithWindows = false,
            ShowInSystemTray = true,
            MinimizeToTray = true,
            ExcludedApplications = new List<string>
            {
                // Windows system processes
                "explorer",
                "taskmgr",
                "dwm",
                "SearchUI",
                "SearchApp",
                "ShellExperienceHost",
                "ApplicationFrameHost",
                "TextInputHost",
                "LockApp",
                "StartMenuExperienceHost",
                "SystemSettings",
                
                // macOS system processes
                "Dock",
                "Control Center",
                "ControlCenter",
                "NotificationCenter",
                "Notification Center",
                "SystemUIServer",
                "WindowServer",
                "loginwindow",
                "CoreServicesUIAgent",
                "UserEventAgent",
                "Problem Reporter",
                "ProblemReporter",
                "Spotlight",
                
                // Linux system processes
                "gnome-shell",
                "plasmashell",
                "xfce4-panel",
                "lxpanel",
                "mate-panel",
                "cinnamon"
            },
            HotKeys = new HotKeysSettings
            {
                ToggleManagerWindow = "Ctrl+Alt+T",
                NextTab = "Ctrl+Tab",
                PreviousTab = "Ctrl+Shift+Tab"
            }
        };
    }
}

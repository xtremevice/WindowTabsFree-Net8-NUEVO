using WindowTabsFree.Common.Interfaces;
using WindowTabsFree.Common.Models;

namespace WindowTabsFree.Core.Services;

/// <summary>
/// Service for managing windows and tabs
/// </summary>
public class WindowManagerService
{
    private readonly IWindowService _windowService;
    private readonly IConfigurationService _configurationService;

    public WindowManagerService(IWindowService windowService, IConfigurationService configurationService)
    {
        _windowService = windowService;
        _configurationService = configurationService;
    }

    /// <summary>
    /// Gets all manageable windows (excluding system windows and configured exclusions)
    /// </summary>
    public IEnumerable<WindowInfo> GetManageableWindows()
    {
        var allWindows = _windowService.GetAllWindows();
        var excludedApps = _configurationService.Settings.ExcludedApplications;

        // Include windows that have either a title OR a process name (but not both empty)
        // This allows detection of Terminal, Brave, GitHub Desktop, games, etc.
        return allWindows.Where(w => 
            (!string.IsNullOrWhiteSpace(w.Title) || !string.IsNullOrWhiteSpace(w.ProcessName)) &&
            !excludedApps.Contains(w.ProcessName, StringComparer.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Focuses a window by handle
    /// </summary>
    public void FocusWindow(IntPtr handle)
    {
        _windowService.SetFocus(handle);
    }

    /// <summary>
    /// Closes a window by handle
    /// </summary>
    public void CloseWindow(IntPtr handle)
    {
        _windowService.Close(handle);
    }

    /// <summary>
    /// Minimizes a window by handle
    /// </summary>
    public void MinimizeWindow(IntPtr handle)
    {
        _windowService.Minimize(handle);
    }

    /// <summary>
    /// Maximizes a window by handle
    /// </summary>
    public void MaximizeWindow(IntPtr handle)
    {
        _windowService.Maximize(handle);
    }

    /// <summary>
    /// Restores a window by handle
    /// </summary>
    public void RestoreWindow(IntPtr handle)
    {
        _windowService.Restore(handle);
    }

    /// <summary>
    /// Gets all tab groups
    /// </summary>
    public IEnumerable<TabGroup> GetTabGroups()
    {
        return _configurationService.Settings.TabGroups;
    }

    /// <summary>
    /// Creates a new tab group
    /// </summary>
    public TabGroup CreateTabGroup(string name)
    {
        var group = new TabGroup
        {
            Name = name,
            Order = _configurationService.Settings.TabGroups.Count
        };

        _configurationService.Settings.TabGroups.Add(group);
        _configurationService.SaveSettings(_configurationService.Settings);

        return group;
    }

    /// <summary>
    /// Deletes a tab group
    /// </summary>
    public void DeleteTabGroup(string groupId)
    {
        var group = _configurationService.Settings.TabGroups.FirstOrDefault(g => g.Id == groupId);
        if (group != null)
        {
            _configurationService.Settings.TabGroups.Remove(group);
            _configurationService.SaveSettings(_configurationService.Settings);
        }
    }

    /// <summary>
    /// Adds a window to a tab group
    /// </summary>
    public void AddWindowToGroup(string groupId, IntPtr windowHandle)
    {
        var group = _configurationService.Settings.TabGroups.FirstOrDefault(g => g.Id == groupId);
        if (group != null && !group.WindowHandles.Contains(windowHandle))
        {
            group.WindowHandles.Add(windowHandle);
            group.LastModifiedAt = DateTime.UtcNow;
            _configurationService.SaveSettings(_configurationService.Settings);
        }
    }

    /// <summary>
    /// Removes a window from a tab group
    /// </summary>
    public void RemoveWindowFromGroup(string groupId, IntPtr windowHandle)
    {
        var group = _configurationService.Settings.TabGroups.FirstOrDefault(g => g.Id == groupId);
        if (group != null)
        {
            group.WindowHandles.Remove(windowHandle);
            group.LastModifiedAt = DateTime.UtcNow;
            _configurationService.SaveSettings(_configurationService.Settings);
        }
    }

    /// <summary>
    /// Sets the active tab group
    /// </summary>
    public void SetActiveTabGroup(string groupId)
    {
        foreach (var group in _configurationService.Settings.TabGroups)
        {
            group.IsActive = group.Id == groupId;
        }
        _configurationService.SaveSettings(_configurationService.Settings);
    }

    /// <summary>
    /// Auto-groups windows by application name (ProcessName)
    /// Creates a tab group for each application with multiple windows
    /// Automatically enables auto-group for grouped applications
    /// </summary>
    public List<TabGroup> AutoGroupByApplication()
    {
        var windows = GetManageableWindows().ToList();
        var newGroups = new List<TabGroup>();
        var autoGroupSettings = _configurationService.Settings.ApplicationGroupSettings;
        
        // Group windows by process name - group ALL apps with multiple windows
        var windowsByApp = windows
            .Where(w => !string.IsNullOrEmpty(w.ProcessName))
            .GroupBy(w => w.ProcessName)
            .Where(g => g.Count() > 1) // Only group apps with multiple windows
            .ToList();

        foreach (var appGroup in windowsByApp)
        {
            var appName = appGroup.Key;
            
            // Enable auto-group for this application
            var setting = autoGroupSettings.FirstOrDefault(s => s.ProcessName == appName);
            if (setting == null)
            {
                setting = new ApplicationGroupSetting
                {
                    ProcessName = appName,
                    IsAutoGroupEnabled = true
                };
                autoGroupSettings.Add(setting);
            }
            else
            {
                setting.IsAutoGroupEnabled = true;
            }
            setting.LastModifiedAt = DateTime.UtcNow;
            
            // Check if an auto-group already exists for this app
            var existingGroup = _configurationService.Settings.TabGroups
                .FirstOrDefault(g => g.IsAutoGrouped && g.ApplicationName == appName);

            if (existingGroup != null)
            {
                // Update existing auto-group
                existingGroup.WindowHandles.Clear();
                foreach (var window in appGroup)
                {
                    existingGroup.WindowHandles.Add(window.Handle);
                }
                existingGroup.LastModifiedAt = DateTime.UtcNow;
            }
            else
            {
                // Create new auto-group
                var group = new TabGroup
                {
                    Name = $"{appName} Windows",
                    ApplicationName = appName,
                    IsAutoGrouped = true,
                    WindowHandles = appGroup.Select(w => w.Handle).ToList(),
                    Order = _configurationService.Settings.TabGroups.Count,
                    Color = GenerateColorForApp(appName)
                };
                
                _configurationService.Settings.TabGroups.Add(group);
                newGroups.Add(group);
            }
        }

        _configurationService.SaveSettings(_configurationService.Settings);
        return newGroups;
    }

    /// <summary>
    /// Removes all auto-grouped tab groups
    /// </summary>
    public void RemoveAutoGroups()
    {
        _configurationService.Settings.TabGroups.RemoveAll(g => g.IsAutoGrouped);
        _configurationService.SaveSettings(_configurationService.Settings);
    }

    /// <summary>
    /// Activates the next window in a tab group
    /// </summary>
    public void ActivateNextWindowInGroup(string groupId)
    {
        var group = _configurationService.Settings.TabGroups.FirstOrDefault(g => g.Id == groupId);
        if (group != null && group.WindowHandles.Count > 0)
        {
            group.ActiveWindowIndex = (group.ActiveWindowIndex + 1) % group.WindowHandles.Count;
            group.LastModifiedAt = DateTime.UtcNow;
            
            var activeHandle = group.WindowHandles[group.ActiveWindowIndex];
            _windowService.SetFocus(activeHandle);
            
            _configurationService.SaveSettings(_configurationService.Settings);
        }
    }

    /// <summary>
    /// Activates the previous window in a tab group
    /// </summary>
    public void ActivatePreviousWindowInGroup(string groupId)
    {
        var group = _configurationService.Settings.TabGroups.FirstOrDefault(g => g.Id == groupId);
        if (group != null && group.WindowHandles.Count > 0)
        {
            group.ActiveWindowIndex = (group.ActiveWindowIndex - 1 + group.WindowHandles.Count) % group.WindowHandles.Count;
            group.LastModifiedAt = DateTime.UtcNow;
            
            var activeHandle = group.WindowHandles[group.ActiveWindowIndex];
            _windowService.SetFocus(activeHandle);
            
            _configurationService.SaveSettings(_configurationService.Settings);
        }
    }

    /// <summary>
    /// Gets the active window handle in a group
    /// </summary>
    public IntPtr? GetActiveWindowInGroup(string groupId)
    {
        var group = _configurationService.Settings.TabGroups.FirstOrDefault(g => g.Id == groupId);
        if (group != null && group.WindowHandles.Count > 0)
        {
            var index = Math.Min(group.ActiveWindowIndex, group.WindowHandles.Count - 1);
            return group.WindowHandles[index];
        }
        return null;
    }

    /// <summary>
    /// Generates a color for an application (for visual distinction)
    /// </summary>
    private string GenerateColorForApp(string appName)
    {
        // Simple hash-based color generation
        var hash = appName.GetHashCode();
        var colors = new[] { "#FF6B6B", "#4ECDC4", "#45B7D1", "#FFA07A", "#98D8C8", "#F7DC6F", "#BB8FCE", "#85C1E2" };
        return colors[Math.Abs(hash) % colors.Length];
    }

    /// <summary>
    /// Toggles auto-group setting for a specific application
    /// </summary>
    public void ToggleAutoGroupForApplication(string processName, bool enabled)
    {
        var setting = _configurationService.Settings.ApplicationGroupSettings
            .FirstOrDefault(s => s.ProcessName == processName);

        if (setting == null)
        {
            // Create new setting
            setting = new ApplicationGroupSetting
            {
                ProcessName = processName,
                IsAutoGroupEnabled = enabled
            };
            _configurationService.Settings.ApplicationGroupSettings.Add(setting);
        }
        else
        {
            // Update existing setting
            setting.IsAutoGroupEnabled = enabled;
            setting.LastModifiedAt = DateTime.UtcNow;
        }

        _configurationService.SaveSettings(_configurationService.Settings);

        // If enabled, create group for ONLY this specific app
        if (enabled)
        {
            AutoGroupSingleApplication(processName);
        }
        else
        {
            // If disabled, remove auto-groups for this app
            var groupsToRemove = _configurationService.Settings.TabGroups
                .Where(g => g.IsAutoGrouped && g.ApplicationName == processName)
                .ToList();
            
            foreach (var group in groupsToRemove)
            {
                _configurationService.Settings.TabGroups.Remove(group);
            }
            _configurationService.SaveSettings(_configurationService.Settings);
        }
    }

    /// <summary>
    /// Auto-groups windows for a single specific application
    /// </summary>
    private void AutoGroupSingleApplication(string processName)
    {
        var allWindows = _windowService.GetAllWindows();
        
        // Get windows for this specific application
        var appWindows = allWindows
            .Where(w => w.ProcessName == processName)
            .ToList();

        // Only group if there are multiple windows
        if (appWindows.Count < 2)
            return;

        // Check if group already exists
        var existingGroup = _configurationService.Settings.TabGroups
            .FirstOrDefault(g => g.IsAutoGrouped && g.ApplicationName == processName);

        if (existingGroup != null)
        {
            // Update existing group
            existingGroup.WindowHandles = appWindows.Select(w => w.Handle).ToList();
            existingGroup.ActiveWindowIndex = 0;
        }
        else
        {
            // Create new group
            var group = new TabGroup
            {
                Id = Guid.NewGuid().ToString(),
                Name = $"{processName} Windows",
                WindowHandles = appWindows.Select(w => w.Handle).ToList(),
                IsAutoGrouped = true,
                ApplicationName = processName,
                Color = GenerateColorForApp(processName),
                ActiveWindowIndex = 0
            };

            _configurationService.Settings.TabGroups.Add(group);
        }

        _configurationService.SaveSettings(_configurationService.Settings);
    }

    /// <summary>
    /// Checks if auto-group is enabled for a specific application
    /// </summary>
    public bool IsAutoGroupEnabledForApplication(string processName)
    {
        var setting = _configurationService.Settings.ApplicationGroupSettings
            .FirstOrDefault(s => s.ProcessName == processName);
        return setting?.IsAutoGroupEnabled ?? false;
    }

    /// <summary>
    /// Activates the next window globally (cycles through all manageable windows)
    /// </summary>
    public void ActivateNextWindowGlobally()
    {
        var windows = GetManageableWindows().ToList();
        if (windows.Count == 0)
            return;

        var currentWindow = _windowService.GetForegroundWindow();
        var currentIndex = windows.FindIndex(w => w.Handle == currentWindow);
        
        // Move to next window. If current window is not in list (currentIndex = -1) or is last, wrap to first
        var nextIndex = (currentIndex + 1) % windows.Count;
        
        _windowService.SetFocus(windows[nextIndex].Handle);
    }

    /// <summary>
    /// Activates the previous window globally (cycles through all manageable windows)
    /// </summary>
    public void ActivatePreviousWindowGlobally()
    {
        var windows = GetManageableWindows().ToList();
        if (windows.Count == 0)
            return;

        var currentWindow = _windowService.GetForegroundWindow();
        var currentIndex = windows.FindIndex(w => w.Handle == currentWindow);
        
        // Move to previous window. If current window is not in list (currentIndex = -1), wrap to last
        var prevIndex = currentIndex <= 0 ? windows.Count - 1 : currentIndex - 1;
        
        _windowService.SetFocus(windows[prevIndex].Handle);
    }

    /// <summary>
    /// Activates the next window in the current window's group, or cycles through all windows if not grouped
    /// </summary>
    public void ActivateNextWindowInCurrentGroup()
    {
        var currentWindow = _windowService.GetForegroundWindow();
        
        // Find which group contains the current window
        var currentGroup = _configurationService.Settings.TabGroups
            .FirstOrDefault(g => g.WindowHandles.Contains(currentWindow));
        
        if (currentGroup != null && currentGroup.WindowHandles.Count > 1)
        {
            // Current window is in a group, cycle within the group
            var currentIndex = currentGroup.WindowHandles.IndexOf(currentWindow);
            var nextIndex = (currentIndex + 1) % currentGroup.WindowHandles.Count;
            
            currentGroup.ActiveWindowIndex = nextIndex;
            currentGroup.LastModifiedAt = DateTime.UtcNow;
            
            _windowService.SetFocus(currentGroup.WindowHandles[nextIndex]);
            _configurationService.SaveSettings(_configurationService.Settings);
        }
        else
        {
            // Not in a group or group has only one window, use global navigation
            ActivateNextWindowGlobally();
        }
    }

    /// <summary>
    /// Activates the previous window in the current window's group, or cycles through all windows if not grouped
    /// </summary>
    public void ActivatePreviousWindowInCurrentGroup()
    {
        var currentWindow = _windowService.GetForegroundWindow();
        
        // Find which group contains the current window
        var currentGroup = _configurationService.Settings.TabGroups
            .FirstOrDefault(g => g.WindowHandles.Contains(currentWindow));
        
        if (currentGroup != null && currentGroup.WindowHandles.Count > 1)
        {
            // Current window is in a group, cycle within the group
            var currentIndex = currentGroup.WindowHandles.IndexOf(currentWindow);
            var prevIndex = (currentIndex - 1 + currentGroup.WindowHandles.Count) % currentGroup.WindowHandles.Count;
            
            currentGroup.ActiveWindowIndex = prevIndex;
            currentGroup.LastModifiedAt = DateTime.UtcNow;
            
            _windowService.SetFocus(currentGroup.WindowHandles[prevIndex]);
            _configurationService.SaveSettings(_configurationService.Settings);
        }
        else
        {
            // Not in a group or group has only one window, use global navigation
            ActivatePreviousWindowGlobally();
        }
    }
}

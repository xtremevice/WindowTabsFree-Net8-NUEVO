using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Timers;
using WindowTabsFree.Common.Interfaces;
using WindowTabsFree.Common.Models;
using WindowTabsFree.Core.Services;

namespace WindowTabsFree.UI.ViewModels;

public partial class MainWindowViewModel : ViewModelBase, IDisposable
{
    private const int RefreshIntervalMs = 10000; // Refresh window list every 10 seconds
    
    private readonly WindowManagerService _windowManager;
    private readonly System.Timers.Timer _refreshTimer;
    private ObservableCollection<WindowInfo> _windows = new();
    private ObservableCollection<TabGroup> _tabGroups = new();
    private ObservableCollection<TabGroup> _manualTabGroups = new();
    private TabGroup? _selectedTabGroup;
    private WindowInfo? _selectedWindow;

    public MainWindowViewModel()
    {
        // Initialize services
        var configService = new ConfigurationService();
        var windowService = WindowTabsFree.Services.WindowServiceFactory.Create();
        _windowManager = new WindowManagerService(windowService, configService);

        // Initialize timer for auto-refresh (every 2 seconds)
        _refreshTimer = new System.Timers.Timer(RefreshIntervalMs);
        _refreshTimer.Elapsed += OnTimerElapsed;
        _refreshTimer.AutoReset = true;
        _refreshTimer.Start();

        // Load windows and tab groups
        RefreshWindows();
        RefreshTabGroups();
    }

    public ObservableCollection<WindowInfo> Windows
    {
        get => _windows;
        set
        {
            _windows = value;
            OnPropertyChanged(nameof(Windows));
        }
    }

    public ObservableCollection<TabGroup> TabGroups
    {
        get => _tabGroups;
        set
        {
            _tabGroups = value;
            OnPropertyChanged(nameof(TabGroups));
            // Update cached manual groups
            UpdateManualTabGroups();
        }
    }

    public ObservableCollection<TabGroup> ManualTabGroups
    {
        get => _manualTabGroups;
        private set
        {
            _manualTabGroups = value;
            OnPropertyChanged(nameof(ManualTabGroups));
        }
    }

    private void UpdateManualTabGroups()
    {
        // Cache filtered manual groups to avoid recreating on every access
        var manualGroups = _tabGroups.Where(g => !g.IsAutoGrouped).ToList();
        ManualTabGroups = new ObservableCollection<TabGroup>(manualGroups);
    }

    public TabGroup? SelectedTabGroup
    {
        get => _selectedTabGroup;
        set
        {
            _selectedTabGroup = value;
            OnPropertyChanged(nameof(SelectedTabGroup));
            CanAddToGroup = value != null;
        }
    }

    public WindowInfo? SelectedWindow
    {
        get => _selectedWindow;
        set
        {
            _selectedWindow = value;
            OnPropertyChanged(nameof(SelectedWindow));
        }
    }

    private bool _canAddToGroup;

    public bool CanAddToGroup
    {
        get => _canAddToGroup;
        set
        {
            _canAddToGroup = value;
            OnPropertyChanged(nameof(CanAddToGroup));
        }
    }

    private bool _topmostEnabled;

    public bool TopmostEnabled
    {
        get => _topmostEnabled;
        set
        {
            _topmostEnabled = value;
            OnPropertyChanged(nameof(TopmostEnabled));
        }
    }

    private void OnTimerElapsed(object? sender, ElapsedEventArgs e)
    {
        try
        {
            RefreshWindows();
            RefreshTabGroups(); // Also refresh tab groups to update window titles
        }
        catch (Exception ex)
        {
            // Log error but keep timer running
            System.Diagnostics.Debug.WriteLine($"Error refreshing windows: {ex.Message}");
        }
    }

    public void RefreshWindows()
    {
        var windows = _windowManager.GetManageableWindows().ToList();
        
        // Calculate metadata for each window
        foreach (var window in windows)
        {
            // Count windows from the same application
            window.ApplicationWindowCount = windows.Count(w => w.ProcessName == window.ProcessName);
            
            // Check if auto-group is enabled for this application
            window.IsAutoGroupEnabled = _windowManager.IsAutoGroupEnabledForApplication(window.ProcessName);
        }
        
        // Sort alphabetically by DisplayTitle
        windows.Sort((a, b) => string.Compare(a.DisplayTitle, b.DisplayTitle, StringComparison.OrdinalIgnoreCase));
        
        // Update on UI thread
        Avalonia.Threading.Dispatcher.UIThread.Post(() =>
        {
            Windows = new ObservableCollection<WindowInfo>(windows);
        });
    }

    public void RefreshTabGroups()
    {
        var groups = _windowManager.GetTabGroups().ToList();
        var allWindows = _windowManager.GetManageableWindows().ToList();
        
        // Populate WindowsInfo for each group
        foreach (var group in groups)
        {
            group.WindowsInfo = new List<WindowInfo>();
            for (int i = 0; i < group.WindowHandles.Count; i++)
            {
                var handle = group.WindowHandles[i];
                var windowInfo = allWindows.FirstOrDefault(w => w.Handle == handle);
                if (windowInfo != null)
                {
                    // Mark the active window in the group
                    windowInfo.IsActiveInGroup = (i == group.ActiveWindowIndex);
                    group.WindowsInfo.Add(windowInfo);
                }
            }
        }
        
        // Update on UI thread to ensure window titles are refreshed in real-time
        Avalonia.Threading.Dispatcher.UIThread.Post(() =>
        {
            TabGroups = new ObservableCollection<TabGroup>(groups);
        });
    }

    public void FocusWindow(WindowInfo window)
    {
        _windowManager.FocusWindow(window.Handle);
    }

    public void CloseWindow(WindowInfo window)
    {
        _windowManager.CloseWindow(window.Handle);
        RefreshWindows();
    }

    public void MinimizeWindow(WindowInfo window)
    {
        _windowManager.MinimizeWindow(window.Handle);
    }

    public void MaximizeWindow(WindowInfo window)
    {
        _windowManager.MaximizeWindow(window.Handle);
    }

    public void RestoreWindow(WindowInfo window)
    {
        _windowManager.RestoreWindow(window.Handle);
    }

    public void CreateNewTabGroup(string groupName)
    {
        _windowManager.CreateTabGroup(groupName);
        RefreshTabGroups();
    }

    public void DeleteTabGroup(TabGroup group)
    {
        _windowManager.DeleteTabGroup(group.Id);
        RefreshTabGroups();
    }

    public void AddWindowToGroup(WindowInfo window, TabGroup group)
    {
        _windowManager.AddWindowToGroup(group.Id, window.Handle);
        RefreshTabGroups();
    }

    public void RemoveWindowFromGroup(WindowInfo window, TabGroup group)
    {
        _windowManager.RemoveWindowFromGroup(group.Id, window.Handle);
        RefreshTabGroups();
    }

    public void AutoGroupByApplication()
    {
        _windowManager.AutoGroupByApplication();
        RefreshTabGroups();
    }

    public void RemoveAutoGroups()
    {
        _windowManager.RemoveAutoGroups();
        RefreshTabGroups();
    }

    public void ActivateNextWindowInGroup(TabGroup group)
    {
        _windowManager.ActivateNextWindowInGroup(group.Id);
        RefreshTabGroups();
    }

    public void ActivatePreviousWindowInGroup(TabGroup group)
    {
        _windowManager.ActivatePreviousWindowInGroup(group.Id);
        RefreshTabGroups();
    }

    public void FocusWindowInGroup(TabGroup group, int windowIndex)
    {
        if (windowIndex >= 0 && windowIndex < group.WindowHandles.Count)
        {
            var handle = group.WindowHandles[windowIndex];
            _windowManager.FocusWindow(handle);
            group.ActiveWindowIndex = windowIndex;
            RefreshTabGroups();
        }
    }

    public void ToggleAutoGroupForApplication(string processName)
    {
        if (string.IsNullOrEmpty(processName))
            return;

        var currentSetting = _windowManager.IsAutoGroupEnabledForApplication(processName);
        _windowManager.ToggleAutoGroupForApplication(processName, !currentSetting);
        
        // Refresh to show updated state
        RefreshWindows();
        RefreshTabGroups();
    }

    public void AddSelectedWindowToSelectedGroup()
    {
        if (SelectedWindow != null && SelectedTabGroup != null)
        {
            _windowManager.AddWindowToGroup(SelectedTabGroup.Id, SelectedWindow.Handle);
            RefreshTabGroups();
        }
    }

    public void Dispose()
    {
        _refreshTimer?.Stop();
        _refreshTimer?.Dispose();
        GC.SuppressFinalize(this);
    }
}

using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Threading;
using System;
using System.Linq;
using WindowTabsFree.Common.Interfaces;
using WindowTabsFree.Common.Models;
using WindowTabsFree.Core.Services;

namespace WindowTabsFree.UI.Views
{
    public partial class FloatingWindowManager : Window
    {
        private readonly WindowManagerService _windowManager;
        private readonly IWindowService _windowService;
        private readonly IConfigurationService _configService;
        private DispatcherTimer? _refreshTimer;

        // Parameterless constructor for XAML (required but not used at runtime)
        public FloatingWindowManager() : this(null!, null!, null!)
        {
        }

        public FloatingWindowManager(WindowManagerService windowManager, IWindowService windowService, IConfigurationService configService)
        {
            InitializeComponent();
            _windowManager = windowManager;
            _windowService = windowService;
            _configService = configService;
            
            // Set up refresh timer
            _refreshTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(2)
            };
            _refreshTimer.Tick += RefreshTimer_Tick;
            _refreshTimer.Start();
            
            // Initial load
            RefreshGroups();
        }

        private void RefreshTimer_Tick(object? sender, EventArgs e)
        {
            RefreshGroups();
        }

        private void RefreshGroups()
        {
            try
            {
                var groups = _windowManager.GetTabGroups().ToList();
                
                // Populate WindowsInfo for each group
                foreach (var group in groups)
                {
                    var windows = group.WindowHandles
                        .Select(handle =>
                        {
                            try
                            {
                                return _windowService.GetAllWindows()
                                    .FirstOrDefault(w => w.Handle == handle);
                            }
                            catch
                            {
                                return null;
                            }
                        })
                        .Where(w => w != null)
                        .ToList();
                    
                    // Mark active window in group
                    for (int i = 0; i < windows.Count; i++)
                    {
                        if (windows[i] != null)
                        {
                            windows[i]!.IsActiveInGroup = (i == group.ActiveWindowIndex);
                        }
                    }
                    
                    group.WindowsInfo = windows!;
                }
                
                GroupsItemsControl.ItemsSource = groups;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error refreshing floating window groups: {ex.Message}");
            }
        }

        private void FocusThisTab_Click(object? sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is WindowInfo windowInfo)
            {
                _windowService.SetFocus(windowInfo.Handle);
            }
        }

        private void PinOnTopToggle_Click(object? sender, RoutedEventArgs e)
        {
            if (sender is Avalonia.Controls.Primitives.ToggleButton toggle)
            {
                Topmost = toggle.IsChecked ?? true;
            }
        }

        private void CloseWindow_Click(object? sender, RoutedEventArgs e)
        {
            Close();
        }

        protected override void OnClosed(EventArgs e)
        {
            _refreshTimer?.Stop();
            _refreshTimer = null;
            base.OnClosed(e);
        }
    }
}

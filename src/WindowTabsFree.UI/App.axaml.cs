using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using System.Linq;
using Avalonia.Markup.Xaml;
using WindowTabsFree.UI.ViewModels;
using WindowTabsFree.UI.Views;
using WindowTabsFree.Common.Interfaces;
using WindowTabsFree.Core.Services;
using System;

namespace WindowTabsFree.UI;

public partial class App : Application
{
    private IHotkeyService? _hotkeyService;
    private WindowManagerService? _windowManager;
    private IConfigurationService? _configService;
    
    // Make this public so MainWindow can re-register hotkeys
    public new static App? Current { get; private set; }

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
        Current = this;
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Avoid duplicate validations from both Avalonia and the CommunityToolkit. 
            // More info: https://docs.avaloniaui.net/docs/guides/development-guides/data-validation#manage-validationplugins
            DisableAvaloniaDataAnnotationValidation();
            
            // Check and request Accessibility permissions on macOS
            if (OperatingSystem.IsMacOS())
            {
                Console.WriteLine("[macOS] Checking Accessibility permissions for hotkeys...");
                bool hasPermissions = WindowTabsFree.Services.macOS.MacOSHotkeyService.CheckAndRequestAccessibilityPermissions();
                
                if (!hasPermissions)
                {
                    Console.WriteLine("[macOS] WARNING: Hotkeys will not work without Accessibility permissions");
                    Console.WriteLine("[macOS] Please grant permissions in System Settings and restart the app");
                }
            }
            
            // Initialize services
            _configService = new ConfigurationService();
            var windowService = WindowTabsFree.Services.WindowServiceFactory.Create();
            _windowManager = new WindowManagerService(windowService, _configService);
            
            // Initialize hotkey service
            _hotkeyService = WindowTabsFree.Services.HotkeyServiceFactory.Create();
            RegisterGlobalHotkeys();
            
            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainWindowViewModel(_windowManager),
            };

            // Cleanup on exit
            desktop.Exit += (s, e) =>
            {
                _hotkeyService?.Dispose();
            };
        }

        base.OnFrameworkInitializationCompleted();
    }

    private void RegisterGlobalHotkeys()
    {
        if (_hotkeyService == null || _configService == null || _windowManager == null)
            return;

        var settings = _configService.Settings;
        if (settings.HotKeys == null)
            return;

        // Unregister existing hotkeys first
        _hotkeyService.UnregisterAllHotkeys();

        // Register NextWindow hotkey (ID: 1)
        if (!string.IsNullOrWhiteSpace(settings.HotKeys.NextWindow))
        {
            Console.WriteLine($"[Hotkeys] Registering NextWindow hotkey: {settings.HotKeys.NextWindow}");
            _hotkeyService.RegisterHotkey(1, settings.HotKeys.NextWindow, () =>
            {
                try
                {
                    Console.WriteLine("[Hotkeys] NextWindow hotkey pressed - activating next window");
                    _windowManager.ActivateNextWindowInCurrentGroup();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error in NextWindow hotkey: {ex.Message}");
                }
            });
        }

        // Register PreviousWindow hotkey (ID: 2)
        if (!string.IsNullOrWhiteSpace(settings.HotKeys.PreviousWindow))
        {
            Console.WriteLine($"[Hotkeys] Registering PreviousWindow hotkey: {settings.HotKeys.PreviousWindow}");
            _hotkeyService.RegisterHotkey(2, settings.HotKeys.PreviousWindow, () =>
            {
                try
                {
                    Console.WriteLine("[Hotkeys] PreviousWindow hotkey pressed - activating previous window");
                    _windowManager.ActivatePreviousWindowInCurrentGroup();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error in PreviousWindow hotkey: {ex.Message}");
                }
            });
        }
    }

    // Public method to re-register hotkeys after configuration changes
    public void ReRegisterHotkeys()
    {
        RegisterGlobalHotkeys();
    }

    private void DisableAvaloniaDataAnnotationValidation()
    {
        // Get an array of plugins to remove
        var dataValidationPluginsToRemove =
            BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

        // remove each entry found
        foreach (var plugin in dataValidationPluginsToRemove)
        {
            BindingPlugins.DataValidators.Remove(plugin);
        }
    }
}
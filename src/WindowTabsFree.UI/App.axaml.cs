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

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Avoid duplicate validations from both Avalonia and the CommunityToolkit. 
            // More info: https://docs.avaloniaui.net/docs/guides/development-guides/data-validation#manage-validationplugins
            DisableAvaloniaDataAnnotationValidation();
            
            // Initialize services
            _configService = new ConfigurationService();
            var windowService = WindowTabsFree.Services.WindowServiceFactory.Create();
            _windowManager = new WindowManagerService(windowService, _configService);
            
            // Initialize hotkey service
            _hotkeyService = WindowTabsFree.Services.HotkeyServiceFactory.Create();
            RegisterGlobalHotkeys();
            
            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainWindowViewModel(),
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

        // Register NextWindow hotkey (ID: 1)
        if (!string.IsNullOrWhiteSpace(settings.HotKeys.NextWindow))
        {
            _hotkeyService.RegisterHotkey(1, settings.HotKeys.NextWindow, () =>
            {
                try
                {
                    _windowManager.ActivateNextWindowGlobally();
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
            _hotkeyService.RegisterHotkey(2, settings.HotKeys.PreviousWindow, () =>
            {
                try
                {
                    _windowManager.ActivatePreviousWindowGlobally();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error in PreviousWindow hotkey: {ex.Message}");
                }
            });
        }
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
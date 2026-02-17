using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using WindowTabsFree.Common.Models;
using WindowTabsFree.UI.ViewModels;
using WindowTabsFree.Core.Services;

namespace WindowTabsFree.UI.Views;

public partial class MainWindow : Window
{
    private TextBox? _capturingTextBox;
    private readonly HashSet<Key> _pressedKeys = new HashSet<Key>();
    private ConfigurationService? _configService;

    public MainWindow()
    {
        InitializeComponent();
        _configService = new ConfigurationService();
        LoadHotkeySettings();
        AttachHotkeyEvents();
    }

    private void LoadHotkeySettings()
    {
        if (_configService == null) return;
        
        var settings = _configService.Settings;
        if (settings.HotKeys != null)
        {
            NextWindowHotkeyTextBox.Text = settings.HotKeys.NextWindow ?? "";
            PreviousWindowHotkeyTextBox.Text = settings.HotKeys.PreviousWindow ?? "";
        }
    }

    private void AttachHotkeyEvents()
    {
        // Attach events to both textboxes
        NextWindowHotkeyTextBox.GotFocus += OnHotkeyTextBoxGotFocus;
        NextWindowHotkeyTextBox.LostFocus += OnHotkeyTextBoxLostFocus;
        NextWindowHotkeyTextBox.KeyDown += OnHotkeyTextBoxKeyDown;
        NextWindowHotkeyTextBox.KeyUp += OnHotkeyTextBoxKeyUp;
        
        PreviousWindowHotkeyTextBox.GotFocus += OnHotkeyTextBoxGotFocus;
        PreviousWindowHotkeyTextBox.LostFocus += OnHotkeyTextBoxLostFocus;
        PreviousWindowHotkeyTextBox.KeyDown += OnHotkeyTextBoxKeyDown;
        PreviousWindowHotkeyTextBox.KeyUp += OnHotkeyTextBoxKeyUp;
    }

    private void OnHotkeyTextBoxGotFocus(object? sender, GotFocusEventArgs e)
    {
        if (sender is TextBox textBox)
        {
            _capturingTextBox = textBox;
            _pressedKeys.Clear();
            textBox.Text = "";
            textBox.Watermark = "Press keys now...";
        }
    }

    private void OnHotkeyTextBoxLostFocus(object? sender, RoutedEventArgs e)
    {
        if (sender is TextBox textBox)
        {
            _capturingTextBox = null;
            _pressedKeys.Clear();
            
            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.Watermark = "Click and press keys...";
            }
        }
    }

    private void OnHotkeyTextBoxKeyDown(object? sender, KeyEventArgs e)
    {
        if (_capturingTextBox == null || sender != _capturingTextBox)
            return;

        e.Handled = true;
        
        // Only add if not already in set to avoid unnecessary updates
        if (_pressedKeys.Add(e.Key))
        {
            UpdateHotkeyText();
        }
    }

    private void OnHotkeyTextBoxKeyUp(object? sender, KeyEventArgs e)
    {
        if (_capturingTextBox == null)
            return;

        e.Handled = true;

        if (_pressedKeys.Count > 0)
        {
            _pressedKeys.Remove(e.Key);
        }
    }

    private void UpdateHotkeyText()
    {
        if (_capturingTextBox == null || _pressedKeys.Count == 0)
            return;

        var modifiers = new List<string>();
        var regularKeys = new List<string>();

        foreach (var key in _pressedKeys.OrderBy(k => GetKeyPriority(k)))
        {
            string keyName = GetKeyName(key);
            
            if (IsModifierKey(key))
            {
                if (!modifiers.Contains(keyName))
                    modifiers.Add(keyName);
            }
            else
            {
                if (!regularKeys.Contains(keyName))
                    regularKeys.Add(keyName);
            }
        }

        var hotkeyParts = new List<string>();
        hotkeyParts.AddRange(modifiers);
        hotkeyParts.AddRange(regularKeys);

        _capturingTextBox.Text = string.Join("+", hotkeyParts);
    }

    private bool IsModifierKey(Key key)
    {
        return key == Key.LeftCtrl || key == Key.RightCtrl ||
               key == Key.LeftAlt || key == Key.RightAlt ||
               key == Key.LeftShift || key == Key.RightShift ||
               key == Key.LWin || key == Key.RWin;
    }

    private int GetKeyPriority(Key key)
    {
        if (key == Key.LeftCtrl || key == Key.RightCtrl) return 0;
        if (key == Key.LeftAlt || key == Key.RightAlt) return 1;
        if (key == Key.LeftShift || key == Key.RightShift) return 2;
        if (key == Key.LWin || key == Key.RWin) return 3;
        return 4;
    }

    private string GetKeyName(Key key)
    {
        return key switch
        {
            Key.LeftCtrl or Key.RightCtrl => "Ctrl",
            Key.LeftAlt or Key.RightAlt => "Alt",
            Key.LeftShift or Key.RightShift => "Shift",
            Key.LWin or Key.RWin => "Win",
            Key.Left => "Left",
            Key.Right => "Right",
            Key.Up => "Up",
            Key.Down => "Down",
            _ => key.ToString()
        };
    }

    private void SaveHotkeysButton_Click(object? sender, RoutedEventArgs e)
    {
        if (_configService == null) return;
        
        var settings = _configService.Settings;
        if (settings.HotKeys == null)
        {
            settings.HotKeys = new HotKeysSettings();
        }
        
        // Validate and save hotkeys (empty values allowed - means no hotkey set)
        settings.HotKeys.NextWindow = string.IsNullOrWhiteSpace(NextWindowHotkeyTextBox.Text) 
            ? null : NextWindowHotkeyTextBox.Text.Trim();
        settings.HotKeys.PreviousWindow = string.IsNullOrWhiteSpace(PreviousWindowHotkeyTextBox.Text) 
            ? null : PreviousWindowHotkeyTextBox.Text.Trim();
        
        _configService.SaveSettings(settings);
        
        // Show success message
        var messageBox = new Window
        {
            Title = "Success",
            Width = 300,
            Height = 120,
            WindowStartupLocation = WindowStartupLocation.CenterOwner
        };
        var panel = new StackPanel { Margin = new Avalonia.Thickness(20) };
        panel.Children.Add(new TextBlock { Text = "✓ Hotkeys saved successfully!", FontSize = 14, Margin = new Avalonia.Thickness(0, 0, 0, 15) });
        var okButton = new Button { Content = "OK", Width = 80, HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center };
        okButton.Click += (s, args) => messageBox.Close();
        panel.Children.Add(okButton);
        messageBox.Content = panel;
        messageBox.ShowDialog(this);
    }

    protected override void OnClosed(EventArgs e)
    {
        // Cleanup timer when window closes
        if (DataContext is MainWindowViewModel vm)
        {
            vm.Dispose();
        }
        base.OnClosed(e);
    }

    private void RefreshButton_Click(object? sender, RoutedEventArgs e)
    {
        if (DataContext is MainWindowViewModel viewModel)
        {
            viewModel.RefreshWindows();
        }
    }

    private void FocusButton_Click(object? sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.Tag is WindowInfo window)
        {
            if (DataContext is MainWindowViewModel viewModel)
            {
                viewModel.FocusWindow(window);
            }
        }
    }

    private void CloseButton_Click(object? sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.Tag is WindowInfo window)
        {
            if (DataContext is MainWindowViewModel viewModel)
            {
                viewModel.CloseWindow(window);
            }
        }
    }

    private void MinimizeButton_Click(object? sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.Tag is WindowInfo window)
        {
            if (DataContext is MainWindowViewModel viewModel)
            {
                viewModel.MinimizeWindow(window);
            }
        }
    }

    private void MaximizeButton_Click(object? sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.Tag is WindowInfo window)
        {
            if (DataContext is MainWindowViewModel viewModel)
            {
                viewModel.MaximizeWindow(window);
            }
        }
    }

    private void RestoreButton_Click(object? sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.Tag is WindowInfo window)
        {
            if (DataContext is MainWindowViewModel viewModel)
            {
                viewModel.RestoreWindow(window);
            }
        }
    }

    private async void NewTabGroupButton_Click(object? sender, RoutedEventArgs e)
    {
        if (DataContext is MainWindowViewModel viewModel)
        {
            // Simple dialog for group name
            var dialog = new Window
            {
                Width = 400,
                Height = 150,
                Title = "Create New Tab Group",
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };

            var panel = new StackPanel { Margin = new Avalonia.Thickness(20) };
            var textBox = new TextBox 
            { 
                Watermark = "Enter group name...",
                Margin = new Avalonia.Thickness(0, 0, 0, 10)
            };
            var buttonPanel = new StackPanel 
            { 
                Orientation = Avalonia.Layout.Orientation.Horizontal,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Right,
                Spacing = 10
            };
            var okButton = new Button { Content = "Create", Width = 80 };
            var cancelButton = new Button { Content = "Cancel", Width = 80 };

            okButton.Click += (s, args) =>
            {
                if (!string.IsNullOrWhiteSpace(textBox.Text))
                {
                    viewModel.CreateNewTabGroup(textBox.Text);
                    dialog.Close();
                }
            };

            cancelButton.Click += (s, args) => dialog.Close();

            buttonPanel.Children.Add(okButton);
            buttonPanel.Children.Add(cancelButton);
            panel.Children.Add(new TextBlock { Text = "Group Name:", Margin = new Avalonia.Thickness(0, 0, 0, 5) });
            panel.Children.Add(textBox);
            panel.Children.Add(buttonPanel);

            dialog.Content = panel;
            await dialog.ShowDialog(this);
        }
    }

    private void DeleteTabGroupButton_Click(object? sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.Tag is TabGroup group)
        {
            if (DataContext is MainWindowViewModel viewModel)
            {
                viewModel.DeleteTabGroup(group);
            }
        }
    }

    private void AddToGroupButton_Click(object? sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.Tag is WindowInfo window)
        {
            if (DataContext is MainWindowViewModel viewModel && viewModel.SelectedTabGroup != null)
            {
                viewModel.AddWindowToGroup(window, viewModel.SelectedTabGroup);
            }
        }
    }



    private void NextTabButton_Click(object? sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.Tag is TabGroup group)
        {
            if (DataContext is MainWindowViewModel viewModel)
            {
                viewModel.ActivateNextWindowInGroup(group);
            }
        }
    }

    private void PrevTabButton_Click(object? sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.Tag is TabGroup group)
        {
            if (DataContext is MainWindowViewModel viewModel)
            {
                viewModel.ActivatePreviousWindowInGroup(group);
            }
        }
    }

    private void FocusTabButton_Click(object? sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.Tag is WindowInfo window)
        {
            if (DataContext is MainWindowViewModel viewModel)
            {
                viewModel.FocusWindow(window);
            }
        }
    }

    private void ToggleAutoGroupCheckBox_Click(object? sender, RoutedEventArgs e)
    {
        if (sender is CheckBox checkBox && checkBox.Tag is string processName)
        {
            if (DataContext is MainWindowViewModel viewModel)
            {
                viewModel.ToggleAutoGroupForApplication(processName);
            }
        }
    }

    private FloatingWindowManager? _floatingWindow;

    private void FloatingWindowButton_Click(object? sender, RoutedEventArgs e)
    {
        if (_floatingWindow == null || !_floatingWindow.IsVisible)
        {
            if (DataContext is MainWindowViewModel viewModel)
            {
                // Get the window service
                var windowService = WindowTabsFree.Services.WindowServiceFactory.Create();
                var configService = new WindowTabsFree.Core.Services.ConfigurationService();
                var windowManager = new WindowTabsFree.Core.Services.WindowManagerService(windowService, configService);
                
                _floatingWindow = new FloatingWindowManager(windowManager, windowService, configService);
                _floatingWindow.Show();
                
                // Minimize main window
                WindowState = WindowState.Minimized;
            }
        }
        else
        {
            _floatingWindow.Activate();
            // Also minimize main window when reactivating floating
            WindowState = WindowState.Minimized;
        }
    }
}
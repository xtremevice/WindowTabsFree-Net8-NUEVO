using Avalonia.Controls;
using Avalonia.Interactivity;
using System;
using WindowTabsFree.Common.Models;
using WindowTabsFree.UI.ViewModels;

namespace WindowTabsFree.UI.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
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
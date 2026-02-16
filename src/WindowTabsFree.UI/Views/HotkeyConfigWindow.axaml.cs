using Avalonia.Controls;
using Avalonia.Interactivity;
using WindowTabsFree.Common.Interfaces;

namespace WindowTabsFree.UI.Views
{
    public partial class HotkeyConfigWindow : Window
    {
        private readonly IConfigurationService _configService;

        public HotkeyConfigWindow(IConfigurationService configService)
        {
            InitializeComponent();
            _configService = configService;
            
            // Load current hotkey settings
            LoadCurrentSettings();
        }

        private void LoadCurrentSettings()
        {
            var settings = _configService.Settings;
            
            if (settings.HotKeys != null)
            {
                NextWindowTextBox.Text = settings.HotKeys.NextWindow ?? "";
                PreviousWindowTextBox.Text = settings.HotKeys.PreviousWindow ?? "";
            }
        }

        private void SaveButton_Click(object? sender, RoutedEventArgs e)
        {
            var settings = _configService.Settings;
            
            // Ensure HotKeys object exists
            if (settings.HotKeys == null)
            {
                settings.HotKeys = new WindowTabsFree.Common.Models.HotKeysSettings();
            }
            
            // Update hotkey settings
            settings.HotKeys.NextWindow = NextWindowTextBox.Text?.Trim();
            settings.HotKeys.PreviousWindow = PreviousWindowTextBox.Text?.Trim();
            
            // Save to file
            _configService.SaveSettings(settings);
            
            Close();
        }

        private void CancelButton_Click(object? sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}

using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using System;
using System.Collections.Generic;
using System.Linq;
using WindowTabsFree.Common.Interfaces;

namespace WindowTabsFree.UI.Views
{
    public partial class HotkeyConfigWindow : Window
    {
        private readonly IConfigurationService _configService;
        private TextBox? _capturingTextBox;
        private readonly HashSet<Key> _pressedKeys = new HashSet<Key>();

        public HotkeyConfigWindow(IConfigurationService configService)
        {
            InitializeComponent();
            _configService = configService;
            
            // Load current hotkey settings
            LoadCurrentSettings();
            
            // Attach key capture events
            AttachKeyCaptureEvents();
        }

        private void AttachKeyCaptureEvents()
        {
            // Attach events to both textboxes
            NextWindowTextBox.GotFocus += OnTextBoxGotFocus;
            NextWindowTextBox.LostFocus += OnTextBoxLostFocus;
            NextWindowTextBox.KeyDown += OnTextBoxKeyDown;
            NextWindowTextBox.KeyUp += OnTextBoxKeyUp;
            
            PreviousWindowTextBox.GotFocus += OnTextBoxGotFocus;
            PreviousWindowTextBox.LostFocus += OnTextBoxLostFocus;
            PreviousWindowTextBox.KeyDown += OnTextBoxKeyDown;
            PreviousWindowTextBox.KeyUp += OnTextBoxKeyUp;
        }

        private void OnTextBoxGotFocus(object? sender, GotFocusEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                _capturingTextBox = textBox;
                _pressedKeys.Clear();
                textBox.Text = "";
                textBox.Watermark = "Press keys now...";
            }
        }

        private void OnTextBoxLostFocus(object? sender, RoutedEventArgs e)
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

        private void OnTextBoxKeyDown(object? sender, KeyEventArgs e)
        {
            if (_capturingTextBox == null || sender != _capturingTextBox)
                return;

            e.Handled = true;

            // Add the key to pressed keys
            _pressedKeys.Add(e.Key);

            // Update the textbox with current combination
            UpdateHotkeyText();
        }

        private void OnTextBoxKeyUp(object? sender, KeyEventArgs e)
        {
            if (_capturingTextBox == null)
                return;

            e.Handled = true;

            // When keys are released, just remove the key from pressed keys
            if (_pressedKeys.Count > 0)
            {
                // Remove the released key
                _pressedKeys.Remove(e.Key);
                
                // Don't auto-advance to next field - let user decide when they're done
                // User can click on another field or use Tab key to move
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

            // Build the hotkey string
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
            // Order: Ctrl, Alt, Shift, Win, then regular keys
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
                Key.Back => "Backspace",
                Key.Return => "Enter",
                Key.Space => "Space",
                Key.Tab => "Tab",
                Key.Escape => "Esc",
                Key.Delete => "Delete",
                Key.Insert => "Insert",
                Key.Home => "Home",
                Key.End => "End",
                Key.PageUp => "PageUp",
                Key.PageDown => "PageDown",
                Key.Left => "Left",
                Key.Right => "Right",
                Key.Up => "Up",
                Key.Down => "Down",
                Key.F1 => "F1",
                Key.F2 => "F2",
                Key.F3 => "F3",
                Key.F4 => "F4",
                Key.F5 => "F5",
                Key.F6 => "F6",
                Key.F7 => "F7",
                Key.F8 => "F8",
                Key.F9 => "F9",
                Key.F10 => "F10",
                Key.F11 => "F11",
                Key.F12 => "F12",
                Key.D0 => "0",
                Key.D1 => "1",
                Key.D2 => "2",
                Key.D3 => "3",
                Key.D4 => "4",
                Key.D5 => "5",
                Key.D6 => "6",
                Key.D7 => "7",
                Key.D8 => "8",
                Key.D9 => "9",
                Key.NumPad0 => "NumPad0",
                Key.NumPad1 => "NumPad1",
                Key.NumPad2 => "NumPad2",
                Key.NumPad3 => "NumPad3",
                Key.NumPad4 => "NumPad4",
                Key.NumPad5 => "NumPad5",
                Key.NumPad6 => "NumPad6",
                Key.NumPad7 => "NumPad7",
                Key.NumPad8 => "NumPad8",
                Key.NumPad9 => "NumPad9",
                Key.Add => "NumPad+",
                Key.Subtract => "NumPad-",
                Key.Multiply => "NumPad*",
                Key.Divide => "NumPad/",
                Key.OemPlus => "+",
                Key.OemMinus => "-",
                Key.OemComma => ",",
                Key.OemPeriod => ".",
                _ => key.ToString()
            };
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

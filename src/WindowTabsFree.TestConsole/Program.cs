using WindowTabsFree.Services;
using WindowTabsFree.Core.Services;

namespace WindowTabsFree.TestConsole;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("WindowTabsFree - Test Console");
        Console.WriteLine("==============================\n");

        try
        {
            // Test WindowService factory
            Console.WriteLine("Testing WindowService factory...");
            var windowService = WindowServiceFactory.Create();
            Console.WriteLine($"✓ Created window service for platform: {Environment.OSVersion.Platform}\n");

            // Test ConfigurationService
            Console.WriteLine("Testing ConfigurationService...");
            var configService = new ConfigurationService();
            var settings = configService.Settings;
            Console.WriteLine($"✓ Configuration loaded successfully");
            Console.WriteLine($"  - Tab Height: {settings.TabSettings.TabHeight}");
            Console.WriteLine($"  - Show in Tray: {settings.ShowInSystemTray}");
            Console.WriteLine($"  - Excluded Apps: {string.Join(", ", settings.ExcludedApplications)}\n");

            // Test Window enumeration
            Console.WriteLine("Testing window enumeration...");
            var windows = windowService.GetAllWindows().ToList();
            Console.WriteLine($"✓ Found {windows.Count} windows");
            
            if (windows.Any())
            {
                Console.WriteLine("\nFirst 5 windows:");
                foreach (var window in windows.Take(5))
                {
                    Console.WriteLine($"  - {window.Title} ({window.ProcessName})");
                }
            }
            else
            {
                Console.WriteLine("  (No windows found - this is expected in headless environment)");
            }

            Console.WriteLine("\n✓ All tests passed!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\n✗ Error: {ex.Message}");
            Console.WriteLine(ex.StackTrace);
            Environment.Exit(1);
        }
    }
}

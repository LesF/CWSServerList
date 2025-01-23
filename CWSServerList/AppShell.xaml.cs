using Microsoft.Maui.Controls.Compatibility.Platform.UWP;
using Microsoft.Maui.Controls.PlatformConfiguration.AndroidSpecific;

namespace CWSServerList
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            CheckAndSwitchToSettingsTab();
        }

        private async void CheckAndSwitchToSettingsTab()
        {
            // Retrieve settings
            var databaseServer = Preferences.Get("DatabaseServer", string.Empty);
            var databaseName = Preferences.Get("DatabaseName", string.Empty);
            var user = Preferences.Get("User", string.Empty);
            var password = Preferences.Get("Password", string.Empty);

            // Check if any of the settings are missing
            if (string.IsNullOrWhiteSpace(databaseServer) || string.IsNullOrWhiteSpace(databaseName) || string.IsNullOrWhiteSpace(user) || string.IsNullOrWhiteSpace(password))
            {
                if (SettingsTab != null)
                {
                    // Switch to the Settings tab
                    CurrentItem = SettingsTab;              // Actions on the UI shouuld be done on the main thread
                    await Task.Delay(10);    // stop it whining about the missing await
                }
            }
        }
    }
}

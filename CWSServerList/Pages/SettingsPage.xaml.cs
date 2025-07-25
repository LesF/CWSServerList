using Microsoft.Maui.Storage;
using System;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using System.Text.RegularExpressions;

namespace CWSServerList.Pages
{
    public partial class SettingsPage : ContentPage
    {
        private readonly ConnectionStringService _connectionStringService;

        public SettingsPage()
        {
            InitializeComponent();
            // Get the ConnectionStringService from the service provider
            _connectionStringService = App.Services?.GetRequiredService<ConnectionStringService>() ?? throw new InvalidOperationException("App.Services is not initialized.");

            // Load settings
            ActivationKeyEntry.Text = Preferences.Get("ActivationKey", string.Empty);
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            // Remove all whitespace (spaces, tabs, newlines, etc.) that may have been included if user pasted the value into the entry
            string newKey = Regex.Replace(ActivationKeyEntry.Text ?? string.Empty, @"\s+", "");
            if (string.IsNullOrEmpty(newKey))
            {
                await DisplayAlert("Validation Error", "Activation Key is required", "OK");
                return;
            }

            string connectionString = _connectionStringService.DecryptDBKey(newKey) ?? string.Empty;
            if (string.IsNullOrEmpty(connectionString))
            {
                await DisplayAlert("Validation Error", "Invalid Activation Key", "OK");
            }
            else
            {
                // Test the database connection
                if (await TestDatabaseConnectionAsync(connectionString))
                {
                    Preferences.Set("ActivationKey", newKey);
                    await DisplayAlert("Settings", "Settings saved successfully", "OK");
                }
                else
                {
                    await DisplayAlert("Connection Error", "Failed to connect to the database. Please check your settings.", "OK");
                }
            }
        }

        private async Task<bool> TestDatabaseConnectionAsync(string connectionString)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    await connection.CloseAsync();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}

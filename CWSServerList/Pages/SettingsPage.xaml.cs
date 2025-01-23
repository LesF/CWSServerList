using Microsoft.Maui.Storage;
using System;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace CWSServerList.Pages
{
    public partial class SettingsPage : ContentPage
    {
        private readonly ConnectionStringService _connectionStringService;

        public SettingsPage()
        {
            InitializeComponent();
            // Get the ConnectionStringService from the service provider
            _connectionStringService = App.Services.GetRequiredService<ConnectionStringService>();

            // Load settings
            DatabaseServerName.Text = Preferences.Get("DatabaseServer", string.Empty);
            DatabaseNameEntry.Text = Preferences.Get("DatabaseName", string.Empty);
            UserEntry.Text = Preferences.Get("User", string.Empty);
            PasswordEntry.Text = Preferences.Get("Password", string.Empty);
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            // Validate inputs
            if (string.IsNullOrWhiteSpace(DatabaseServerName.Text))
            {
                await DisplayAlert("Validation Error", "Database Server Name is required.", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(DatabaseNameEntry.Text))
            {
                await DisplayAlert("Validation Error", "Database Name is required.", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(UserEntry.Text))
            {
                await DisplayAlert("Validation Error", "User is required.", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(PasswordEntry.Text))
            {
                await DisplayAlert("Validation Error", "Password is required.", "OK");
                return;
            }

            // Create connection string
            var connectionString = $"Server={DatabaseServerName.Text};Database={DatabaseNameEntry.Text};User Id={UserEntry.Text};Password={PasswordEntry.Text};TrustServerCertificate=True;";

            // Test the database connection
            if (await TestDatabaseConnectionAsync(connectionString))
            {
                // Save the settings securely
                Preferences.Set("DatabaseServer", DatabaseServerName.Text);
                Preferences.Set("DatabaseName", DatabaseNameEntry.Text);
                Preferences.Set("User", UserEntry.Text);
                Preferences.Set("Password", PasswordEntry.Text);

                // Update the connection string in the service
                _connectionStringService.UpdateConnectionString(DatabaseServerName.Text, DatabaseNameEntry.Text, UserEntry.Text, PasswordEntry.Text);

                await DisplayAlert("Settings", "Settings saved successfully", "OK");
            }
            else
            {
                await DisplayAlert("Connection Error", "Failed to connect to the database. Please check your settings.", "OK");
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

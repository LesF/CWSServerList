public class ConnectionStringService
{
    private string? _connectionString;

    public string GetConnectionString()
    {
        if (string.IsNullOrEmpty(_connectionString))
        {
            // Retrieve settings
            var databaseServer = Preferences.Get("DatabaseServer", string.Empty);
            var databaseName = Preferences.Get("DatabaseName", string.Empty);
            var user = Preferences.Get("User", string.Empty);
            var password = Preferences.Get("Password", string.Empty);

            // Create connection string
            _connectionString = $"Server={databaseServer};Database={databaseName};User Id={user};Password={password};TrustServerCertificate=True;";
        }

        return _connectionString;
    }

    public void UpdateConnectionString(string databaseServer, string databaseName, string user, string password)
    {
        _connectionString = $"Server={databaseServer};Database={databaseName};User Id={user};Password={password};TrustServerCertificate=True;";
    }
}

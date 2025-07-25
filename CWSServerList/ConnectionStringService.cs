using System.Security.Cryptography;
using System.Text;

public class ConnectionStringService
{
    private string? _connectionString;

    // Example secret key and IV (must be 32 bytes for AES-256 and 16 bytes for IV)
    private static readonly byte[] Key = Encoding.UTF8.GetBytes("00Cws00ServerList00Activation001");
    private static readonly byte[] IV = Encoding.UTF8.GetBytes("00Cws00Server0IV");

    public string GetConnectionString()
    {
        if (string.IsNullOrEmpty(_connectionString))
        {
            var activationKey = Preferences.Get("ActivationKey", string.Empty);
            _connectionString = DecryptDBKey(activationKey);
        }
        return _connectionString;
    }

    // EncryptDBKey method that takes the four database connection parameters and encrypts them into a text block
    public string EncryptDBKey(string databaseServer, string databaseName, string user, string password)
    {
        string activationKey = $"{databaseServer}|{databaseName}|{user}|{password}";
        using (Aes aes = Aes.Create())
        {
            aes.Key = Key;
            aes.IV = IV;
            ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

            byte[] plainBytes = Encoding.UTF8.GetBytes(activationKey);
            byte[] encryptedBytes;
            using (var ms = new MemoryStream())
            using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
            {
                cs.Write(plainBytes, 0, plainBytes.Length);
                cs.FlushFinalBlock();
                encryptedBytes = ms.ToArray();
            }
            // Return as Base64 string for sharing
            return Convert.ToBase64String(encryptedBytes);
        }
    }

    // DecryptDBKey method to decrypt the ActivationKey text, extract the four parameters needed for the database connection string
    public string DecryptDBKey(string activationKey)
    {
        try
        {
            byte[] encryptedBytes = Convert.FromBase64String(activationKey);
            using (Aes aes = Aes.Create())
            {
                aes.Key = Key;
                aes.IV = IV;
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (var ms = new MemoryStream(encryptedBytes))
                using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                using (var reader = new StreamReader(cs, Encoding.UTF8))
                {
                    string decrypted = reader.ReadToEnd();
                    string[] dbParams = (decrypted + "|||").Split(new char[] { '|' });
                    return $"Server={dbParams[0]};Database={dbParams[1]};User Id={dbParams[2]};Password={dbParams[3]};TrustServerCertificate=True;";
                }
            }
        }
        catch
        {
            // Fallback to original logic if not encrypted
            string[] dbParams = (activationKey).Split(new char[] { '|' });
            if (dbParams.Length > 3)
                return $"Server={dbParams[0]};Database={dbParams[1]};User Id={dbParams[2]};Password={dbParams[3]};TrustServerCertificate=True;";
        }
        return "";
    }

    public void UpdateConnectionString(string databaseServer, string databaseName, string user, string password)
    {
        _connectionString = $"Server={databaseServer};Database={databaseName};User Id={user};Password={password};TrustServerCertificate=True;";
    }
}

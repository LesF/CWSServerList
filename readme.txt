
dotnet ef dbcontext scaffold "Server=your_server;Database=your_database;User Id=your_user;Password=your_password;" Microsoft.EntityFrameworkCore.SqlServer -o Models

<add connectionString="Server=WAI-DB-CWSCLI-D-002.db.waikato.health.govt.nz;Database=ClinicalIntranet_Alpha;User ID=sql_healthviews_alpha;Password=a8pruXEq" name="CISDB" providerName="System.Data.SqlClient" />

dotnet ef dbcontext scaffold "Server=WAI-DB-CWSCLI-D-002.db.waikato.health.govt.nz;Database=ClinicalIntranet_Alpha;User ID=sql_healthviews_alpha;Password=a8pruXEq;TrustServerCertificate=True;" Microsoft.EntityFrameworkCore.SqlServer -o Models -t CWSServer -t UserPrefs -t CWSEnvironment -t CWSServerPurpose -t CWSVersion

dotnet ef dbcontext scaffold "Server=localhost;Database=MyDatabase;User Id=myuser;Password=mypassword;" Microsoft.EntityFrameworkCore.SqlServer -o Models -t Employees -t Departments



// Retrieve settings
//var databaseServer = Preferences.Get("DatabaseServer", string.Empty);
//var databaseName = Preferences.Get("DatabaseName", string.Empty);
//var user = Preferences.Get("User", string.Empty);
//var password = Preferences.Get("Password", string.Empty);
// Create connection string
// var connectionString = $"Server={databaseServer};Database={databaseName};User Id={user};Password={password};";
// Add DbContext
// builder.Services.AddDbContext<AppDbContext>(options =>
//    options.UseSqlServer(connectionString));


//public class AppDbContext : DbContext
//{
//    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

//    // Define your DbSets here
//    //public DbSet<YourEntity> YourEntities { get; set; }
//}


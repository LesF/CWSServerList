using CWSServerList.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Storage;
using CWSServerList.ViewModels;

namespace CWSServerList
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            // Register ConnectionStringService
            builder.Services.AddSingleton<ConnectionStringService>();

            // Add DbContext
            builder.Services.AddDbContext<ClinicalIntranetAlphaContext>((serviceProvider, options) =>
            {
                var connectionStringService = serviceProvider.GetRequiredService<ConnectionStringService>();
                options.UseSqlServer(connectionStringService.GetConnectionString());
            });

            // Register DataService with scoped lifetime
            builder.Services.AddScoped<DataService>();

            // Register MainPageViewModel with transient lifetime
            builder.Services.AddTransient<MainPageViewModel>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            var app = builder.Build();

            // Check for command line argument key=xxx and generate a new ActivationKey value
            var args = Environment.GetCommandLineArgs();
            foreach (var arg in args)
            {
                if (arg.StartsWith("key="))
                {
                    var value = arg.Substring("key=".Length);
                    if (!string.IsNullOrEmpty(value))
                    {
                        GenerateNewKey(value, app.Services);
                    }
                }
            }

            return app;
        }

        // Process command-line value for generating a new ActivationKey
        // Expected format: key=server;database;user;password
        // The new ActivationKey will be stored to user's app preferences.  This key can be
        // shared with users, who can paste it into their settings to activate database access.
        private static void GenerateNewKey(string value, IServiceProvider services)
        {
            var connectionStringService = services.GetRequiredService<ConnectionStringService>();
            string[] dbParams = value.Split(';');
            if (dbParams.Length > 3)
            {
                string newKey = connectionStringService.EncryptDBKey(dbParams[0], dbParams[1], dbParams[2], dbParams[3]);
                if (!string.IsNullOrEmpty(newKey))
                {
                    Preferences.Set("ActivationKey", newKey);
                    // and refresh the connection string
                    connectionStringService.GetConnectionString();
                }
            }
        }
    }
}

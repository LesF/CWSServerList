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
            // On first run, the settings tab will be displayed, allowing the user to enter database connection information
            // On subsequent runs, the connection string will be read from the settings file
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

            return builder.Build();
        }
    }

}

using System;
using System.Collections.ObjectModel;
using CWSServerList.Models;
using CWSServerList.ViewModels;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using System.Diagnostics;
using System.IO;

namespace CWSServerList.Pages
{
    public partial class MainPage : ContentPage
    {
        private MainPageViewModel? _mainPageViewModel;

        public MainPage()
        {
            InitializeComponent();
            _mainPageViewModel = App.Services?.GetRequiredService<MainPageViewModel>();
            Console.WriteLine("Groups: {0}", _mainPageViewModel?.ServerGroups.Count);

            BindingContext = _mainPageViewModel;

            Console.WriteLine("MainPage constructor");
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (_mainPageViewModel != null)
            {
                Console.WriteLine("OnAppearing: Calling LoadData");
                _mainPageViewModel.LoadData();
            }
        }

        private async void OnBrowseTapped(object sender, EventArgs e)
        {
            if (sender is Image image && image.BindingContext is Cwsserver server)
            {
                // Handle the Browse action
                Console.WriteLine($"Browse tapped for server: {server.ServerName}");

                if (!string.IsNullOrEmpty(server.Cwsurl))
                {
                    try
                    {
                        await Launcher.OpenAsync(new Uri(server.Cwsurl));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Failed to open URL: {ex.Message}");
                    }
                }
            }
        }

        private void OnRdpTapped(object sender, EventArgs e)
        {
            if (sender is Image image && image.BindingContext is Cwsserver server)
            {
                // Handle the RDP action
                Console.WriteLine($"RDP tapped for server: {server.ServerName}");

                try
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = "mstsc",
                        Arguments = $"/v:{server.ServerName}",
                        UseShellExecute = true
                    });
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to start RDP session: {ex.Message}");
                }
            }
        }

        private void OnLogsTapped(object sender, EventArgs e)
        {
            if (sender is Image image && image.BindingContext is Cwsserver server)
            {
                // Handle the Logs action
                Console.WriteLine($"Logs tapped for server: {server.ServerName}");

                string logPath = $@"\\{server.ServerName}\CWSLogs";

                try
                {
                    // Check if user has access to the path
                    if (Directory.Exists(logPath))
                    {
                        Console.WriteLine($"Logs path exists: {logPath}");

                        // Check user permissions on the files
                        var files = Directory.GetFiles(logPath);

                        // Open the path in the default file explorer
                        Process.Start(new ProcessStartInfo
                        {
                            FileName = logPath,
                            UseShellExecute = true
                        });
                    }
                    else
                    {
                        Console.WriteLine($"Logs path does not exist: {logPath}");
                    }
                }
                catch (UnauthorizedAccessException)
                {
                    Console.WriteLine($"Access denied to path: {logPath}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to open logs path: {ex.Message}");
                }
            }
        }
    }

}

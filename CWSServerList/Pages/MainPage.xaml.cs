using System;
using System.Collections.ObjectModel;
using CWSServerList.Models;
using CWSServerList.ViewModels;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using System.Diagnostics;

namespace CWSServerList.Pages
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();
            var mainPageViewModel = App.Services?.GetRequiredService<MainPageViewModel>();
            Console.WriteLine("Groups: {0}", mainPageViewModel.ServerGroups.Count);

            BindingContext = mainPageViewModel;

            Console.WriteLine("MainPage constructor");
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
                // Add your logic here
            }
        }
    }

    /* To display the full URL:
     * <Label Text="{Binding Cwsurl}" VerticalOptions="Center" IsVisible="{Binding Cwsurl, Converter={StaticResource Key=StringToVisibilityConverter}}" Margin="10,0,0,0" Grid.Column="2" />
     * Removed this, will have an icon instead, and dsplay Notes in the third column
     * 
     * Removed inactive servers from the data source so this is not needed:
     * <Image Source="{Binding IsActive, Converter={StaticResource BoolToImageConverter}}" WidthRequest="16" HeightRequest="16" VerticalOptions="Center" Margin="10,0,0,0" Grid.Column="1" Visual="Default" />
     */
}

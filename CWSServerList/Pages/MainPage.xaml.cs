
using System.Collections.ObjectModel;
using CWSServerList.Models;
using CWSServerList.ViewModels;

namespace CWSServerList.Pages
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();

            // BindingContext = App.Services?.GetRequiredService<MainPageViewModel>();

            var mainPageViewModel = App.Services?.GetRequiredService<MainPageViewModel>();
            Console.WriteLine("Groups: {0}", mainPageViewModel.ServerGroups.Count);

            BindingContext = mainPageViewModel;

            Console.WriteLine("MainPage constructor");
        }
    }

    /* <Label Text="{Binding IsActive, Converter={StaticResource BoolToImageConverter}}" Grid.Column="1" />
     */
}

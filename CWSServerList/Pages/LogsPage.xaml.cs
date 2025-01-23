using Microsoft.Maui.Controls;
using CWSServerList.ViewModels;

namespace CWSServerList.Pages
{
    public partial class LogsPage : ContentPage
    {
        private LogsPageViewModel _viewModel;

        public LogsPage()
        {
            InitializeComponent();
            _viewModel = BindingContext as LogsPageViewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel?.StartAutoRefresh();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            _viewModel?.StopAutoRefresh();
        }
    }
}

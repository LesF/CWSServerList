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

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (_viewModel != null)
            {
                // Ensure that logs are refreshed when the page appears
                await _viewModel.RefreshLogsAsync();
                _viewModel.StartAutoRefresh();
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            _viewModel?.StopAutoRefresh();
        }

        private void RadioButton_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (sender is RadioButton radioButton && e.Value)
            {
                var viewModel = BindingContext as LogsPageViewModel;
                if (viewModel != null)
                {
                    viewModel.SelectedEnvironment = radioButton.Content.ToString().StartsWith("P") ? "P" : "T";
                }
            }
        }

        private void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Clear selection after handling to allow re-selection of the same item
            ((CollectionView)sender).SelectedItem = null;
        }
    }
}

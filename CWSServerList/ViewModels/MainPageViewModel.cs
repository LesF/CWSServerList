using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using CWSServerList.Models;
using Microsoft.Extensions.Logging;

namespace CWSServerList.ViewModels
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        private readonly DataService _dataService;

        public ObservableCollection<EnvironmentGroup> _serverGroups { get; set; }

        public ObservableCollection<EnvironmentGroup> ServerGroups
        {
            get => _serverGroups;
            set
            {
                _serverGroups = value;
                OnPropertyChanged();
            }
        }

        public MainPageViewModel(DataService dataService)
        {
            _dataService = dataService;
            _serverGroups = new ObservableCollection<EnvironmentGroup>();
            LoadData();
        }
        public MainPageViewModel() : this(App.Services.GetRequiredService<DataService>())
        {
        }

        private async void LoadData()
        {
            _serverGroups = new ObservableCollection<EnvironmentGroup>();
            var groupedData = new List<EnvironmentGroup>();
            try
            {
                Console.WriteLine("Loading environments...");
                var environments = await _dataService.GetEnvironmentsAsync();
                Console.WriteLine($"Environments loaded: {environments.Count}");

                Console.WriteLine("Loading servers...");
                var servers = await _dataService.GetServersAsync();
                Console.WriteLine($"Servers loaded: {servers.Count}");

                groupedData = environments.Select(env =>
                    new EnvironmentGroup(
                        env.Environment,
                        servers.Where(s => s.EnvironmentCode == env.EnvironmentCode && s.IsActive).ToList())).ToList();
            }
            catch (System.Exception ex)
            {
                Console.WriteLine("Error loading environments and servers; {0}", ex.Message);
                return;
            }

            try
            {
                foreach (var group in groupedData)
                {
                    _serverGroups.Add(group);
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine("Error adding groups; {0}", ex.Message);
                return;
            }
            Console.WriteLine("Groups loaded: {0}", ServerGroups.Count);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class EnvironmentGroup : ObservableCollection<Cwsserver>
    {
        // Group heading name
        public string EnvironmentName { get; private set; }

        public EnvironmentGroup(string environmentName, List<Cwsserver> servers) : base(servers)
        {
            EnvironmentName = environmentName;
        }
    }
}


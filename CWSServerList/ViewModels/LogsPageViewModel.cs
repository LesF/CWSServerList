using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Timers;
using CWSServerList.Models;
using Microsoft.Maui.Controls;

namespace CWSServerList.ViewModels
{
    public class LogsPageViewModel : INotifyPropertyChanged
    {
        private readonly DataService _dataService;
        private readonly System.Timers.Timer _refreshTimer;
        private ObservableCollection<ServerLogGroup> _logGroups;
        private string _selectedEnvironment;

        public ObservableCollection<ServerLogGroup> LogGroups
        {
            get => _logGroups;
            set
            {
                _logGroups = value;
                OnPropertyChanged();
            }
        }

        public string SelectedEnvironment
        {
            get => _selectedEnvironment;
            set
            {
                _selectedEnvironment = value;
                OnPropertyChanged();
                RefreshLogsAsync();
            }
        }

        public LogsPageViewModel(DataService dataService)
        {
            _dataService = dataService;
            _logGroups = new ObservableCollection<ServerLogGroup>();
            _selectedEnvironment = "P"; // Default to Production
            _refreshTimer = new System.Timers.Timer(30000); // 30 seconds
            _refreshTimer.Elapsed += async (sender, e) => await RefreshLogsAsync();
            _refreshTimer.Start();
        }

        public LogsPageViewModel() : this(App.Services.GetRequiredService<DataService>())
        {
        }

        public async Task RefreshLogsAsync()
        {
            // Poll the log directories and update the LogGroups collection
            var servers = await _dataService.GetServersByEnvironmentAsync(_selectedEnvironment);
            var logGroups = new ObservableCollection<ServerLogGroup>();

            foreach (var server in servers)
            {
                var logPath = $@"\\{server.ServerName}\CWSLogs";
                if (Directory.Exists(logPath))
                {
                    var logFiles = Directory.GetFiles(logPath, "*.log", SearchOption.TopDirectoryOnly)
                        .Select(file => new LogFile
                        {
                            FileName = Path.GetFileName(file),
                            Size = new FileInfo(file).Length,
                            LastUpdated = File.GetLastWriteTime(file)
                        })
                        .ToList();

                    logGroups.Add(new ServerLogGroup(server.ServerName, logFiles));
                }
            }

            LogGroups = logGroups;
        }

        public void StartAutoRefresh()
        {
            _refreshTimer.Start();
        }

        public void StopAutoRefresh()
        {
            _refreshTimer.Stop();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class ServerLogGroup : ObservableCollection<LogFile>
    {
        public string ServerName { get; private set; }

        public ServerLogGroup(string serverName, List<LogFile> logFiles) : base(logFiles)
        {
            ServerName = serverName;
        }
    }

    public class LogFile
    {
        public string FileName { get; set; }
        public long Size { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}

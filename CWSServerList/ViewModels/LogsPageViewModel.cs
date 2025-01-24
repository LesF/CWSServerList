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
using Microsoft.Maui.ApplicationModel;

namespace CWSServerList.ViewModels
{
    public class LogsPageViewModel : INotifyPropertyChanged
    {
        private readonly DataService _dataService;
        private readonly System.Timers.Timer _refreshTimer;
        private ObservableCollection<ServerLogGroup> _logGroups;
        private string _selectedEnvironment = "P";

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
                if (_selectedEnvironment != value)
                {
                    _selectedEnvironment = value;
                    OnPropertyChanged(nameof(SelectedEnvironment));
                    RefreshLogsAsync().ConfigureAwait(false);
                }
            }
        }

        public Command<LogFile> OpenLogCommand { get; }

        public LogsPageViewModel(DataService dataService)
        {
            _dataService = dataService;
            _logGroups = new ObservableCollection<ServerLogGroup>();
            _selectedEnvironment = "P"; // Default to Production
            _refreshTimer = new System.Timers.Timer(30000); // 30 seconds
            _refreshTimer.Elapsed += async (sender, e) => await RefreshLogsAsync();
            _refreshTimer.Start();

            OpenLogCommand = new Command<LogFile>(async (log) => await ExecuteOpenLogCommand(log));

            SelectedEnvironment = "P"; // This will trigger RefreshLogsAsync
        }

        public LogsPageViewModel() : this(App.Services.GetRequiredService<DataService>())
        {
        }

        public async Task RefreshLogsAsync()
        {
            // Poll the log directories and update the LogGroups collection
            try
            {
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
                                LastUpdated = File.GetLastWriteTime(file),
                                DirectoryPath = logPath
                            })
                            .ToList();

                        logGroups.Add(new ServerLogGroup(server.ServerName, logFiles));
                    }
                }

                LogGroups = logGroups;
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., log error, notify user)
                Console.WriteLine($"Error refreshing logs: {ex.Message}");
            }
        }

        public void StartAutoRefresh()
        {
            _refreshTimer.Start();
            RefreshLogsAsync().ConfigureAwait(false);
        }

        public void StopAutoRefresh()
        {
            _refreshTimer.Stop();
        }

        private async Task ExecuteOpenLogCommand(LogFile log)
        {
            if (log != null)
            {
                try
                {
                    string networkPath = Path.Combine(log.DirectoryPath, log.FileName);
                    var uri = new Uri(networkPath);

                    // Open the log file with the default viewer
                    await Launcher.OpenAsync(uri);
                }
                catch (Exception ex)
                {
                    // Display an alert to the user
                    await Application.Current.MainPage.DisplayAlert("Error", $"Unable to open log file: {ex.Message}", "OK");
                }
            }
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
        public string DirectoryPath { get; set; }
        public long Size { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}

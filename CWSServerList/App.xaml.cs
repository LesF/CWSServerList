namespace CWSServerList
{
    public partial class App : Application
    {
        public static IServiceProvider? Services { get; private set; }

        public App(IServiceProvider serviceProvider)
        {
            InitializeComponent();
            Services = serviceProvider;
            MainPage = new AppShell();
        }

        protected override Window CreateWindow(IActivationState ?activationState)
        {
            var window = base.CreateWindow(activationState);

            // Get the screen width and height
            var displayInfo = DeviceDisplay.MainDisplayInfo;
            var screenWidth = displayInfo.Width / displayInfo.Density;
            var screenHeight = displayInfo.Height / displayInfo.Density;

            // Set the window size to 1/3 of the screen width and 2/3 of the screen height
            window.Width = screenWidth / 3;
            window.Height = (screenHeight * 2) / 3;

            return window;
        }
    }
}

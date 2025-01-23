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

            // This could be a mess on different screen sizes, to be reviewed
            int width = 360;
            if (width > screenWidth)
            {
                width = (int)(screenWidth - 20);
            }
            int height = 760;
            if (height > screenHeight)
            {
                height = (int)(screenHeight - 20);
            }
            window.Width = width;
            window.Height = height;

            return window;
        }
    }
}

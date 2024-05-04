namespace ICS
{
    /*public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }
    }*/
    public partial class App : Application
    {
        public App(IServiceProvider serviceProvider)
        {
            InitializeComponent();

            MainPage = serviceProvider.GetRequiredService<AppShell>();
        }
    }
}

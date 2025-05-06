namespace DataGridMAUI
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MDAxQDMyMzkyZTMwMmUzMDNiMzIzOTNiaEJ0cWhtUVpBOXpnNXk3SDBjNkwybGJHQTRSU2xMZHFtbGM4ZkxjR3pndz0=");
             MainPage = new MainPage();
        }
    }
}

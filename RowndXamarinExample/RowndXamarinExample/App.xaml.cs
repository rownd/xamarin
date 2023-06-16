using Rownd;
using RowndXamarinExample.Services;
using Xamarin.Forms;

namespace RowndXamarinExample
{
    public partial class App : Application
    {
        public RowndInstance Rownd;

        public App()
        {
            var rowndConfig = new Rownd.Core.Config();
            rowndConfig.ApiUrl = "https://api.us-east-2.dev.rownd.io";
            rowndConfig.HubUrl = "https://hub.dev.rownd.io";
            rowndConfig.SubdomainExtension = ".dev.rownd.link";

            Rownd = RowndInstance.GetInstance(this, rowndConfig);
            Rownd.Configure("b60bc454-c45f-47a2-8f8a-12b2062f5a77");

            DependencyService.RegisterSingleton<IRowndInstance>(Rownd);

            InitializeComponent();

            DependencyService.Register<MockDataStore>();

            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}

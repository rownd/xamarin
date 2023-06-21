using Rownd.Xamarin;
using RowndXamarinExample.Services;
using Xamarin.Forms;

namespace RowndXamarinExample
{
    public partial class App : Application
    {
        public RowndInstance Rownd { get; private set; }

        public App()
        {
            Rownd = RowndInstance.GetInstance(this);
            Rownd.Configure("YOUR_APP_KEY");

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

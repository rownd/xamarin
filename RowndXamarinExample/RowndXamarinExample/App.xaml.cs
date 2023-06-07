using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using RowndXamarinExample.Services;
using RowndXamarinExample.Views;
using Rownd;

namespace RowndXamarinExample
{
    public partial class App : Application
    {

        private RowndInstance rownd;

        public App ()
        {
            var rowndConfig = new Rownd.Core.Config();
            rowndConfig.apiUrl = "https://api.us-east-2.dev.rownd.io";
            rowndConfig.hubUrl = "https://hub.dev.rownd.io";

            rownd = RowndInstance.GetInstance(this, rowndConfig);
            rownd.Configure("b60bc454-c45f-47a2-8f8a-12b2062f5a77");
            
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            
            MainPage = new AppShell();
        }

        protected override void OnStart ()
        {
        }

        protected override void OnSleep ()
        {
        }

        protected override void OnResume ()
        {
        }
    }
}


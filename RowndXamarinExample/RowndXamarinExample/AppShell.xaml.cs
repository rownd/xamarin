using System;
using System.Collections.Generic;
using RowndXamarinExample.ViewModels;
using RowndXamarinExample.Views;
using Xamarin.Forms;

namespace RowndXamarinExample
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
        }

    }
}


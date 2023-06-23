using System;
using System.ComponentModel;
using RowndXamarinExample.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RowndXamarinExample.Views
{
    public partial class AboutPage : ContentPage
    {
        private AboutViewModel ViewModel;

        public AboutPage()
        {
            InitializeComponent();
            ViewModel = new(this);
            BindingContext = ViewModel;
        }
    }
}

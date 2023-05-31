using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BottomSheetXF.Implementations;
using Rownd.Core;
using RowndXamarinExample.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RowndXamarinExample.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        private LoginViewModel ViewModel;

        public LoginPage()
        {
            InitializeComponent();
            ViewModel = new();
            BindingContext = ViewModel;
        }
    }
}

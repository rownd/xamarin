using System.ComponentModel;
using Xamarin.Forms;
using RowndXamarinExample.ViewModels;

namespace RowndXamarinExample.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}

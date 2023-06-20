using System;
using System.ComponentModel;
using System.Windows.Input;
using Rownd;
using Rownd.Models.Domain;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace RowndXamarinExample.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        public GlobalState RowndState { get; set; }

        public AboutViewModel()
        {
            Title = "About";
            OpenWebCommand = new Command(async () => await Browser.OpenAsync("https://aka.ms/xamarin-quickstart"));
            RequestSignIn = new Command(() => Rownd.RequestSignIn());
            SignOut = new Command(() => Rownd.SignOut());
            RefreshToken = new Command(() => Rownd._InternalTestRefreshToken());
            Rownd.Store.Select().Subscribe((state) =>
            {
                RowndState = state;
                //NotifyPropertyChanged("RowndState");
                //SetProperty(ref RowndState, state);
                OnPropertyChanged("RowndState");
            });
        }

        public ICommand OpenWebCommand { get; }

        public ICommand RequestSignIn { get; }

        public ICommand SignOut { get; }

        public ICommand RefreshToken { get; }

        //public event PropertyChangedEventHandler PropertyChanged;
        //private void NotifyPropertyChanged(string property)
        //{
        //    if (PropertyChanged != null)
        //    {
        //        PropertyChanged(this, new PropertyChangedEventArgs(property));
        //    }
        //}
    }
}

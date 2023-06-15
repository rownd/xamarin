using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Rownd.Core;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Rownd.Controls
{
    public partial class HubPage : ContentPage
    {

        #region Properties

        public static BindableProperty SheetHeightProperty = BindableProperty.Create(
            nameof(SheetHeight),
            typeof(double),
            typeof(BottomSheet),
            defaultValue: default(double),
            defaultBindingMode: BindingMode.TwoWay);

        public double SheetHeight
        {
            get { return (double)GetValue(SheetHeightProperty); }
            set { SetValue(SheetHeightProperty, value); OnPropertyChanged(); }
        }

        //public static BindableProperty SheetContentProperty = BindableProperty.Create(
        //    nameof(SheetContent),
        //    typeof(View),
        //    typeof(BottomSheet),
        //    defaultValue: default(View),
        //    defaultBindingMode: BindingMode.TwoWay);

        //public View SheetContent
        //{
        //    get { return (View)GetValue(SheetContentProperty); }
        //    set { SetValue(SheetContentProperty, value); OnPropertyChanged(); }
        //}

        #endregion

        public HubPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            SheetHeight = 300;
            BackgroundColor = Color.Transparent;
            _ = AnimateIn();
        }

        readonly uint duration = 450;
        double openPosition = (DeviceInfo.Platform == DevicePlatform.Android) ? 20 : 60;
        double currentPosition = 0;

        public async void PanGestureRecognizer_PanUpdated(object sender, PanUpdatedEventArgs e)
        {
            try
            {
                switch(e.StatusType)
                {
                    case GestureStatus.Running:
                        var translateY = Math.Max(Math.Min(0, currentPosition + e.TotalY), -Math.Abs((Height * .25) - Height));
                        //PanContainerRef.Content.TranslationY = openPosition + e.TotalY;
                        await PanContainerRef.Content.TranslateTo(0, openPosition + e.TotalY);
                        break;

                    case GestureStatus.Completed:
                        var threshold = SheetHeight * 0.55;
                        currentPosition = e.TotalY;
                        var finalTranslation = Math.Max(Math.Min(0, -1000), -Math.Abs(e.TotalY + currentPosition));

                        if (isSwipeUp(e))
                        {
                            await AnimateTo(finalTranslation, Easing.SpringIn);
                        }
                        else
                        {
                            await AnimateTo(finalTranslation, Easing.SpringOut);
                        }

                        if (currentPosition < threshold)
                        {
                            //await OpenSheet();
                        }
                        else
                        {
                            //await Dismiss();
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public bool isSwipeUp(PanUpdatedEventArgs e)
        {
            if (e.TotalY < 0)
            {
                return true;
            }
            return false;
        }

        public void Expand(object sender, FocusEventArgs e)
        {
            //open sheet to 85% of the view
            //var finalTranslation = Math.Max(Math.Min(0, -1000), -Math.Abs(GetProportionCoordinate(.85)));
            //bottomSheet.TranslateTo(bottomSheet.X, finalTranslation, 150, Easing.SpringIn);
        }

        public async Task Dismiss()
        {
            
            await AnimateOut();
            await Shared.app.MainPage.Navigation.PopModalAsync(false);
        }

        double GetProportionCoordinate(double proportion)
        {
            return proportion * Height;
        }

        async Task AnimateTo(double position, Easing easing = null)
        {
            if (easing == null)
            {
                easing = Easing.SpringOut;
            }

            await PanContainerRef.Content.TranslateTo(0, position, duration, easing);
        }

        async Task AnimateIn()
        {
            //var finalTranslation = Math.Max(Math.Min(0, -1000), -Math.Abs(GetProportionCoordinate(.15)));
            //bottomSheet.TranslateTo(bottomSheet.X, finalTranslation, 450, Easing.Linear);
            PanContainerRef.Content.TranslationY = SheetHeight + 60;
            await Task.WhenAll(
                Backdrop.FadeTo(0.4, length: duration),
                Sheet.TranslateTo(0, openPosition, length: duration, easing: Easing.SinIn)
            );
        }

        async Task AnimateOut()
        {
            await Task.WhenAll(
                PanContainerRef.Content.TranslateTo(x: 0, y: 0, length: duration, easing: Easing.SinIn),
                Backdrop.FadeTo(0)
            );
        }

        void TapGestureRecognizer_Tapped(System.Object sender, System.EventArgs e)
        {
            _ = Dismiss();
        }
    }
}


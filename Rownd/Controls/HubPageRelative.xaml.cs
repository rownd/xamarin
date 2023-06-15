using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Rownd.Core;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Rownd.Controls
{
    public partial class HubPageRelative : ContentPage
    {

        #region Properties

        public static BindableProperty InitialPositionProperty = BindableProperty.Create(
            nameof(InitialPosition),
            typeof(int),
            typeof(BottomSheet),
            defaultValue: 200,
            defaultBindingMode: BindingMode.OneTime);

        public int InitialPosition
        {
            get { return (int)GetValue(InitialPositionProperty); }
            set { SetValue(InitialPositionProperty, value); OnPropertyChanged(); }
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

        public ICommand TriggerExpand { get; }

        #endregion

        public HubPageRelative()
        {
            InitializeComponent();
            TriggerExpand = new Command(() => Expand());
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            BackgroundColor = new Color(0, 0, 0, 0.01);
            _ = AnimateIn();
        }

        readonly uint duration = 300;
        double currentPosition = 0;

        public async void OnBottomSheetPan(object sender, PanUpdatedEventArgs e)
        {
            try
            {
                switch(e.StatusType)
                {
                    case GestureStatus.Running:
                        var translateY = Math.Max(Math.Min(0, currentPosition + e.TotalY), -Math.Abs((Height * .10) - Height));
                        await Sheet.TranslateTo(Sheet.X, translateY, 20);

                        break;

                    case GestureStatus.Completed:
                        currentPosition = Sheet.TranslationY;

                        if (!isSwipeUp(e) && Math.Abs(currentPosition) < InitialPosition)
                        {
                            await Dismiss();
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

        public void Expand()
        {
            //open sheet to 90% of the view
            var finalTranslation = Math.Max(Math.Min(0, -1000), -Math.Abs(GetProportionCoordinate(.90)));
            Sheet.TranslateTo(Sheet.X, finalTranslation, 150, Easing.SpringIn);
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

            await Sheet.TranslateTo(0, position, duration, easing);
        }

        async Task AnimateIn()
        {
            await Task.WhenAll(
                Backdrop.FadeTo(0.5, length: duration),
                Sheet.TranslateTo(0, -InitialPosition, length: duration, easing: Easing.SinOut),
                Sheet.FadeTo(1, duration, Easing.SinOut)
            );
            currentPosition = Sheet.TranslationY;
        }

        async Task AnimateOut()
        {
            await Task.WhenAll(
                Sheet.TranslateTo(x: 0, y: 0, length: duration, easing: Easing.SinIn),
                Sheet.FadeTo(0, duration, Easing.SinIn),
                Backdrop.FadeTo(0, duration)
            );
            currentPosition = Sheet.TranslationY;
        }

        void TapGestureRecognizer_Tapped(System.Object sender, System.EventArgs e)
        {
            _ = Dismiss();
        }
    }
}


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

        private static BindableProperty initialPositionProperty = BindableProperty.Create(
            nameof(InitialPosition),
            typeof(int),
            typeof(HubPageRelative),
            defaultValue: 200,
            defaultBindingMode: BindingMode.OneTime);

        public int InitialPosition
        {
            get
            {
                return (int)GetValue(initialPositionProperty);
            }
            set
            {
                SetValue(initialPositionProperty, value);
                OnPropertyChanged();
            }
        }

        private static BindableProperty isDismissableProperty = BindableProperty.Create(
            nameof(IsDismissable),
            typeof(bool),
            typeof(HubPageRelative),
            defaultValue: true,
            defaultBindingMode: BindingMode.TwoWay
        );

        public bool IsDismissable
        {
            get
            {
                return (bool)GetValue(isDismissableProperty);
            }
            set
            {
                SetValue(isDismissableProperty, value);
                OnPropertyChanged();
            }
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
            BindingContext = new HubContentViewModel();

            if (Webview is IBottomSheetChild child)
            {
                child.SetBottomSheetParent(this);
            }
        }

        public HubPageRelative(HubContentViewModel viewModel) : this()
        {
            BindingContext = viewModel;

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
                switch (e.StatusType)
                {
                    case GestureStatus.Running:
                        var translateY = Math.Max(Math.Min(0, currentPosition + e.TotalY), -Math.Abs((Height * .10) - Height));
                        await Sheet.TranslateTo(Sheet.X, translateY, 20, Easing.SpringOut);

                        break;

                    case GestureStatus.Completed:
                        currentPosition = Sheet.TranslationY;

                        if (!isSwipeUp(e) && Math.Abs(currentPosition) < InitialPosition)
                        {
                            await Dismiss();
                        }
                        else
                        {
                            // await Sheet.TranslateTo(Sheet.X, currentPosition, 150, Easing.SpringOut);
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
            // open sheet to 90% of the view
            var finalTranslation = Math.Max(Math.Min(0, -1000), -Math.Abs(GetProportionCoordinate(.90)));
            Sheet.TranslateTo(Sheet.X, finalTranslation, 150, Easing.SpringIn);
        }
        public async Task RequestHeight(int height)
        {
            var maxHeight = Math.Abs(GetProportionCoordinate(.90));
            if (height > maxHeight)
            {
                height = (int)maxHeight;
            }

            await AnimateTo(-height);
        }

        public async Task Dismiss()
        {
            if (!IsDismissable)
            {
                await Shake();
                return;
            }

            await AnimateOut();
            await Shared.app.MainPage.Navigation.PopModalAsync(false);
        }

        private double GetProportionCoordinate(double proportion)
        {
            return proportion * Height;
        }

        public async Task AnimateTo(double position, Easing easing = null)
        {
            easing ??= Easing.SpringOut;

            await Sheet.TranslateTo(Sheet.X, position, duration, easing);
            currentPosition = Sheet.TranslationY;
        }

        private async Task AnimateIn()
        {
            await Task.WhenAll(
                Backdrop.FadeTo(0.5, length: duration),
                Sheet.TranslateTo(0, -InitialPosition, length: duration, easing: Easing.SinOut),
                Sheet.FadeTo(1, duration, Easing.SinOut)
            );
            currentPosition = Sheet.TranslationY;
        }

        private async Task AnimateOut()
        {
            await Task.WhenAll(
                Sheet.TranslateTo(x: 0, y: 0, length: duration, easing: Easing.SinIn),
                Sheet.FadeTo(0, duration, Easing.SinIn),
                Backdrop.FadeTo(0, duration)
            );
            currentPosition = Sheet.TranslationY;
        }

        private void TapGestureRecognizer_Tapped(object sender, System.EventArgs e)
        {
            _ = Dismiss();
        }

        private async Task Shake()
        {
            uint timeout = 50;
            await Sheet.TranslateTo(0, currentPosition - 15, timeout);
            await Sheet.TranslateTo(0, currentPosition + 15, timeout);
            await Sheet.TranslateTo(0, currentPosition - 10, timeout);
            await Sheet.TranslateTo(0, currentPosition + 10, timeout);
            await Sheet.TranslateTo(0, currentPosition - 5, timeout);
            await Sheet.TranslateTo(0, currentPosition + 5, timeout);
            Sheet.TranslationY = currentPosition;
        }
    }
}
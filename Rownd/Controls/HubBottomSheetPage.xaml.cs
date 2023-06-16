using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Rownd.Core;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Rownd.Controls
{
    public partial class HubBottomSheetPage : ContentPage
    {
        #region Properties

        private static readonly BindableProperty InitialPositionProperty = BindableProperty.Create(
            nameof(InitialPosition),
            typeof(int),
            typeof(HubBottomSheetPage),
            defaultValue: 250,
            defaultBindingMode: BindingMode.OneTime);

        public int InitialPosition
        {
            get
            {
                return (int)GetValue(InitialPositionProperty);
            }
            set
            {
                SetValue(InitialPositionProperty, value);
                OnPropertyChanged();
            }
        }

        private static readonly BindableProperty IsDismissableProperty = BindableProperty.Create(
            nameof(IsDismissable),
            typeof(bool),
            typeof(HubBottomSheetPage),
            defaultValue: true,
            defaultBindingMode: BindingMode.TwoWay
        );

        public bool IsDismissable
        {
            get
            {
                return (bool)GetValue(IsDismissableProperty);
            }
            set
            {
                SetValue(IsDismissableProperty, value);
                OnPropertyChanged();
            }
        }

        private static readonly BindableProperty IsLoadingProperty = BindableProperty.Create(
            nameof(IsLoading),
            typeof(bool),
            typeof(HubBottomSheetPage),
            defaultValue: true,
            defaultBindingMode: BindingMode.OneWay
        );

        public bool IsLoading
        {
            get
            {
                return (bool)GetValue(IsLoadingProperty);
            }
            set
            {
                SetValue(IsLoadingProperty, value);
                OnPropertyChanged();
            }
        }

        private static readonly BindableProperty SheetBackgroundColorProperty = BindableProperty.Create(
            nameof(SheetBackgroundColor),
            typeof(Color),
            typeof(HubBottomSheetPage),
            defaultValue: Color.White,
            defaultBindingMode: BindingMode.OneWay
        );

        public Color SheetBackgroundColor
        {
            get
            {
                return (Color)GetValue(SheetBackgroundColorProperty);
            }
            set
            {
                SetValue(SheetBackgroundColorProperty, value);
                OnPropertyChanged();
            }
        }

        private static readonly BindableProperty PrimaryForegroundColorProperty = BindableProperty.Create(
            nameof(PrimaryForegroundColor),
            typeof(Color),
            typeof(HubBottomSheetPage),
            defaultValue: Color.FromHex("#333333"),
            defaultBindingMode: BindingMode.OneWay
        );

        public Color PrimaryForegroundColor
        {
            get
            {
                return (Color)GetValue(PrimaryForegroundColorProperty);
            }
            set
            {
                SetValue(PrimaryForegroundColorProperty, value);
                OnPropertyChanged();
            }
        }

        public ICommand TriggerExpand { get; }

        #endregion

        public HubBottomSheetPage()
        {
            OSAppTheme currentTheme = Shared.App.RequestedTheme;
            InitializeComponent();
            TriggerExpand = new Command(() => Expand());
            BindingContext = this;

            if (Webview is IBottomSheetChild child)
            {
                child.SetBottomSheetParent(this);
            }

            SheetBackgroundColor = Shared.ServiceProvider.GetService<Config>().Customizations.SheetBackgroundColor;
            PrimaryForegroundColor = Shared.ServiceProvider.GetService<Config>().Customizations.PrimaryForegroundColor;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            BackgroundColor = new Color(0, 0, 0, 0.01);
            _ = AnimateIn();
        }

        private readonly uint duration = 300;
        private double currentPosition = 0;

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

                        if (!IsSwipeUp(e) && Math.Abs(currentPosition) < InitialPosition)
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

        public bool IsSwipeUp(PanUpdatedEventArgs e)
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
            await Shared.App.MainPage.Navigation.PopModalAsync(false);
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
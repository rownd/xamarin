using System;
using Newtonsoft.Json;
using Rownd.Xamarin.Utils;
using Xamarin.Forms;

namespace Rownd.Xamarin.Core
{
    public class Customizations
    {
        [JsonConverter(typeof(JsonColorHexConverter))]
        public Color SheetBackgroundColor { get; set; }

        [JsonIgnore]
        public Color DynamicSheetBackgroundColor
        {
            get
            {
                OSAppTheme currentTheme = Application.Current.RequestedTheme;
                return SheetBackgroundColor != null
                    ? SheetBackgroundColor
                    : (currentTheme == OSAppTheme.Dark
                        ? Color.FromHex("#1c1c1e")
                        : Color.FromHex("#ffffff"));
            }
        }

        [JsonIgnore]
        public Color PrimaryForegroundColor { get; set; }

        public int SheetCornerBorderRadius { get; set; } = 12;

        public double DefaultFontSize { get; set; } = Device.GetNamedSize(NamedSize.Medium, typeof(Label)) * 0.6;

        public Customizations()
        {
            OSAppTheme currentTheme = Application.Current.RequestedTheme;

            switch (currentTheme)
            {
                case OSAppTheme.Unspecified:
                case OSAppTheme.Light:
                    {
                        SheetBackgroundColor = Color.White;
                        PrimaryForegroundColor = Color.FromHex("#222222");
                        break;
                    }

                case OSAppTheme.Dark:
                    {
                        SheetBackgroundColor = Color.FromHex("#333333");
                        PrimaryForegroundColor = Color.White;
                        break;
                    }
            }
        }
    }
}
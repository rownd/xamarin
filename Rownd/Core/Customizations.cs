using System;
using Newtonsoft.Json;
using Rownd.Utils;
using Xamarin.Forms;

namespace Rownd.Core
{
    public class Customizations
    {
        [JsonConverter(typeof(JsonColorHexConverter))]
        public Color SheetBackgroundColor { get; set; }

        [JsonIgnore]
        public Color PrimaryForegroundColor { get; set; }
        public int SheetCornerBorderRadius { get; set; } = 12;

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
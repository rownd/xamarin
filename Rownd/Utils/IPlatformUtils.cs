using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Rownd.Xamarin.Utils
{
    public interface IPlatformUtils
    {
        public Thickness GetWindowSafeArea();
        public double GetWindowHeight();
    }
}

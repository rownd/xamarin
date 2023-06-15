using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Rownd.Controls
{	
	public partial class HubContentView : ContentView
	{	
		public HubContentView ()
		{
			InitializeComponent ();
		}

        double x, y;
        void OnPanUpdated(object sender, PanUpdatedEventArgs e)
        {
            // Handle the pan
            switch (e.StatusType)
            {
                case GestureStatus.Running:
                    break;
                case GestureStatus.Completed:
                    y = bottomSheet.TranslationY;
                    break;
            }

        }

    }
}


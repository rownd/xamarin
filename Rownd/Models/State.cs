using System;
using Xamarin.CommunityToolkit.ObjectModel;

namespace Rownd.Models
{
	public class State : ObservableObject
	{
		public Auth AuthState
		{
			get;
			private set;
		}

		public State()
		{
			
		}


	}
}


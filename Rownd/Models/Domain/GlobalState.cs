using System;
using Xamarin.CommunityToolkit.ObjectModel;
using System.Reactive.Subjects;
using Xamarin.Forms;
using Rownd.Core;

namespace Rownd.Models.Domain
{
	public class GlobalState
	{
		public AppConfigState AppConfig { get; set; } = new AppConfigState();
		public AuthState Auth { get; set; } = new AuthState();
	}
}


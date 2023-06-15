using System;
using ReduxSimple;
using Rownd.Core;
using Rownd.Models.Domain;

namespace Rownd
{
	public interface IRowndInstance
	{
		void RequestSignIn();
		void RequestSignIn(SignInMethod with);
		void SignOut();
		String GetAccessToken();
		String GetAccessToken(String token);
		ReduxStore<GlobalState> Store { get; }
    }
}


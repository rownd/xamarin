using System;
using Rownd.Core;

namespace Rownd
{
	public interface IRowndInstance
	{
		void RequestSignIn();
		void RequestSignIn(SignInMethod with);
		String GetAccessToken();
		String GetAccessToken(String token);
	}
}


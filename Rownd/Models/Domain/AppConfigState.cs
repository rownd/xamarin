using System;
using System.Collections.Generic;

namespace Rownd.Models.Domain
{
	public class AppConfigState
	{
		public bool IsLoading = false;
		public String Id;
		public IList<String> UserVerificationFields = new List<String>();
	}
}


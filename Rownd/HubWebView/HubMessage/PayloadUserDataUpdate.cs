using System;
using System.Collections.Generic;

namespace Rownd.HubWebView.HubMessage
{
    public class PayloadUserDataUpdate : PayloadBase
    {
        public Dictionary<string, dynamic> Data { get; set; }

        public PayloadUserDataUpdate()
        {
        }
    }
}

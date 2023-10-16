﻿using System;
using System.Collections.Generic;

namespace Rownd.Xamarin.Hub.HubMessage
{
    public class PayloadUserDataUpdate : PayloadBase
    {
        public Dictionary<string, dynamic> Data { get; set; }

        public Dictionary<string, dynamic> Meta { get; set; } = null;
    }
}

﻿using System;
using Rownd.Xamarin.Models.Domain;

namespace Rownd.Xamarin.Models
{
    public class StateActions
    {
        public class SetGlobalState
        {
            public GlobalState GlobalState { get; set; }
        }

        public class SetAppConfig
        {
            public AppState AppConfig { get; set; }
        }

        public class SetAuthState
        {
            public AuthState AuthState { get; set; }
        }

        public class SetUserState
        {
            public UserState UserState { get; set; }
        }

        public class SetSignInState
        {
            public SignInState SignInState { get; set; }
        }
    }
}

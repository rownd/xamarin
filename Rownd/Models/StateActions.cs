using System;
using Rownd.Models.Domain;

namespace Rownd.Models
{
    public class StateActions
    {
        public class SetGlobalState
        {
            public GlobalState GlobalState { get; set; }
        }

        public class SetAppConfig
        {
            public AppConfigState AppConfig { get; set; }
        }

        public class SetAuthState
        {
            public AuthState AuthState { get; set; }
        }

        public class SetUserState
        {
            public UserState UserState { get; set; }
        }
    }
}

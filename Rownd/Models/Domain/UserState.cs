using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Rownd.Models.Repos;

namespace Rownd.Models.Domain
{
    public class UserState
    {
        [JsonIgnore]
        public bool IsLoading { get; set; }
        public Dictionary<string, dynamic> Data { get; set; } = new Dictionary<string, dynamic>();

        public dynamic Get(string field)
        {
            return Data[field];
        }

        public T Get<T>(string field)
        {
            if (Data[field] is T value)
            {
                return value;
            }

            return default;
        }

        public void Set(string field, dynamic value)
        {
            Data[field] = value;
            StateRepo.Get().Store.Dispatch(new StateActions.SetUserState
            {
                UserState = this
            });
        }

        public void Set(Dictionary<string, dynamic> data)
        {
            Data = data;
            StateRepo.Get().Store.Dispatch(new StateActions.SetUserState
            {
                UserState = this
            });
        }
    }
}

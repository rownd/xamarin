using System;
using System.Collections.Generic;

namespace Rownd.Models.Domain
{
    public class UserState
    {
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
            // TODO: Persist to state
        }

        public void Set(Dictionary<string, dynamic> data)
        {
            Data = data;
            // TODO: Persist to state
        }
    }
}

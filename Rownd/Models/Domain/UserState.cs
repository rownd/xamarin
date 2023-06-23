using System.Collections.Generic;
using Newtonsoft.Json;
using Rownd.Xamarin.Models.Repos;

namespace Rownd.Xamarin.Models.Domain
{
    public class UserState
    {
        [JsonIgnore]
        public bool IsLoading { get; set; }
        public Dictionary<string, dynamic> Data { get; set; } = new Dictionary<string, dynamic>();

        public Dictionary<string, dynamic> Get()
        {
            return new Dictionary<string, dynamic>(Data);
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
            _ = UserRepo.GetInstance().SaveUser(this);
        }

        public void Set(Dictionary<string, dynamic> data)
        {
            Data = new Dictionary<string, dynamic>(data);
            StateRepo.Get().Store.Dispatch(new StateActions.SetUserState
            {
                UserState = this
            });
            _ = UserRepo.GetInstance().SaveUser(this);
        }
    }
}

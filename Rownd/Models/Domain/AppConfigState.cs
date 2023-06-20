using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Rownd.Models.Domain
{
    public class AppConfigState
    {
        [JsonIgnore]
        public bool IsLoading { get; set; } = false;
        public string Id { get; set; }
        public IList<string> UserVerificationFields { get; set; } = new List<string>();
    }
}

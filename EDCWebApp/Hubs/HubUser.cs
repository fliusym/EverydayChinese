using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EDCWebApp.Hubs
{
    public class HubUser
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("isTeacher")]
        public bool IsTeacher
        {
            get;
            set;
        }
    }
}
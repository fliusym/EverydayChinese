using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace EDCWebApp.Hubs
{
    public class Position
    {
        [JsonProperty("xValue")]
        public double XValue { get; set; }
        [JsonProperty("yValue")]
        public double YValue { get; set; }
    }
    public class BlackBoard
    {
        [JsonProperty("previousPosition")]
        public Position PreviousPosition { get; set; }
        [JsonProperty("currentPosition")]
        public Position CurrentPosition { get; set; }
        [JsonProperty("eraseFlag")]
        public bool EraseFlag { get; set; }
        [JsonIgnore]
        public string LastUpdateBy { get; set; }
    }
}
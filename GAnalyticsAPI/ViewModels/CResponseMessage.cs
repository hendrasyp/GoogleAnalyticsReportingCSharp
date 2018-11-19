using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace GAnalyticsAPI.ViewModels
{
    public class CResponseMessage
    {
        public HttpStatusCode _CODE { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string _MESSAGE { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string _DESCRIPTION { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? _LENGTH { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IQueryable<Object> _RESULTs { get; set; }
    }
}
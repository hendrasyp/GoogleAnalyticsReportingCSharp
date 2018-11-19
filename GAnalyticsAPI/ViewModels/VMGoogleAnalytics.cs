using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GAnalyticsAPI.ViewModels
{

    [JsonObject(IsReference = true)]
    public class GAResult
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<ModelGAResult> USERs { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<ModelGAResult> SESSIONs { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<ModelGAResult> BOUNCERATEs { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<ModelGAResult> SESSIONDURATIONs { get; set; }
    }

    [JsonObject(IsReference = true)]
    public class VMGoogleAnalyticsRequest
    {
        public string START_DATE { get; set; }
        public string END_DATE { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class PointType
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        public PointType(string  v1, string v2)
        {
            this.DIMENSION = v1;
            this.VALUE = v2;
        }
        /// <summary>
        /// 
        /// </summary>
        public string DIMENSION { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string VALUE { get; set; }
    }


    /// <summary>
    /// 
    /// </summary>
    [JsonObject(IsReference = false)]
    public class ModelGAResult
    {
        /// <summary>
        /// 
        /// </summary>
        public string DIMENSION { get; set; }
        public string VALUE { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<PointType> RESULTs { get; set; }
    }

    /// <summary>
    /// Model Result
    /// </summary>
    [JsonObject(IsReference = false)]
    public class SimulationGAResult
    {
        /// <summary>
        /// 
        /// </summary>
        public List<ViewModels.ModelGAResult> RESULTs { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public SimulationGAResult()
        {
            RESULTs = new List<ViewModels.ModelGAResult>(); //create the list 
        }
    }
}
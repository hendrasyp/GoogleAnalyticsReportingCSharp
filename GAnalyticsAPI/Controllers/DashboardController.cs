using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using GAnalyticsAPI.ViewModels;
using System.Web;
using Google.Apis.Auth.OAuth2;
using System.IO;
using Google.Apis.AnalyticsReporting.v4;
using Google.Apis.Services;
using Google.Apis.AnalyticsReporting.v4.Data;
using System.Threading.Tasks;

namespace GAnalyticsAPI.Controllers
{
    public class DashboardController : ApiController
    {
        [NonAction]
        public IQueryable<ModelGAResult> GenerateData(string type, string START_DATE, string END_DATE)
        {
            string filepath = HttpContext.Current.Server.MapPath("~/App_Data/secret_key.json");

            #region Google Credentials
            GoogleCredential credential;
            using (var stream = new FileStream(filepath, FileMode.Open, FileAccess.Read))
            {
                string[] scopes = { AnalyticsReportingService.Scope.AnalyticsReadonly };
                var googleCredential = GoogleCredential.FromStream(stream);
                credential = googleCredential.CreateScoped(scopes);
            }
            #endregion

            SimulationGAResult resultList = new SimulationGAResult();
            // LOOP
            using (var svc = new AnalyticsReportingService(
                new BaseClientService.Initializer
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "Google Analytics Reporting API"
                }))
            {

                #region DateRange Report Analytics
                var dateRange = new DateRange
                {
                    StartDate = START_DATE,
                    EndDate = END_DATE
                };
                #endregion

                #region Metric Collections
                var SessionMetric = new Metric
                {
                    Expression = "ga:sessions",
                    Alias = "Sessions",
                    FormattingType = "INTEGER"
                };
                var UserMetric = new Metric
                {
                    Expression = "ga:users",
                    Alias = "Users",
                    FormattingType = "INTEGER"
                };
                var BounceRateMetric = new Metric
                {
                    Expression = "ga:bounceRate",
                    Alias = "Bounce Rate",
                    FormattingType = "PERCENT"
                };
                var SessionDurationMetric = new Metric
                {
                    Expression = "ga:sessionDuration",
                    Alias = "Session Durations",
                    FormattingType = "TIME"
                };
                #endregion

                #region Dimension Result
                Dimension DateDimension = new Dimension { Name = "ga:date" };
                Dimension BrowserDimension = new Dimension { Name = "ga:browser" };
                #endregion

                #region Metric Use
                Metric usedMetric = new Metric();
                switch (type)
                {
                    case "user":
                        usedMetric = UserMetric;
                        break;
                    case "session":
                        usedMetric = SessionMetric;
                        break;
                    case "bounce":
                        usedMetric = BounceRateMetric;
                        break;
                    case "duration":
                        usedMetric = SessionDurationMetric;
                        break;
                }

                var reportRequest = new ReportRequest
                {
                    DateRanges = new List<DateRange> { dateRange },
                    Dimensions = new List<Dimension> { DateDimension },
                    Metrics = new List<Metric> { usedMetric },
                    ViewId = "173657940"
                };
                #endregion

                List<ReportRequest> requests = new List<ReportRequest>();
                requests.Add(reportRequest);

                // Create the GetReportsRequest object.
                GetReportsRequest getReport = new GetReportsRequest() { ReportRequests = requests };

                // Call the batchGet method.
                GetReportsResponse response = null;
                response = svc.Reports.BatchGet(getReport).Execute();

                foreach (var item in response.Reports.First().Data.Rows)
                {
                    ModelGAResult mdlres = new ModelGAResult();
                    List<PointType> points = new List<PointType>();
                    for (int index = 0; index < item.Dimensions.Count; index++)
                    {
                        string tmpDate = item.Dimensions[index];
                        string year = tmpDate.Substring(0, 4);
                        string month = tmpDate.Substring(4, 2);
                        string date = tmpDate.Substring(6, 2);
                        mdlres.DIMENSION = year + "-" + month + "-" + date;
                        var MetricValues = string.Join(",", item.Metrics.First().Values);
                        var MetricValuesArray = MetricValues.Split(',');
                        for (var metricIndex = 0; metricIndex < MetricValuesArray.Length; metricIndex++)
                        {
                            mdlres.VALUE = MetricValuesArray[0];
                        }

                    }
                    resultList.RESULTs.Add(mdlres);
                }
            }
            return resultList.RESULTs.AsQueryable();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="GRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/dashboard/webtraffic")]
        public HttpResponseMessage WebTraffic([FromBody]VMGoogleAnalyticsRequest GRequest)
        {
            HttpResponseMessage response = null;
            CResponseMessage customResponse = new CResponseMessage();


            List<GAResult> dummy = new List<GAResult>();
            dummy.Add(new GAResult()
            {
                USERs = GenerateData("user", GRequest.START_DATE, GRequest.END_DATE).ToList(),
                SESSIONs = GenerateData("session", GRequest.START_DATE, GRequest.END_DATE).ToList(),
                BOUNCERATEs = GenerateData("bounce", GRequest.START_DATE, GRequest.END_DATE).ToList(),
                SESSIONDURATIONs = GenerateData("duration", GRequest.START_DATE, GRequest.END_DATE).ToList(),
            });
            customResponse._CODE = HttpStatusCode.OK;
            customResponse._RESULTs = dummy.AsQueryable();

            response = Request.CreateResponse(customResponse._CODE, customResponse);
            return response;


        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s">Start Date = yyyy-mm-dd</param>
        /// <param name="e">End Date = yyyy-mm-dd</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/dashboard/webtraffic")]
        public HttpResponseMessage WebTrafficGet(string s, string e)
        {
            HttpResponseMessage response = null;
            CResponseMessage customResponse = new CResponseMessage();


            List<GAResult> dummy = new List<GAResult>();
            dummy.Add(new GAResult()
            {
                USERs = GenerateData("user", s, e).ToList(),
                SESSIONs = GenerateData("session", s, e).ToList(),
                BOUNCERATEs = GenerateData("bounce", s, e).ToList(),
                SESSIONDURATIONs = GenerateData("duration", s, e).ToList(),
            });
            customResponse._CODE = HttpStatusCode.OK;
            customResponse._RESULTs = dummy.AsQueryable();

            response = Request.CreateResponse(customResponse._CODE, customResponse);
            return response;


        }
    }
}

//var dm = string.Join(",", x.Dimensions);
//retSTR = retSTR + string.Join(", ", x.Dimensions) + "   " + string.Join(", ", x.Metrics.First().Values);
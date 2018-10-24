using System.Net;
using System.Net.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using System.Text;

namespace Challenge01
{
    public static class GetRating
    {
        [FunctionName("GetRating")]
        public static HttpResponseMessage Run([HttpTrigger(AuthorizationLevel.Anonymous, "get","post",
            Route = null)] HttpRequestMessage req, [DocumentDB(databaseName: "esradb",
                collectionName: "test",
                ConnectionStringSetting = "Cosmosconnection",
                Id = "{Query.id}")] dynamic RatingItem,
            TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");

            if (RatingItem == null)
           
            {
                return req.CreateResponse(HttpStatusCode.BadRequest);
            }

            //return req.CreateResponse(HttpStatusCode.OK);
            //var test = JObject.Parse(RatingItem)["userId"];
            //return name == null
            //? req.CreateResponse(HttpStatusCode.BadRequest, "Please pass a name on the query string or in the request body")
            //var res = req.CreateResponse(HttpStatusCode.OK, "Hello ");
            // res.Content = new StringContent(RatingItem, Encoding.UTF8, "application/json");
            //return res;
            //var resp = new HttpResponseMessage { Content = new StringContent(RatingItem, System.Text.Encoding.UTF8, "application/json") };
            //return resp;
            var temp = Newtonsoft.Json.JsonConvert.SerializeObject(RatingItem);
             var response = req.CreateResponse(HttpStatusCode.OK);
             response.Content = new StringContent( temp, Encoding.UTF8, "application/json");
             return response;
             

        }
    }
}

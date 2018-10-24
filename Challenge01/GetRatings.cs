
using System.Net;
using System.Net.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using System.Text;
using System.Collections.Generic;

namespace Challenge01
{
    public static class GetRatings
    {
        [FunctionName("GetRatings")]
        public static HttpResponseMessage Run([HttpTrigger(AuthorizationLevel.Anonymous, "get","post",
            Route =  "GetRatings/userId/{userId}")] HttpRequestMessage req, [DocumentDB(databaseName: "esradb",
                collectionName: "test",
                ConnectionStringSetting = "Cosmosconnection",
                SqlQuery = "SELECT top 10 * FROM c WHERE c.userId =  {userId}")] IEnumerable<dynamic> RatingsItem,

        //SqlQuery = "{Query.userId}")] dynamic RatingsItem,
        TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");

            if (RatingsItem == null)

            {
                return req.CreateResponse(HttpStatusCode.BadRequest, "Item not found");
            }

                
                var temp = Newtonsoft.Json.JsonConvert.SerializeObject(RatingsItem);
                var response = req.CreateResponse(HttpStatusCode.OK);
                response.Content = new StringContent(temp, Encoding.UTF8, "application/json");
                return response; 
            

        }
    }
}

using System.Linq;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json.Linq;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Newtonsoft.Json;

namespace Challenge01
{
    public static class CreateRating
    {
        private static readonly HttpClient client = new HttpClient();

        [FunctionName("CreateRating")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]HttpRequestMessage req,
            [DocumentDB(
                databaseName: "esradb",
                collectionName: "test",
                ConnectionStringSetting = "Cosmosconnection")]
            IAsyncCollector<dynamic> toDoItemsOut, TraceWriter log)
        {
            
        log.Info("C# HTTP trigger function processed a request.");
                // Get request body
            dynamic data = await req.Content.ReadAsAsync<object>();

            try
            {
                var userId = data?.userId;
                var responseString = await client.GetStringAsync("https://serverlessohuser.trafficmanager.net/api/GetUser?userId=" + userId);
            }
            catch (Exception e)
            {
                 return req.CreateResponse(HttpStatusCode.BadRequest,"Please pass a valid product ID");
            }

            try
            {
                var productId = data?.productId;
                var responseString = await client.GetStringAsync("https://serverlessohproduct.trafficmanager.net/api/GetProduct?productId=" + productId);
            }
            catch (Exception e)
            {
                return req.CreateResponse(HttpStatusCode.BadRequest, "Please pass a valid product ID");
            }
            var rating = data?.rating;
            if ((rating <1 ) || (rating > 5))
                    {
                return req.CreateResponse(HttpStatusCode.BadRequest, "Not valid rating");
            }

            try
            {
                Guid id = Guid.NewGuid();
            }
            catch
            {
                return req.CreateResponse(HttpStatusCode.BadRequest, "Not Valid GUID");
            }
            
            //DateTime timestamp = DateTime.Now;

            dynamic jsonobject = new JObject();
            jsonobject.id = Guid.NewGuid();
            jsonobject.userId = data?.userId;
            jsonobject.productId = data?.productId;
            jsonobject.timestamp = DateTime.Now;
            jsonobject.locationName = data?.locationName;
            jsonobject.rating = data?.rating;
            jsonobject.userNoted = data?.userNotes;
           
            

            await toDoItemsOut.AddAsync(jsonobject);

            //await toDoItemsOut.AddAsync(productId);

            return req.CreateResponse(HttpStatusCode.OK, "Hello ");
            
        }
    
    }
}

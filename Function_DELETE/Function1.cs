using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents;

namespace Function_DELETE
{
    public class Auto
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("modelo")]
        public string Modelo { get; set; }
        [JsonProperty("color")]
        public string Color { get; set; }
    }
    //Entendible :)
    public class Response<T>
    {
        [JsonProperty("data")]
        public T Data { get; set; } //Lista return
        [JsonProperty("success")]
        public bool Success { get; set; }
        [JsonProperty("method")]
        public string Method { get; set; }
    }
    public static class Function1
    {
        [FunctionName("Function1")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "Autos/{id}")] HttpRequest req,
            string id,
            [CosmosDB(
                databaseName: "dbdistribuida",
                collectionName: "tabloide",
                ConnectionStringSetting = "strCosmos")]
                DocumentClient client,
            ILogger log)
        {
            await client.DeleteDocumentAsync(
                UriFactory.CreateDocumentUri("dbdistribuida", "tabloide", id),
                new RequestOptions() { PartitionKey = new PartitionKey(id) });
            var Response = new Response<string>
            {
                Data = id,
                Success = true,
                Method = "DELETE"
            };
            return new OkObjectResult(Response);
        }
    }
}

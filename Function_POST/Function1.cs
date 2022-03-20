using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Function_POST
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
            [HttpTrigger(AuthorizationLevel.Anonymous,"post", Route = "Autos/")] HttpRequest req,
            [CosmosDB(
                databaseName: "dbdistribuida",
                collectionName: "tabloide",
                ConnectionStringSetting = "strCosmos")]
                IAsyncCollector<Auto> itemsAutos,
            ILogger log)
        {

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var Auto = JsonConvert.DeserializeObject<Auto>(requestBody);
            await itemsAutos.AddAsync(Auto);
            var Response = new Response<Auto>
            {
                Data = Auto,
                Success = true,
                Method = "POST"
            };
            return new OkObjectResult(Response);
        }
    }
}

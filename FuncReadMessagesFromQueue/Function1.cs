using System;
using System.Net.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FuncReadMessagesFromQueue
{
    public class Function1
    {
        [FunctionName("Function1")]
        public void Run([QueueTrigger("weatherforecastmessage", Connection = "ConnStrName")]string myQueueItem, ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
            //throw new Exception("Not found");
            WeatherForcast weatherForcast = JsonConvert.DeserializeObject<WeatherForcast>(myQueueItem);
            HttpClient client = new HttpClient();
            using (var content=new StringContent(JsonConvert.SerializeObject(weatherForcast), System.Text.Encoding.UTF8, "application/json")) {
                HttpResponseMessage result = client.PostAsync("https://localhost:7038/WeatherForecast", content).Result;
                if(result.StatusCode==System.Net.HttpStatusCode.Created) {
                
                }
                string returnValue=result.Content.ReadAsStringAsync().Result;
            }
        }

        public class WeatherForcast
        {
            public DateTime Date { get; set;}
            public int TemperatureC { get; set; }
            public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
            public string? Summary { get; set;}
}
    }
}

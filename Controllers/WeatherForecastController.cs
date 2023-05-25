using Azure.Storage.Queues;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace SendMessageToQueue.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpPost]

        public Task Post([FromBody]WeatherForecast weatherForecast)
        {
            return Task.FromResult<string>("Message Recieved");  
        }

        [HttpPost("PostMessageToQueue")]

        public async Task PostMessage([FromBody] WeatherForecast weatherforecast)
        {
            var connectionstring = "defaultendpointsprotocol=https;accountname=teststoragemanju;accountkey=9igvkqs7mmjodleqyicv4kx19hey5npnkdvlm/tfncnjvzgaweg/dzpvfpzofpdgx4uuoweidjzd+ast9trjxw==;endpointsuffix=core.windows.net";
            var queuename = "manjuqueue";
            var queueclient = new QueueClient(connectionstring, queuename);
            string jsonweatherforecast = JsonSerializer.Serialize(weatherforecast);
            await queueclient.SendMessageAsync(jsonweatherforecast);
        }

        [HttpPost("readqueuemessage")]

        public async Task ReadQueueMessage()
        {

            var connectionstring = "defaultendpointsprotocol=https;accountname=teststoragemanju;accountkey=9igvkqs7mmjodleqyicv4kx19hey5npnkdvlm/tfncnjvzgaweg/dzpvfpzofpdgx4uuoweidjzd+ast9trjxw==;endpointsuffix=core.windows.net";
            var queuename = "manjuqueue";
            var queueclient = new QueueClient(connectionstring, queuename);
            var msg = await queueclient.ReceiveMessageAsync();
            if (msg != null && msg.Value != null)
            {
                var message = msg.Value.Body;
                var weatherdata = JsonSerializer.Deserialize<WeatherForecast>(message);

            }
        }
    }

}
using System.Linq;
using System.Net;
using System.Net.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using System;
using Microsoft.Azure.Devices;
using HerhalingsOefeningen;
using Newtonsoft.Json;
using System.Text;

namespace HerhalingsOefeningen
{


    public static class Led
    {
        [FunctionName("Led")]
        public static HttpResponseMessage LedRun([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "send/{device}/{sensor}/{status}")]HttpRequestMessage req, string device, string sensor, string status, TraceWriter log)
        {
            string connectionString = Environment.GetEnvironmentVariable("IoTHub");
            ServiceClient serviceClient;
            serviceClient = ServiceClient.CreateFromConnectionString(connectionString);
            PiMessage pim = new PiMessage()
            {
                Sensor = sensor,
                Command = status
            };
            var json = JsonConvert.SerializeObject(pim);
            var bytes = Encoding.ASCII.GetBytes(json);
            Message message = new Message(bytes);
            serviceClient.SendAsync(device, message);
            return req.CreateResponse(HttpStatusCode.OK);
        }
    }
}
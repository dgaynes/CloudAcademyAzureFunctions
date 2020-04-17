using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace AzureFunctionTimer
{
    public static class FunctionDurable
    {
        [FunctionName("FunctionDurable")]
        public static async Task<List<string>> RunOrchestrator([OrchestrationTrigger] IDurableOrchestrationContext context, 
            ILogger log)
        {
            log.LogInformation($"Orchestrator running");

            var outputs = new List<string>();
            int x = 0;

            outputs.Add(await context.CallActivityAsync<string>("FunctionDurable_Hello", "Tokyo"));

            x = await context.CallActivityAsync<int>("FunctionDurable_Add5", x);
            outputs.Add(x.ToString());

            x = await context.CallActivityAsync<int>("FunctionDurable_Add5", x);
            outputs.Add(x.ToString());

            outputs.Add(await context.CallActivityAsync<string>("FunctionDurable_Hello", "London"));

            return outputs;
        }

        [FunctionName("FunctionDurable_Hello")]
        public static string SayHello([ActivityTrigger] string name, ILogger log)
        {
            log.LogInformation($"Saying hello to {name}.");
            return $"Hello {name}!";
        }

        [FunctionName("FunctionDurable_Add5")]
        public static int Add5([ActivityTrigger] int value, ILogger log)
        {
            int x = 5;
            log.LogInformation($"Adding " + x.ToString() + "+" + value);
            return x + value;
        }

        [FunctionName("FunctionDurable_HttpStart")]
        public static async Task<HttpResponseMessage> HttpStart(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestMessage req,
            [DurableClient] IDurableOrchestrationClient starter,
            ILogger log)
        {
            // Function input comes from the request content.
            string instanceId = await starter.StartNewAsync("FunctionDurable", null);

            log.LogInformation($"Started orchestration with ID = '{instanceId}'.");

            return starter.CreateCheckStatusResponse(req, instanceId);
        }
    }
}
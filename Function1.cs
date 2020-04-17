using System;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Queue;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace AzureFunctionTimer
{
    [StorageAccount("AzureWebJobsStorage")]
    public static class Function1
    {
        [FunctionName("Function1")]

        public static void Run([TimerTrigger("0 */5 * * * *")]TimerInfo myTimer,
            [Queue("thequeue", Connection = "AzureWebJobsStorage")] out string QMessage,
            ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now.ToLongTimeString()}");
            QMessage = "Timer triggered";
          //  QMessage = DateTime.Now.ToLongTimeString().Replace(" ", "-") + ".jpg";
        }

    }
}
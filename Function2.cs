
using System;
using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace AzureFunctionTimer
{
    public static class Function2
    {
        [FunctionName("Function2")]
        public static void Run([QueueTrigger("thequeue", Connection = "AzureWebJobsStorage")]string myQueueItem,
            [Blob("theblobs/{queueTrigger}", FileAccess.Read, Connection = "AzureWebJobsStorage")] Stream myPic,
            ILogger log)
        {

            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
            if (myPic != null)
                if (myQueueItem.ToUpper().Contains("SECURITY"))
                    log.LogInformation($"Security Image found PicSize:" + myPic.Length + " bytes");
                else
                    log.LogInformation($"New Image found PicSize:" + myPic.Length + " bytes");

        }
    }
}

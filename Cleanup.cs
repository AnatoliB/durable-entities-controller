using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.DurableTask.Client;
using Microsoft.DurableTask.Entities;
using Microsoft.Extensions.Logging;

namespace Company.Function
{
    public class Cleanup
    {
        private readonly ILogger<Cleanup> _logger;

        public Cleanup(ILogger<Cleanup> logger)
        {
            _logger = logger;
        }

        [Function("Cleanup")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req,
            [DurableClient] DurableTaskClient client)
        {
            var entityId = new EntityInstanceId(nameof(CounterManager), "theCounterManager");
            await client.Entities.SignalEntityAsync(entityId, "Cleanup");
            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
            response.WriteString("Cleanup succeeded!");
            return response;
        }
    }
}

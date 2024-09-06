using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.DurableTask.Client;
using Microsoft.DurableTask.Entities;

public class IncrementCounter
{
    [Function("IncrementCounter")]
    public static async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req,
        [DurableClient] DurableTaskClient client)
    {
        var entityId = new EntityInstanceId(nameof(Counter), "myCounter");
        await client.Entities.SignalEntityAsync(entityId, "Add", 1);
        var response = req.CreateResponse(HttpStatusCode.OK);
        response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
        response.WriteString("Incremented counter!");
        return response;
    }
}

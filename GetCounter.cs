using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.DurableTask.Client;
using Microsoft.DurableTask.Entities;

public class GetCounter
{
    [Function("GetCounter")]
    public static async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req,
        [DurableClient] DurableTaskClient client)
    {
        var entityId = new EntityInstanceId(nameof(Counter), "myCounter");
        var entity = await client.Entities.GetEntityAsync<Counter>(entityId);
        if (entity is null)
        {
            return req.CreateResponse(HttpStatusCode.NotFound);
        }

        var response = req.CreateResponse(HttpStatusCode.OK);
        await response.WriteAsJsonAsync(entity);
        return response;
    }
}

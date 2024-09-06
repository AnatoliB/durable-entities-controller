using Microsoft.Azure.Functions.Worker;
using Microsoft.DurableTask.Entities;
using Newtonsoft.Json;

[JsonObject(MemberSerialization.OptIn)]
public class CounterManager
{
    public void Manage(int amount, TaskEntityContext context)
    {
        var entityId = new EntityInstanceId(nameof(Counter), "myCounter");
        for (int i = 0; i < amount; i++)
        {
            context.SignalEntity(entityId, "Add", 1);
        }
    }

    public void Cleanup(TaskEntityContext context)
    {
        var entityId = new EntityInstanceId(nameof(Counter), "myCounter");
        context.SignalEntity(entityId, "Delete");
    }

    [Function(nameof(CounterManager))]
    public static Task RunEntityAsync([EntityTrigger] TaskEntityDispatcher dispatcher)
        => dispatcher.DispatchAsync<CounterManager>();
}

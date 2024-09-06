using Microsoft.Azure.Functions.Worker;
using Newtonsoft.Json;

[JsonObject(MemberSerialization.OptIn)]
public class Counter
{
    [JsonProperty("value")]
    public int Value { get; set; }

    public void Add(int amount) => Value += amount;

    public Task Reset()
    {
        this.Value = 0;
        return Task.CompletedTask;
    }

    public Task<int> Get() => Task.FromResult(this.Value);

    [Function(nameof(Counter))]
    public static Task RunEntityAsync([EntityTrigger] TaskEntityDispatcher dispatcher)
        => dispatcher.DispatchAsync<Counter>();
}

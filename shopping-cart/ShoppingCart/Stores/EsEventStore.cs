using System.Text;
using System.Text.Json;
using EventStore.ClientAPI;
using ShoppingCart.Abstractions.Stores;

namespace ShoppingCart.Stores;

public class EsEventStore : IEventStore
{
    private readonly IConfiguration _configuration;

    public EsEventStore(IConfiguration configuration)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    private string GetConnectionString()
    {
        var eventStoreDbSection = _configuration.GetSection("EventStoreDb");
        var eventStoreUser = eventStoreDbSection.GetValue<string>("User");
        var eventStorePassword = eventStoreDbSection.GetValue<string>("Password");
        var eventStoreHost = eventStoreDbSection.GetValue<string>("Host");
        var eventStorePort = eventStoreDbSection.GetValue<string>("Port");
        
        return $"tcp://{eventStoreUser}:{eventStorePassword}@{eventStoreHost}{eventStorePort}";
    }

    public async Task<IEnumerable<Event>> GetEventsAsync(long firstEventSequenceNumber, long lastEventSequenceNumber)
    {
        var connectionSettings = ConnectionSettings.Create().DisableTls().Build();
        using var connection = EventStoreConnection.Create(connectionSettings, new Uri(GetConnectionString()));
        await connection.ConnectAsync();

        var result = await connection.ReadStreamEventsForwardAsync("ShoppingCart",
            firstEventSequenceNumber, count: Convert.ToInt32(lastEventSequenceNumber - firstEventSequenceNumber),
            resolveLinkTos: false);

        return result.Events
            .Select(evt => new
            {
                Content = Encoding.UTF8.GetString(evt.Event.Data),
                Metatada = JsonSerializer.Deserialize<EventMetadata>(evt.Event.Metadata, new JsonSerializerOptions{PropertyNameCaseInsensitive = true})
            })
            .Select((evt, index) => new Event
            {
                SequenceNumber = index + firstEventSequenceNumber,
                Name = evt.Metatada?.EventName,
                OccurredAt = evt.Metatada?.OccurredAt ?? DateTimeOffset.UtcNow,
                Content = evt.Content
            });
    }

    public async Task RaiseAsync(string eventName, object content)
    {
        var eventContent = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(content));
        var eventMetaData = new EventMetadata(DateTimeOffset.UtcNow, eventName);
        var eventMetaDataBytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(eventMetaData));
        var eventData = new EventData(Guid.NewGuid(), "ShoppingCartEvent", true, eventContent, eventMetaDataBytes);
        
        var connectionSettings = ConnectionSettings.Create().DisableTls().Build();
        using var connection = EventStoreConnection.Create(connectionSettings, new Uri(GetConnectionString()));
        await connection.ConnectAsync();
        await connection.AppendToStreamAsync("ShoppingCart", ExpectedVersion.Any, eventData);
    }
}
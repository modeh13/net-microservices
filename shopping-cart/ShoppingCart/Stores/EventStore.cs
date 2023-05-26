using System.Data.SqlClient;
using System.Text.Json;
using Dapper;
using ShoppingCart.Abstractions.Stores;

namespace ShoppingCart.Stores;

public class EventStore : IEventStore
{
    private readonly IConfiguration _configuration;

    private sealed class EventStoreDatabase
    {
        private readonly IDictionary<long, Event> Events;

        public EventStoreDatabase()
        {
            Events = new Dictionary<long, Event>();
        }
        
        public long NextSequenceNumber()
        {
            return Events.Keys.DefaultIfEmpty().Max() + 1;
        }

        public void Add(Event @event)
        {
            Events.Add(@event.SequenceNumber, @event);
        }

        public IEnumerable<Event> GetEvents(long firstEventSequenceNumber, long lastEventSequenceNumber)
        {
            return Events.Values
                .Where(evt => evt.SequenceNumber >= firstEventSequenceNumber && evt.SequenceNumber <= lastEventSequenceNumber)
                .OrderBy(evt => evt.SequenceNumber);
        }
    }

    private static readonly EventStoreDatabase Database = new();

    private const string SelectEventsSql =
        @"SELECT Id AS SequenceNumber, Name, OccurredAt, Content FROM dbo.EventStore WHERE Id >= @Start AND Id <= @End";

    private const string InsertEventSql = @"INSERT INTO dbo.EventStore (Name, OccurredAt, Content) VALUES (@Name, @OccurredAt, @Content)";
    
    public EventStore(IConfiguration configuration)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    private string? GetConnectionString()
    {
        return _configuration.GetConnectionString("ShoppingCartDb");
    }

    public async Task<IEnumerable<Event>> GetEventsAsync(long firstEventSequenceNumber, long lastEventSequenceNumber)
    {
        await using var sqlConnection = new SqlConnection(GetConnectionString());
        var events = await sqlConnection.QueryAsync<Event>(SelectEventsSql, new
        {
            Start = firstEventSequenceNumber,
            End = lastEventSequenceNumber
        });

        return events;
    }

    public async Task RaiseAsync(string eventName, object content)
    {
        await using var sqlConnection = new SqlConnection(GetConnectionString());
        _ = await sqlConnection.ExecuteAsync(InsertEventSql, new
        {
            Name = eventName,
            OccurredAt = DateTimeOffset.UtcNow,
            Content = JsonSerializer.Serialize(content)
        });

    }
}

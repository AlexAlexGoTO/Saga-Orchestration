using Contracts;
using Newtonsoft.Json;
using Rebus.Extensions;
using Rebus.Messages;
using Rebus.Serialization;
using System.Collections.Concurrent;
using System.Text;

namespace Orchestrator.Infrastructure.Messaging;

public class MessageHandlers
{
    internal static ConcurrentDictionary<string, Type> KnownTypes = new ConcurrentDictionary<string, Type>();

    public static void MapHandlers()
    {
        KnownTypes.TryAdd("CreateOrderCommand, Gateway.Api", typeof(CreateOrderCommand));
    }
}

class CustomMessageDeserializer : ISerializer
{
    /// <summary>
    /// If the type name found in the '<see cref="Headers.Type"/>' header can be found in this dictionary, the incoming
    /// message will be deserialized into the specified type
    /// </summary>

    readonly ISerializer _serializer;

    public CustomMessageDeserializer(ISerializer serializer) => _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));

    public Task<TransportMessage> Serialize(Message message) => _serializer.Serialize(message);

    public async Task<Message> Deserialize(TransportMessage transportMessage)
    {
        var headers = transportMessage.Headers.Clone();
        var json = Encoding.UTF8.GetString(transportMessage.Body);
        var typeName = headers.GetValue(Headers.Type);

        // if we don't know the type, just deserialize the message into a JObject
        if (!MessageHandlers.KnownTypes.TryGetValue(typeName, out var type))
        {
            return await _serializer.Deserialize(transportMessage);
        }

        var body = JsonConvert.DeserializeObject(json, type);

        return new Message(headers, body);
    }
}

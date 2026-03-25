namespace FrostTrace.Api.Infrastructure.Messaging;

/// <summary>
/// Interface for publishing domain events to RabbitMQ.
/// FrostTrace.Ingestor and FrostTrace.Worker consume from these exchanges.
/// </summary>
public interface IRabbitMqPublisher
{
    Task PublishAsync<T>(string routingKey, T message, CancellationToken cancellationToken = default);
}

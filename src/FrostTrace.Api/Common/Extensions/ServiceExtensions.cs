using FluentValidation;
using FrostTrace.Api.Features.Batches.CreateBatch;
using FrostTrace.Api.Infrastructure.Messaging;
using FrostTrace.Api.Infrastructure.Persistence;
using MongoDB.Driver;

namespace FrostTrace.Api.Common.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddMongoDb(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IMongoClient>(_ =>
            new MongoClient(configuration["MongoDB:ConnectionString"] ?? "mongodb://localhost:27017"));

        services.AddSingleton<MongoDbContext>();
        return services;
    }

    public static IServiceCollection AddRabbitMq(this IServiceCollection services)
    {
        services.AddSingleton<IRabbitMqPublisher, RabbitMqPublisher>();
        return services;
    }

    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // MediatR — auto-discovers all IRequestHandler<,> in this assembly
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<CreateBatchHandler>());

        // FluentValidation — auto-discovers all AbstractValidator<> in this assembly
        services.AddValidatorsFromAssemblyContaining<CreateBatchValidator>();

        return services;
    }
}

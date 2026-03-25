using FrostTrace.Api.Common.Extensions;
using FrostTrace.Api.Common.Middleware;
using FrostTrace.Api.Features.Batches.CreateBatch;
using FrostTrace.Api.Features.Batches.GetBatch;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    // ─── Logging ─────────────────────────────────────────────────────────────
    builder.Host.UseSerilog((ctx, lc) => lc
        .ReadFrom.Configuration(ctx.Configuration)
        .WriteTo.Console());

    // ─── Services ────────────────────────────────────────────────────────────
    builder.Services.AddMongoDb(builder.Configuration);
    builder.Services.AddRabbitMq();
    builder.Services.AddApplicationServices();

    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new() { Title = "FrostTrace.Api", Version = "v1" });
        c.CustomSchemaIds(type => type.FullName);
    });

    // ─── Build ───────────────────────────────────────────────────────────────
    var app = builder.Build();

    // ─── Middleware Pipeline ──────────────────────────────────────────────────
    app.UseMiddleware<GlobalExceptionMiddleware>();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "FrostTrace.Api v1");
        });
    }

    // ─── Controller Registration ──────────────────────────────────────────────
    app.MapGet("/", () => Results.Redirect("/swagger"));
    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "FrostTrace.Api terminated unexpectedly.");
}
finally
{
    Log.CloseAndFlush();
}

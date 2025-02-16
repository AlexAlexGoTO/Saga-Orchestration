using Rebus.Bus;
using Rebus.Config;
using Rebus.Routing.TypeBased;
using Rebus.Serialization.Json;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddRebus(rebus => rebus
    .Routing(r =>
            r.TypeBased().Map<CreateOrderCommand>("orchestrator-queue"))
    .Transport(t =>
        t.UseRabbitMq(
            "amqp://guest:guest@localhost:5672/",
            "gateway-queue"))
    .Serialization(s => s.UseNewtonsoftJson(JsonInteroperabilityMode.PureJson)));

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapPost("/api/orders", async (IBus bus) =>
{
    await bus.Send(new CreateOrderCommand(Guid.NewGuid()));

    return Results.Ok(new { Message = "Order request sent to queue" });
});

app.UseHttpsRedirection();

app.MapReverseProxy();

app.Run();

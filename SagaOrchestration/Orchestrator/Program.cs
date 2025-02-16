using Contracts;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Orchestrator.Application.Orders;
using Orchestrator.Infrastructure.Messaging;
using Rebus.Bus;
using Rebus.Config;
using Rebus.Extensions;
using Rebus.Messages;
using Rebus.Persistence.InMem;
using Rebus.Routing.TypeBased;
using Rebus.Serialization;
using Rebus.Serialization.Json;
using System.Collections.Concurrent;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddRebus(rebus => rebus
    .Routing(r =>
                r.TypeBased()
                    .Map<ProcessOrderCommand>("order-queue")
                    .Map<ReserveStockCommand>("stock-queue")
                    .Map<PaymentRequestCommand>("payment-queue"))
    .Transport(t =>
        t.UseRabbitMq(
            "amqp://guest:guest@localhost:5672/",
            "orchestrator-queue"))
    .Sagas(s =>
        s.StoreInMemory())
    .Serialization(s => s.UseNewtonsoftJson(JsonInteroperabilityMode.PureJson))
                    .Options(o => o.Decorate<ISerializer>(c => new CustomMessageDeserializer(c.Get<ISerializer>()))));

//mapping cross assembly messages because rebus is strong typed.
MessageHandlers.MapHandlers();

builder.Services.AutoRegisterHandlersFromAssemblyOf<OrderCreateSaga>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();


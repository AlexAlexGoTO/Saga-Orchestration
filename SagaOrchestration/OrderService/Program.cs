using Contracts;
using OrderService.Application.Order;
using Rebus.Config;
using Rebus.Routing.TypeBased;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddRebus(rebus => rebus
    .Routing(r =>
        r.TypeBased().Map<OrderCreatedEvent>("orchestrator-queue"))
    .Transport(t =>
        t.UseRabbitMq(
            builder.Configuration.GetConnectionString("MessageBroker"),
            "order-queue")));

builder.Services.AutoRegisterHandlersFromAssemblyOf<ProcessOrderCommandHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.Run();

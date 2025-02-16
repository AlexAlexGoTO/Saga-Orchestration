using Contracts;
using Rebus.Config;
using Rebus.Routing.TypeBased;
using StockService.Application.Stock;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddRebus(rebus => rebus
    .Routing(r =>
        r.TypeBased()
            .Map<StockReservedEvent>("orchestrator-queue")
            .Map<ReserveStockFailedEvent>("orchestrator-queue"))
    .Transport(t =>
        t.UseRabbitMq(
            builder.Configuration.GetConnectionString("MessageBroker"),
            "stock-queue")));

builder.Services.AutoRegisterHandlersFromAssemblyOf<ReserveStockCommandHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();

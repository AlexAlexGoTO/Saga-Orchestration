using Contracts;
using PaymentService.Application.Payment;
using Rebus.Config;
using Rebus.Routing.TypeBased;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddRebus(rebus => rebus
    .Routing(r =>
        r.TypeBased()
            .Map<PaymentRequestSentEvent>("orchestrator-queue")
            .Map<PaymentRequestFailedEvent>("orchestrator-queue"))
    .Transport(t =>
        t.UseRabbitMq(
            builder.Configuration.GetConnectionString("MessageBroker"),
            "payment-queue")));

builder.Services.AutoRegisterHandlersFromAssemblyOf<PaymentRequestCommandHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();

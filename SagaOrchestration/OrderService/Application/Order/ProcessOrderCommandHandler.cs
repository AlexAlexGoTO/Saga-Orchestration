using Contracts;
using Rebus.Bus;
using Rebus.Handlers;

namespace OrderService.Application.Order
{
    

    public class ProcessOrderCommandHandler : IHandleMessages<ProcessOrderCommand>
    {
        private readonly ILogger<ProcessOrderCommand> _logger;

        private readonly IBus _bus;

        public ProcessOrderCommandHandler(ILogger<ProcessOrderCommand> logger, IBus bus)
        {
            _logger = logger;
            _bus = bus;
        }

        public async Task Handle(ProcessOrderCommand message)
        {
            _logger.LogInformation("Create order {@OrderId}", message.OrderId);

            await Task.Delay(2000);

            _logger.LogInformation("Order created {@OrderId}", message.OrderId);

            await _bus.Send(new OrderCreatedEvent(message.OrderId));
        }
    }
}

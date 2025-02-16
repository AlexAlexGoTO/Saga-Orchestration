using Contracts;
using Rebus.Bus;
using Rebus.Handlers;

namespace StockService.Application.Stock
{
    public class ReserveStockCommandHandler : IHandleMessages<ReserveStockCommand>
    {
        private readonly ILogger<ReserveStockCommand> _logger;

        private readonly IBus _bus;

        public ReserveStockCommandHandler(ILogger<ReserveStockCommand> logger, IBus bus)
        {
            _logger = logger;
            _bus = bus;
        }

        public async Task Handle(ReserveStockCommand message)
        {
            try
            {
                _logger.LogInformation("Reserve stock {@OrderId}", message.OrderId);

                await Task.Delay(2000);

                _logger.LogInformation("Stock reserved {@OrderId}", message.OrderId);

                await _bus.Send(new StockReservedEvent(message.OrderId));
            }
            catch (Exception e)
            {
                _logger.LogInformation("Reserve stock failed {@OrderId}", message.OrderId);

                await _bus.Send(new ReserveStockFailedEvent(message.OrderId));
            }
        }
    }
}

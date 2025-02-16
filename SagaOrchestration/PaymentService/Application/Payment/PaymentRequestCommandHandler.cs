using Contracts;
using Rebus.Bus;
using Rebus.Handlers;

namespace PaymentService.Application.Payment
{
    public class PaymentRequestCommandHandler : IHandleMessages<PaymentRequestCommand>
    {
        private readonly ILogger<PaymentRequestCommand> _logger;

        private readonly IBus _bus;

        public PaymentRequestCommandHandler(ILogger<PaymentRequestCommand> logger, IBus bus)
        {
            _logger = logger;
            _bus = bus;
        }

        public async Task Handle(PaymentRequestCommand message)
        {
            try
            {
                _logger.LogInformation("Send payment request {@OrderId}", message.OrderId);

                await Task.Delay(2000);

                _logger.LogInformation("Payment request sent {@OrderId}", message.OrderId);

                await _bus.Send(new PaymentRequestSentEvent(message.OrderId));
            }
            catch(Exception e)
            {
                _logger.LogInformation("Send payment request failed {@OrderId}", message.OrderId);

                await _bus.Send(new PaymentRequestFailedEvent(message.OrderId));
            }
        }
    }
}

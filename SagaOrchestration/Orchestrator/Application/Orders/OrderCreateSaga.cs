using Contracts;
using Rebus.Bus;
using Rebus.Handlers;
using Rebus.Sagas;

namespace Orchestrator.Application.Orders
{
    public class OrderCreateSaga : Saga<OrderCreateSagaData>,
        IAmInitiatedBy<CreateOrderCommand>,
        IHandleMessages<OrderCreatedEvent>,
        IHandleMessages<StockReservedEvent>,
        IHandleMessages<PaymentRequestSentEvent>,
        IHandleMessages<ReserveStockFailedEvent>,
        IHandleMessages<PaymentRequestFailedEvent>
    {
        private readonly IBus _bus;

        public OrderCreateSaga(IBus bus)
        {
            _bus = bus;
        }

        protected override void CorrelateMessages(ICorrelationConfig<OrderCreateSagaData> config)
        {
            config.Correlate<CreateOrderCommand>(m => m.OrderId, s => s.OrderId);
            config.Correlate<OrderCreatedEvent>(m => m.OrderId, s => s.OrderId);
            config.Correlate<StockReservedEvent>(m => m.OrderId, s => s.OrderId);
            config.Correlate<PaymentRequestSentEvent>(m => m.OrderId, s => s.OrderId);
            config.Correlate<ReserveStockFailedEvent>(m => m.OrderId, s => s.OrderId);
            config.Correlate<PaymentRequestFailedEvent>(m => m.OrderId, s => s.OrderId);
        }

        public async Task Handle(CreateOrderCommand message)
        {
            if (!IsNew)
            {
                return; //duplicate message
            }
            
            await _bus.Send(new ProcessOrderCommand(message.OrderId));
        }

        public async Task Handle(OrderCreatedEvent message)
        {
            Data.OrderCreated = true;

            await _bus.Send(new ReserveStockCommand(message.OrderId));
        }

        public async Task Handle(StockReservedEvent message)
        {
            Data.StockReserved = true;

            await _bus.Send(new PaymentRequestCommand(message.OrderId));
        }

        public Task Handle(PaymentRequestSentEvent message)
        {
            Data.PaymentRequestSent = true;

            MarkAsComplete();

            return Task.CompletedTask;
        }

        public Task Handle(ReserveStockFailedEvent message)
        {
            if (Data.OrderCreated)
            {
                //Compensating transaction, now we should send comand to cancel our order
                //OrderService should consume CancelOrderCommand in Handler and execute rollback for previously created order

                //await _bus.Send(new CancelOrderCommand(message.OrderId));
            }

            return Task.CompletedTask;
        }

        public Task Handle(PaymentRequestFailedEvent message)
        {
            if (Data.StockReserved)
            {
                //Compensating transaction, now we should send comand to release order stock
                //StockService should consume ReleaseOrderStockCommand in Handler and execute rollback for previously reserved stock

                //await _bus.Send(new ReleaseOrderStockCommand(message.OrderId));
            }

            return Task.CompletedTask;
        }
    }
}

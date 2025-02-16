using Contracts;
using Rebus.Sagas;

namespace Orchestrator.Application.Orders
{
    public class OrderCreateSagaData : ISagaData
    {
        public Guid Id { get; set; }
        public int Revision { get; set; }

        public Guid OrderId { get; set; }

        public bool OrderCreated { get; set; }
        public bool StockReserved { get; set; }
        public bool PaymentRequestSent { get; set; }

    }
}

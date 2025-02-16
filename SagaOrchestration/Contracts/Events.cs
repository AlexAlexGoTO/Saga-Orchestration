namespace Contracts;

//Shared dependency for typed rebus communicatin (for simplified example, I would avoid to do it in real project)

public record StockReservedEvent(Guid OrderId);
public record OrderCreatedEvent(Guid OrderId);
public record PaymentRequestSentEvent(Guid OrderId);
public record ReserveStockFailedEvent(Guid OrderId);
public record PaymentRequestFailedEvent(Guid OrderId);

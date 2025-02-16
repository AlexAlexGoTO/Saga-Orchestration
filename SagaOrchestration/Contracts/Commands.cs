namespace Contracts;

//Shared dependency for typed rebus communicatin (for simplified example, I would avoid to do it in real project)

public record CreateOrderCommand(Guid OrderId);
public record ReserveStockCommand(Guid OrderId);
public record ProcessOrderCommand(Guid OrderId);
public record PaymentRequestCommand(Guid OrderId);

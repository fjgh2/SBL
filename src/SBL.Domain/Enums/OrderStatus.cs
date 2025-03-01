namespace SBL.Domain.Enums;

public enum OrderStatus
{
    Created = 0,
    PendingPayment = 1,
    PaymentReceived = 2,
    PaymentFailed = 3,
    Processing = 4,
    Shipped = 5,
    Delivered = 6, 
}

namespace OrderProcessing.Orders
{
    public enum OrderStatus
    {
        WaitingForPayment=0,
        PaymentProcessing,
        ReadyForShipment
    }
}
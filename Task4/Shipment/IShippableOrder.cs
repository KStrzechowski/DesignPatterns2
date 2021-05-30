using OrderProcessing.Orders;

namespace OrderProcessing.Shipment
{
    public interface IShippableOrder
    {
        int OrderId { get; }
        Address Recipient { get; }
        decimal PaidAmount { get; }
    }
}
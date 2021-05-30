using OrderProcessing.Payments;

namespace OrderProcessing.Orders
{
    public static class OrderExtensions
    {
        public static Order SetPayment(this Order order, PaymentMethod method, decimal maxAvailableAmount)
        {
            order.SelectedPayments.Add(new Payment(method, maxAvailableAmount));
            return order;
        }
    }
}

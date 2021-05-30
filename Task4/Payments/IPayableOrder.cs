using System.Collections.Generic;
using OrderProcessing.Orders;

namespace OrderProcessing.Payments
{
    public interface IPayableOrder
    {
        int OrderId { get; }
        decimal DueAmount { get; }
        decimal PaidAmount { get; }
        OrderStatus Status { get; set; }
        ICollection<Payment> SelectedPayments { get; }
        ICollection<Payment> FinalizedPayments { get; }
    }
}
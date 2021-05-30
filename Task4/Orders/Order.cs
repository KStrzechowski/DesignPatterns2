using System.Collections.Generic;
using System.Linq;
using OrderProcessing.Payments;
using OrderProcessing.Shipment;

namespace OrderProcessing.Orders
{
    public class Order : IPayableOrder, IShippableOrder
    {
        public Order(int orderId, IEnumerable<LineItem> items)
        {
            OrderId = orderId;
            Status = OrderStatus.WaitingForPayment;
            Items = new List<LineItem>(items);
            SelectedPayments = new List<Payment>();
            FinalizedPayments = new List<Payment>();
        }

        public int OrderId { get; }

        public IEnumerable<LineItem> Items { get; }

        public OrderStatus Status { get; set; }

        public ICollection<Payment> SelectedPayments { get; set; } 

        public ICollection<Payment> FinalizedPayments { get; set; }
        
        public Address Recipient { get; set; }
        
        public decimal AmountToBePaid => Items.Sum(s => s.TotalPrice);

        public decimal PaidAmount => FinalizedPayments.Sum(s => s.Amount);

        public decimal DueAmount => AmountToBePaid - PaidAmount;
    }
}
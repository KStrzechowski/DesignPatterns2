#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderProcessing.Orders;

namespace OrderProcessing.Payments
{
    class CreditCardPaymentHandler : BasePaymentHandler
    {
        public override void Handle(Order order)
        {
            var payments = order.SelectedPayments
                                    .Where((x, y) => (x.PaymentType == PaymentMethod.CreditCard));

            foreach (var payment in payments)
            {
                if (order.DueAmount > 0)
                {
                    order.Status = OrderStatus.PaymentProcessing;
                    if (order.DueAmount <= payment.Amount)
                    {
                        payment.Amount = order.DueAmount;
                    }

                    Console.WriteLine($"Order {order.OrderId} paid {payment.Amount} via CreditCard");
                    order.FinalizedPayments.Add(payment);
                }
            }

            if (order.DueAmount > 0)
            {
                base.Handle(order);
            }
        }
    }
}

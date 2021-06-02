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
        public override object? Handle(Order order)
        {
            Payment? payment = order.SelectedPayments
                                    .Where((x, y) => (x.PaymentType == PaymentMethod.CreditCard))
                                    .FirstOrDefault();

            if (payment != null)
            {
                order.Status = OrderStatus.PaymentProcessing;
                if (order.DueAmount <= payment.Amount)
                {
                    payment.Amount = order.DueAmount;
                    Console.WriteLine($"Order {order.OrderId} paid {payment.Amount} via CreditCard");
                    order.FinalizedPayments.Add(payment);
                    return null;
                }
                else
                {
                    Console.WriteLine($"Order {order.OrderId} paid {payment.Amount} via CreditCard");
                    order.FinalizedPayments.Add(payment);
                    return base.Handle(order);
                }
            }
            else
            {
                return base.Handle(order);
            }
        }
    }
}

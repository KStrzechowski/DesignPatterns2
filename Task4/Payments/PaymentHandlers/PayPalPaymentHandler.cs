#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderProcessing.Orders;

namespace OrderProcessing.Payments
{
    class PayPalPaymentHandler : BasePaymentHandler
    {
        public override object? Handle(Order order)
        {
            Payment? payment = order.SelectedPayments
                                    .Where((x, y) => (x.PaymentType == PaymentMethod.PayPal))
                                    .FirstOrDefault();

            if (payment != null)
            {
                var random = new Random(1234);
                if (random.Next(1, 10) > 3)
                {
                    order.Status = OrderStatus.PaymentProcessing;
                    if (order.DueAmount <= payment.Amount)
                    {
                        payment.Amount = order.DueAmount;
                        Console.WriteLine($"Order {order.OrderId} paid {payment.Amount} via PayPal");
                        order.FinalizedPayments.Add(payment);
                        return null;
                    }
                    else
                    {
                        Console.WriteLine($"Order {order.OrderId} paid {payment.Amount} via PayPal");
                        order.FinalizedPayments.Add(payment);
                        return base.Handle(order);
                    }
                }
                else
                {
                    Console.WriteLine($"Order {order.OrderId} payment Paypal has failed");
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

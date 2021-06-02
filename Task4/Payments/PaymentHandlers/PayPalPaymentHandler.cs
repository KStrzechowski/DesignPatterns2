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
        public override void Handle(Order order)
        {
            var payments = order.SelectedPayments
                                    .Where((x, y) => (x.PaymentType == PaymentMethod.PayPal));

            foreach (var payment in payments)
            {
                if (order.DueAmount > 0)
                {
                    var random = new Random(1234);
                    if (random.Next(1, 10) > 3)
                    {
                        order.Status = OrderStatus.PaymentProcessing;
                        if (order.DueAmount <= payment.Amount)
                        {
                            payment.Amount = order.DueAmount;
                        }

                        Console.WriteLine($"Order {order.OrderId} paid {payment.Amount} via PayPal");
                        order.FinalizedPayments.Add(payment);
                    }
                    else
                    {
                        Console.WriteLine($"Order {order.OrderId} payment Paypal has failed");
                    }
                }
            }

            if (order.DueAmount > 0)
            {
                base.Handle(order);
            }
        }
    }
}

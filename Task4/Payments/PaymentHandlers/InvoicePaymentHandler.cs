#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderProcessing.Orders;

namespace OrderProcessing.Payments
{
    class InvoicePaymentHandler : BasePaymentHandler
    {
        private static int _paymentCount = 0;
        public override object? Handle(Order order)
        {
            Payment? payment = order.SelectedPayments
                                    .Where((x, y) => (x.PaymentType == PaymentMethod.Invoice))
                                    .FirstOrDefault();

            if (payment != null)
            {
                _paymentCount++;
                if (_paymentCount % 3 != 0)
                {
                    order.Status = OrderStatus.PaymentProcessing;
                    if (order.DueAmount <= payment.Amount)
                    {
                        payment.Amount = order.DueAmount;
                        Console.WriteLine($"Order {order.OrderId} paid {payment.Amount} via Invoice");
                        order.FinalizedPayments.Add(payment);
                        return null;
                    }
                    else
                    {
                        Console.WriteLine($"Order {order.OrderId} paid {payment.Amount} via Invoice");
                        order.FinalizedPayments.Add(payment);
                        return base.Handle(order);
                    }
                }
                else
                {
                    Console.WriteLine($"Order {order.OrderId} payment Invoice has failed");
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

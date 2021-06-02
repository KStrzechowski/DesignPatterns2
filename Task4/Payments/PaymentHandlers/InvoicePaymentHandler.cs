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
        public override void Handle(Order order)
        {
            var payments = order.SelectedPayments
                                    .Where((x, y) => (x.PaymentType == PaymentMethod.Invoice));

            foreach(var payment in payments)
            {
                if (order.DueAmount > 0)
                {
                    _paymentCount++;
                    if (_paymentCount % 3 != 0)
                    {
                        order.Status = OrderStatus.PaymentProcessing;
                        if (order.DueAmount <= payment.Amount)
                        {
                            payment.Amount = order.DueAmount;
                        }

                        Console.WriteLine($"Order {order.OrderId} paid {payment.Amount} via Invoice");
                        order.FinalizedPayments.Add(payment);
                    }
                    else
                    {
                        Console.WriteLine($"Order {order.OrderId} payment Invoice has failed");
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

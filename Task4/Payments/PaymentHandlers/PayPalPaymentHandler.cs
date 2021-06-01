using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderProcessing.Orders;

namespace OrderProcessing.Payments
{
    class PayPalPaymentHandler : PaymentHandler
    {
        public override object Handle(Order request)
        {
            // Jak to zmienić??
            //var item = request.SelectedPayments.Where((x, y) => (x.PaymentType == PaymentMethod.PayPal)).FirstOrDefault();

            foreach (var Payment in request.SelectedPayments)
            {
                if (Payment.PaymentType == PaymentMethod.PayPal)
                {
                    var random = new Random(1234);
                    if (random.Next(1, 10) > 3)
                    {
                        request.FinalizedPayments.Add(Payment);
                        Console.WriteLine($"Order {request.OrderId} payment Paypal was succesfull");
                        if (request.DueAmount <= 0)
                        {
                            request.Status = OrderStatus.PaymentProcessing;
                            return base.Handle(request);
                        }
                        else
                        {
                            request.Status = OrderStatus.ReadyForShipment;
                            return base.Handle(request);
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Order {request.OrderId} payment Paypal has failed");
                        return base.Handle(request);
                    }
                }
            }

            return base.Handle(request);
        }
    }
}

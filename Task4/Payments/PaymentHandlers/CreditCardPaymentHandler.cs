using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderProcessing.Orders;

namespace OrderProcessing.Payments
{
    class CreditCardPaymentHandler : PaymentHandler
    {
        public override object Handle(Order request)
        {

                return base.Handle(request);

        }
    }
}

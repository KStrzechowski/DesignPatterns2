#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderProcessing.Orders;

namespace OrderProcessing.Payments
{
    interface IPaymentHandler
    {
        IPaymentHandler? SetNext(IPaymentHandler? handler);

        void Handle(Order order);
    }
}

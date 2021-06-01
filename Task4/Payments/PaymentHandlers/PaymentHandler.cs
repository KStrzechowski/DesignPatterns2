using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderProcessing.Orders;

namespace OrderProcessing.Payments
{
    abstract class PaymentHandler : IPaymentHandler
    {
        private IPaymentHandler _nextHandler;

        public IPaymentHandler SetNext(IPaymentHandler handler)
        {
            this._nextHandler = handler;
            return handler;
        }

        public virtual object Handle(Order request)
        {
            if (this._nextHandler != null)
            {
                return this._nextHandler.Handle(request);
            }
            else
            {
                return null;
            }
        }
    }
}

#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderProcessing.Orders;

namespace OrderProcessing.Payments
{
    abstract class BasePaymentHandler : IPaymentHandler
    {
        private IPaymentHandler? _nextHandler;

        public IPaymentHandler? SetNext(IPaymentHandler? handler)
        {
            _nextHandler = handler;
            return handler;
        }

        public virtual object? Handle(Order order)
        {
            if (_nextHandler != null)
            {
                return _nextHandler.Handle(order);
            }
            else
            {
                return null;
            }
        }
    }
}

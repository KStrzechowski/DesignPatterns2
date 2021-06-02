#nullable enable
using System.Collections.Generic;
using System.Linq;
using OrderProcessing.Orders;

namespace OrderProcessing.Shipment
{
    class ShipmentHandler
    {
        private ShipmentHandler? _nextHandler;

        public ShipmentHandler? SetNext(ShipmentHandler? handler)
        {
            this._nextHandler = handler;
            return handler;
        }

        public virtual object? Handle(Order order)
        {
            if (this._nextHandler != null)
            {
                return this._nextHandler.Handle(order);
            }
            else
            {
                return null;
            }
        }
    }
}

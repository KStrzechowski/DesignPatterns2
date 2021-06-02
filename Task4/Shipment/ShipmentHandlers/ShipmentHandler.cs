#nullable enable
using System.Collections.Generic;
using System.Linq;
using OrderProcessing.Orders;

namespace OrderProcessing.Shipment
{
    class ShipmentHandler : IShipmentHandler
    {
        private IShipmentHandler? _nextHandler;

        public IShipmentHandler? SetNext(IShipmentHandler? handler)
        {
            this._nextHandler = handler;
            return handler;
        }

        public virtual IShipmentProvider? Handle(Order order)
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

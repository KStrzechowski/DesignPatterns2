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
            _nextHandler = handler;
            return handler;
        }

        public virtual IShipmentProvider? Handle(Order order, ITaxRateProvider taxRateProvider)
        {
            if (_nextHandler != null)
            {
                return _nextHandler.Handle(order, taxRateProvider);
            }
            else
            {
                return null;
            }
        }
    }
}

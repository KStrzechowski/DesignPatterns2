#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using OrderProcessing.Orders;

namespace OrderProcessing.Shipment
{
    class GlobalShipmentHandler : ShipmentHandler
    {
        private IShipmentProvider? _shipmentProvider = null;
        public override IShipmentProvider? Handle(Order order)
        {
            if (_shipmentProvider == null)
            {
                _shipmentProvider = new GlobalShipmentProvider();
            }
            return _shipmentProvider;
        }
    }
}

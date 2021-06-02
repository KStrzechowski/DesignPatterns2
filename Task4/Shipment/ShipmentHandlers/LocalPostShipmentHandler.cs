#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using OrderProcessing.Orders;

namespace OrderProcessing.Shipment
{
    class LocalPostShipmentHandler : ShipmentHandler
    {
        private IShipmentProvider? _shipmentProvider = null;
        public override IShipmentProvider? Handle(Order order, ITaxRateProvider taxRateProvider)
        {
            if (order.Recipient.Country == "Polska")
            {
                if (_shipmentProvider == null)
                {
                    _shipmentProvider = new LocalPostShipmentProvider(taxRateProvider);
                }
                return _shipmentProvider;
            }
            else
            {
                return base.Handle(order, taxRateProvider);
            }
        }
    }
}

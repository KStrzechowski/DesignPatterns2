#nullable enable
using System.Collections.Generic;
using System.Linq;
using OrderProcessing.Orders;

namespace OrderProcessing.Shipment
{
    class GlobalShipmentHandler : ShipmentHandler
    {
        public override object? Handle(Order order)
        {
            return base.Handle(order);
        }
    }
}

#nullable enable
using System.Collections.Generic;
using System.Linq;
using OrderProcessing.Orders;

namespace OrderProcessing.Shipment
{
    interface IShipmentHandler
    {
        IShipmentHandler? SetNext(IShipmentHandler? handler);

        IShipmentProvider? Handle(Order order);
    }
}

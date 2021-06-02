using System.Collections.Generic;
using System.Linq;
using OrderProcessing.Orders;

namespace OrderProcessing.Shipment
{
    public interface IShipmentProvider
    {
        string Name { get; }

        void RegisterForShipment(IShippableOrder order);

        string GetLabelForOrder(IShippableOrder order);

        IEnumerable<IParcel> GetParcels();
    }
}
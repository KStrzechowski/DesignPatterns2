#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderProcessing.Orders;

namespace OrderProcessing.Databases
{
    internal class FilterReadyForShipment : IFilter
    {
        public FilterReadyForShipment()
        {
        }

        public bool Filter(Order order)
        {
            return order.Status == OrderStatus.ReadyForShipment;
        }
    }
}

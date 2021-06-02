using OrderProcessing.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderProcessing.Shipment
{
    interface ITaxRateProvider
    {
        public KeyValuePair<string, int> GetCorrectTax(IShippableOrder order);
    }
}

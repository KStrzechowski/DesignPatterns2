using OrderProcessing.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderProcessing.Shipment
{
    class TaxRateProvider : ITaxRateProvider
    {
        readonly TaxRatesDB _taxRatesDB;
        public TaxRateProvider(TaxRatesDB taxRatesDB)
        {
            _taxRatesDB = taxRatesDB;
        }

        public  KeyValuePair<string, int> GetCorrectTax(IShippableOrder order)
        {
            var taxRates = _taxRatesDB.TaxRates.Where(x => x.Key == order.Recipient.Country).FirstOrDefault();
            if (taxRates.Key == null)
            {
                taxRates = new KeyValuePair<string, int>(order.Recipient.Country, 0);
            }
            return taxRates;
        }
    }
}

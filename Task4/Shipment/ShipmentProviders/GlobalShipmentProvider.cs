using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderProcessing.Shipment
{
    class GlobalShipmentProvider : IShipmentProvider
    {
        public string Name => "Global";
        private readonly List<KeyValuePair<string, int>> _taxRates = new();
        private readonly List<IShippableOrder> _orders = new();
        readonly GlobalLabelFormatter _formatter = new ();
        private readonly ITaxRateProvider _taxRateProvider;

        public GlobalShipmentProvider(ITaxRateProvider taxRateProvider)
        {
            _taxRateProvider = taxRateProvider;
        }

        public string GetLabelForOrder(IShippableOrder order)
        {
            return _formatter.GenerateLabelForOrder(Name, order.Recipient);
        }

        public IEnumerable<IParcel> GetParcels()
        {
            List<IParcel> parcels = new();
            decimal sum;
            foreach (var tax in _taxRates)
            {
                sum = 0;
                var orderList = _orders.FindAll(x => x.Recipient.Country == tax.Key);
                foreach(var order in orderList)
                {
                    sum += order.PaidAmount;
                }
                var taxCalculator = new LinearTaxCalculator(tax.Value);
                var summaryFormatter = new SummaryFormatter(taxCalculator);

                parcels.Add(new Parcel()
                {
                    ShipmentProviderName = Name,
                    BundlePrice = sum,
                    BundleTax = taxCalculator.CalculateTax(sum),
                    BundlePriceWithTax = sum + taxCalculator.CalculateTax(sum),
                    BundleHeader = summaryFormatter.PrintHeader(orderList.First().Recipient.Country),
                    Summary = summaryFormatter.PrintOrdersSummary(orderList),
                });
            }
            return parcels;
        }

        public void RegisterForShipment(IShippableOrder order)
        {
            _orders.Add(order);
            var taxRate = _taxRateProvider.GetCorrectTax(order);
            if (!_taxRates.Contains(taxRate))
            {
                _taxRates.Add(taxRate);
            }
        }
    }
}

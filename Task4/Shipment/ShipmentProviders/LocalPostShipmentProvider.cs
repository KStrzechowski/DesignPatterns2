using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderProcessing.Orders;

namespace OrderProcessing.Shipment
{
    class LocalPostShipmentProvider : IShipmentProvider
    {
        public string Name => "LocalPost";
        private int _taxRate = -1;
        private readonly List<IShippableOrder> _orders = new();

        public string GetLabelForOrder(IShippableOrder order)
        {
            var formatter = new LocalPostLabelFormatter();
            return formatter.GenerateLabelForOrder(Name, order.Recipient);
        }

        public IEnumerable<IParcel> GetParcels()
        {
            List<IParcel> parcels = new();
            var taxCalculator = new LinearTaxCalculator(_taxRate);
            var summaryFormatter = new SummaryFormatter(taxCalculator);

            decimal sum = 0;
            foreach (var order in _orders)
            {
                sum += order.PaidAmount;
            }

            parcels.Add(new Parcel()
            {
                ShipmentProviderName = Name,
                BundlePrice = sum,
                BundleTax = taxCalculator.CalculateTax(sum),
                BundlePriceWithTax = sum + taxCalculator.CalculateTax(sum),
                BundleHeader = summaryFormatter.PrintHeader(_orders.First().Recipient.Country),
                Summary = summaryFormatter.PrintOrdersSummary(_orders),
            });

            return parcels;
        }

        public void RegisterForShipment(IShippableOrder order)
        {
            _orders.Add(order);
        }

        public void GetTaxRate(KeyValuePair<string, int> taxRate)
        {
            if (this._taxRate < 0)
            {
                this._taxRate = taxRate.Value;
            }
        }
    }
}

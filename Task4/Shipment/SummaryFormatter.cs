using System.Collections.Generic;
using System.Text;

namespace OrderProcessing.Shipment
{
    public interface ISummaryFormatter
    {
        public string PrintHeader(string name);
        public string PrintOrdersSummary(IEnumerable<IShippableOrder> orders);
    }

    public class SummaryFormatter :ISummaryFormatter
    {
        private readonly ITaxCalculator _taxCalculator;
        private const int _lineLength = 56;

        public SummaryFormatter(ITaxCalculator taxCalculator)
        {
            _taxCalculator = taxCalculator;
        }

        public string PrintHeader(string name)
        {
            var suffixLength = (_lineLength- name.Length) / 2;
            var prefix = new string('-', suffixLength);
            var suffix = new string('-', _lineLength - name.Length - prefix.Length);
            return $"{prefix}{name}{suffix}";
        }

        public string PrintOrdersSummary(IEnumerable<IShippableOrder> orders)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"{"OrderId",8} {"Amount",15} {"Tax",15} {"AmountWithTax",15}");
            var totalAmount = 0m;
            var totalAmountWithTax = 0m;
            foreach (var order in orders)
            {
                totalAmount += order.PaidAmount;
                totalAmountWithTax += _taxCalculator.CalculateTax(order.PaidAmount) + order.PaidAmount;
                sb.AppendLine(
                    $"{order.OrderId,8} {order.PaidAmount,15:N2} {_taxCalculator.CalculateTax(order.PaidAmount),15:N2} {_taxCalculator.CalculateTax(order.PaidAmount) + order.PaidAmount,15:N2}");
            }

            sb.AppendLine($"TOTALS:  {totalAmount,15:N2} {" ",15} {totalAmountWithTax,15:N2}");
            return sb.ToString();
        }
    }
}

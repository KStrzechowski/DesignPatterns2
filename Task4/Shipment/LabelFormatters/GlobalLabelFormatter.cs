using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderProcessing.Shipment
{
    class GlobalLabelFormatter : ILabelFormatter
    {
        public string GenerateLabelForOrder(string providerName, IAddress address)
        {
            var sb = new StringBuilder(100);
            sb.AppendLine($"Shipment provider: {providerName}");
            sb.AppendLine(address.Name);
            sb.AppendLine(address.Line1);
            sb.AppendLine(address.Line2);
            sb.AppendLine(address.PostalCode);
            sb.AppendLine(address.Country);
            return sb.ToString();
        }
    }
}

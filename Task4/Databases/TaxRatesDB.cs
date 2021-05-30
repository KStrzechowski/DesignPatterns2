using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderProcessing
{
    public class TaxRatesDB
    {
        public IDictionary<string, int> TaxRates { get; }

        public TaxRatesDB(IDictionary<string, int> taxRates)
        {
            this.TaxRates = taxRates;
        }
    }
}

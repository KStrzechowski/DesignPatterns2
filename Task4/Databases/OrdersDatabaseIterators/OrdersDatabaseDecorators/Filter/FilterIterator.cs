#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderProcessing.Orders;

namespace OrderProcessing.Databases
{
    internal class FilterIterator : BaseVirusIteratorDecorator
    {
        private readonly IFilter _filter;
        public FilterIterator(IOrdersDatabaseIterator it, IFilter filter) : base(it)
        {
            this._filter = filter;
        }

        public override Order? Next()
        {
            var order = base.Next();
            while (order != null && !_filter.Filter(order))
            {
                order = base.Next();
            }
            return order;
        }
    }
}

#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderProcessing.Orders;

namespace OrderProcessing.Databases
{
    public abstract class BaseVirusIteratorDecorator : IOrdersDatabaseIterator
    {
        protected readonly IOrdersDatabaseIterator inner;
        public BaseVirusIteratorDecorator(IOrdersDatabaseIterator inner)
        {
            this.inner = inner;
        }

        public virtual Order? Next() => inner.Next();
    }
}

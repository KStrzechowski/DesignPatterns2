#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderProcessing.Orders;


namespace OrderProcessing.Databases
{
    class LocalOrdersDBIterator : IOrdersDatabaseIterator
    {
        private readonly Order[] _orders;
        private int i = 0;
        public LocalOrdersDBIterator(LocalOrdersDB database)
        {
            _orders = database.Orders;
        }

        public Order? Next()
        {
            if (_orders.Length > i)
            {
                var order = _orders[i];
                i++;
                return order;
            }
            else
            {
                return null;
            }
        }
    }
}

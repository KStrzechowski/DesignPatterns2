#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderProcessing.Orders;

namespace OrderProcessing.Databases
{
    class GlobalOrdersDBIterator : IOrdersDatabaseIterator
    {
        private readonly Queue<OrderNode> _nodeQueue = new();
        public GlobalOrdersDBIterator(GlobalOrdersDB database)
        {
            _nodeQueue.Enqueue(database.Root);
        }

        public Order? Next()
        {
            if (_nodeQueue.Count != 0)
            {
                var currentNode = _nodeQueue.Dequeue();
                if (currentNode.Left != null)
                    _nodeQueue.Enqueue(currentNode.Left);
                if (currentNode.Right != null)
                    _nodeQueue.Enqueue(currentNode.Right);

                return currentNode.Order;
            }
            else
            {
                return null;
            }
        }
    }
}

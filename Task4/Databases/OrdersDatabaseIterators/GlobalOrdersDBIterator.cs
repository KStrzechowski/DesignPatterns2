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
        private readonly Queue<OrderNode> nodeQueue = new Queue<OrderNode>();
        public GlobalOrdersDBIterator(GlobalOrdersDB database)
        {
            nodeQueue.Enqueue(database.Root);
        }

        public Order? Next()
        {
            if (nodeQueue.Count != 0)
            {
                var currentNode = nodeQueue.Dequeue();
                if (currentNode.Left != null)
                    nodeQueue.Enqueue(currentNode.Left);
                if (currentNode.Right != null)
                    nodeQueue.Enqueue(currentNode.Right);

                return currentNode.Order;
            }
            else
            {
                return null;
            }
        }
    }
}

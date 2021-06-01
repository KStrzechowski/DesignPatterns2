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
        private readonly Stack<OrderNode> nodeStack = new Stack<OrderNode>();
        public GlobalOrdersDBIterator(GlobalOrdersDB database)
        {
            nodeStack.Push(database.Root);
        }

        public Order? Next()
        {
            if (nodeStack.Count != 0)
            {
                var currentNode = nodeStack.Pop();
                if (currentNode.Parent != null)     
                    nodeStack.Push(currentNode.Parent);
                if (currentNode.Right != null)
                    nodeStack.Push(currentNode.Right);

                return currentNode.Order;
            }
            else
            {
                return null;
            }
        }
    }
}

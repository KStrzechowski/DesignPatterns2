using OrderProcessing.Orders;

namespace OrderProcessing.Databases
{
    public class LocalOrdersDB
    {
        public Order[] Orders { get; }

        public LocalOrdersDB(Order[] orders)
        {
            Orders = orders;
        }
    }
}
namespace OrderProcessing.Orders
{
    public class LineItem
    {
        public LineItem(string name, int quantity, decimal singleItemPrice)
        {
            Name = name;
            Quantity = quantity;
            SingleItemPrice = singleItemPrice;
        }

        public string Name { get; set; }

        public int Quantity { get; set; }

        public decimal SingleItemPrice { get; set; }

        public decimal TotalPrice => SingleItemPrice * Quantity;
    }
}

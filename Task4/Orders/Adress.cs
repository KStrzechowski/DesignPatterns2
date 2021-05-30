using OrderProcessing.Shipment;

namespace OrderProcessing.Orders
{
    public struct Address : IAddress
    {
        public string Name { get; set; }
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
    }
}

namespace OrderProcessing.Shipment
{
    public interface IAddress
    {
        string Name { get; }
        string Line1 { get; }
        string Line2 { get; }
        string PostalCode { get; }
        string Country { get; }
    }
}
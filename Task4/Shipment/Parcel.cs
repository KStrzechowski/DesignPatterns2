namespace OrderProcessing.Shipment
{
    public interface IParcel
    {
        string ShipmentProviderName { get; }
        string BundleHeader { get; }
        string Summary { get; }

        decimal BundlePrice { get; }
        decimal BundleTax { get; }
        decimal BundlePriceWithTax { get; }
    }

    public class Parcel : IParcel
    {
        public string ShipmentProviderName { get; set; }
        public string BundleHeader { get; set; }
        public string Summary { get; set; }
        public decimal BundlePrice { get; set; }
        public decimal BundleTax { get; set; }
        public decimal BundlePriceWithTax { get; set; }
    }
}
using System.Text;

namespace OrderProcessing.Shipment
{
    public interface ILabelFormatter
    {
        string GenerateLabelForOrder(string providerName, IAddress address);
    }
}
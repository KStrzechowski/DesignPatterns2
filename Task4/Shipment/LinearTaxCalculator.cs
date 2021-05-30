namespace OrderProcessing.Shipment
{
    public interface ITaxCalculator
    {
        decimal CalculateTax(decimal price);
    }

    public class LinearTaxCalculator : ITaxCalculator
    {
        private readonly int _taxPercent;

        public LinearTaxCalculator(int taxPercent)
        {
            _taxPercent = taxPercent;
        }

        public decimal CalculateTax(decimal price)
        {
            return (_taxPercent * price)/100.0m;
        }
    }
}
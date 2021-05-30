namespace OrderProcessing.Payments
{
    public class Payment
    {
        public Payment(PaymentMethod paymentType, decimal amount)
        {
            PaymentType = paymentType;
            Amount = amount;
        }

        public PaymentMethod PaymentType { get; set; }

        public decimal Amount { get; set; }
    }
}
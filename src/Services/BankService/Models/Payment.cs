namespace BankService.Models
{
    public class Payment
    {
        public string Id { get; set; }
        public int Amount { get; set; }
        public Customer Receiver { get; set; }
        public Customer Sender { get; set; }
    }
}
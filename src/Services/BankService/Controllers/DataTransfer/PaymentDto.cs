namespace BankService.Controllers.DataTransfer
{
    public class PaymentDto
    {
        public int Amount { get; set; }
        public string ReceiverId { get; set; }
        public string SenderId { get; set; }
    }
}
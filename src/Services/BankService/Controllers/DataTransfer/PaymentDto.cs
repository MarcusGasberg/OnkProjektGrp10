using System.ComponentModel.DataAnnotations;

namespace BankService.Controllers.DataTransfer
{
    public class PaymentDto
    {
        public int Amount { get; set; }
        [Required]
        public string ReceiverId { get; set; }
        [Required]
        public string SenderId { get; set; }
    }
}
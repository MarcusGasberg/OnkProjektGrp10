using System.ComponentModel.DataAnnotations.Schema;

namespace BankService.Models
{
    public class Payment
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public int Amount { get; set; }
        public Customer Receiver { get; set; }
        public string ReceiverId { get; set; }
        public Customer Sender { get; set; }
        public string SenderId { get; set; }
    }
}
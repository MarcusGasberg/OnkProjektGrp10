using System.Collections.Generic;

namespace BankService.Models
{
    public class Customer
    {
        public string Id { get; set; }
        public string RegistrationNumber { get; set; }
        public int Balance { get; set; }
        public List<Payment> ReceivedPayments { get; set; } = new List<Payment>();
        public List<Payment> SendPayments { get; set; } = new List<Payment>();
    }
}
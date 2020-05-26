using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using IdentityModel;
using BankService.Controllers.DataTransfer;
using BankService.Data;
using BankService.Models;

namespace BankService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class PaymentController : ControllerBase
    {
        private readonly ILogger<PaymentController> _logger;
        private BankDbContext dbContext;

        public PaymentController(ILogger<PaymentController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return new JsonResult(from c in User.Claims select new { c.Type, c.Value });
        }

        [HttpPost]
        public async Task<IActionResult> Post(PaymentDto payment)
        {
            using (dbContext = new BankDbContext())
            {
                Customer sender = await dbContext.Customers.FindAsync(payment.SenderId);
                Customer receiver = await dbContext.Customers.FindAsync(payment.ReceiverId);
                if (sender == null)
                {
                    return BadRequest("Sender isn't registered");
                }
                if (receiver == null)
                {
                    return BadRequest("Receiver isn't registered");
                }

                if (sender.Balance < payment.Amount)
                {
                    return BadRequest("Insufficient funds");
                }

                var dbPayment = new Payment()
                {
                    Sender = sender,
                    Receiver = receiver,
                    Amount = payment.Amount
                };

                dbPayment = (await dbContext.AddAsync(dbPayment)).Entity;

                dbContext.AttachRange(new[] { sender, receiver });

                sender.Balance -= payment.Amount;
                receiver.Balance += payment.Amount;

                sender.SendPayments.Add(dbPayment);
                receiver.ReceivedPayments.Add(dbPayment);

                await dbContext.SaveChangesAsync();

                return Created($"payment/{dbPayment.Id}", dbPayment);
            }
        }
    }
}

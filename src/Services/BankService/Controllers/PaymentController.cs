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
        private readonly ILogger<PaymentController> logger;
        private BankDbContext dbContext;

        public PaymentController(ILogger<PaymentController> logger, BankDbContext dbContext)
        {
            this.logger = logger;
            this.dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return new JsonResult(from c in User.Claims select new { c.Type, c.Value });
        }

        [HttpPost]
        public async Task<IActionResult> Post(PaymentDto payment)
        {
            if (!ModelState.IsValid)
            {
                logger.LogDebug($"BadRequest: receiver: {payment.ReceiverId}, sender: {payment.SenderId} ");
                return BadRequest(ModelState);
            }

            Customer sender = await dbContext.Customers.FindAsync(payment.SenderId);
            Customer receiver = await dbContext.Customers.FindAsync(payment.ReceiverId);

            if (sender == null)
            {
                logger.LogDebug($"Sender not found for id: {sender?.Id}");
                return BadRequest("Sender isn't registered");
            }

            if (receiver == null)
            {
                logger.LogDebug($"Receiver not found for id: {sender?.Id}");
                return BadRequest("Receiver isn't registered");
            }

            if (sender.Balance < payment.Amount)
            {
                logger.LogDebug($"Sender has insufficient funds id: {sender?.Id}");
                return BadRequest("Insufficient funds");
            }

            var dbPayment = new Payment()
            {
                Amount = payment.Amount
            };

            dbContext.AttachRange(new[] { sender, receiver });

            sender.Balance -= payment.Amount;
            receiver.Balance += payment.Amount;

            sender.SendPayments.Add(dbPayment);
            receiver.ReceivedPayments.Add(dbPayment);

            await dbContext.SaveChangesAsync();

            logger.LogInformation($"Payment created with id: {dbPayment.Id}");

            return Created($"payment/{dbPayment.Id}", dbPayment);
        }
    }
}

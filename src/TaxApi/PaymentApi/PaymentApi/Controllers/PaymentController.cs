using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TaxApi.Controllers;

namespace PaymentApi.Controllers
{


    [ApiController]
    [Route("[controller]")]
    public class PaymentController : ControllerBase
    {
        private TaxController tax;

        private readonly ILogger<PaymentController> _logger;

        public PaymentController(ILogger<PaymentController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetPayment()
        {
            //get på account i user

            int money = 100;

            //get tax
            var tax = await this.tax.CalTax(100, "test");

            //call charge requester

            //call pay provider

            if (/*result ok*/)
                return Ok();
            else
            {
                return BadRequest();
            }

        }
    }
}

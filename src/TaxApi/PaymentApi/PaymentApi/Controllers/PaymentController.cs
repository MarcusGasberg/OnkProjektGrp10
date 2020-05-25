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
        private HttpClient _client = new HttpClient();

        private readonly ILogger<PaymentController> _logger;

        public PaymentController(ILogger<PaymentController> logger)
        {
            _logger = logger;
        }

        //TODO might want to change this to target specific ports in Kubenetes  

        [HttpGet]
        public async Task<IActionResult> GetPayment()
        {
            //get på account i user
            var userResponse = await _client.GetAsync("useraccount");

            userResponse.EnsureSuccessStatusCode();

            //get tax

            var taxResponse = await _client.PostAsync("controller/tax", userResponse.Content);

            taxResponse.EnsureSuccessStatusCode();

            //call charge requester

            var chargeResponse = await _client.PostAsync("payprovider", taxResponse.Content);

            chargeResponse.EnsureSuccessStatusCode();

            //call pay provider

            var payResponse = await _client.PutAsync("payment", chargeResponse.Content);

            payResponse.EnsureSuccessStatusCode();

            return Ok();

        }
    }
}

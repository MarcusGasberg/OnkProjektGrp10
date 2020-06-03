using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace PaymentApi.Controllers
{


    [ApiController]
    [Route("[controller]")]
    public class PaymentController : ControllerBase{
        private HttpClient _client = new HttpClient();


        private readonly ILogger<PaymentController> _logger;

        public PaymentController(ILogger<PaymentController> logger)
        {
            _logger = logger;
        }

        //TODO might want to change this to target specific ports in Kubenetes  

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> PostPayment(RequestBody body) {
            
        var accessToken = await HttpContext.GetTokenAsync("access_token");

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        
        var taxdata = new {
            Price = body.Price,
            BuyerID = body.BuyerID
            };
            
            //get tax

        var taxResponse = await _client.PostAsync(requestUri: "https://localhost:5003", new StringContent(JsonSerializer.Serialize(taxdata)));

        taxResponse.EnsureSuccessStatusCode();

            //call charge requester
        var paymentdata = new {
            Price = taxResponse.Price,
            BuyerId = body.BuyerID,
            SellerId = body.SellerId };

            var chargeResponse = await _client.PostAsync("http://localhost:5004/payment",new StringContent(JsonSerializer.Serialize(paymentdata)));

            chargeResponse.EnsureSuccessStatusCode();

            return Ok();

        }
    }

    public class RequestBody{
        public int Price { get; set; }

        public string StockName { get; set; }

        public string SellerId { get; set; }

        public string BuyerID { get; set; }
    }

    public class Tax{
        public int Amount { set; get; }
        public string Id { set; get; }

        public Tax(int amount, string id)
        {
            Amount = amount;
            Id = id;
        }
    
    }

    public class PaymentDTO{

        public int Amount { get; set; }

        public string ReceiverId { get; set; }

        public string SenderId { get; set; }

        public PaymentDTO(int Amount, string ReceiverId, string SenderId)
        {
            this.Amount = Amount;
            this.ReceiverId = ReceiverId;
            this.SenderId = SenderId;
        }
    }

}



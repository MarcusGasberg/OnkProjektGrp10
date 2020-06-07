using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;

namespace PaymentApi.Controllers
{
    [ApiController]
    [Microsoft.AspNetCore.Mvc.Route("[controller]")]
    [Authorize]
    public class PaymentController : ControllerBase
    {
        private HttpClient _client = new HttpClient();
        private readonly ILogger<PaymentController> _logger;
        private readonly IConfiguration configuration;

        public PaymentController(ILogger<PaymentController> logger, IConfiguration configuration)
        {
            _logger = logger;
            this.configuration = configuration;
        }

        //TODO might want to change this to target specific ports in Kubenetes  

        [Microsoft.AspNetCore.Mvc.HttpPost]
        public async Task<IActionResult> PostPayment(RequestBody body)
        {

            var accessToken = await HttpContext.GetTokenAsync("access_token");

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var taxdata = new Tax(body.Price, body.BuyerId);

            var taxedObject = await GetTaxAsync(taxdata);

            //call charge requester
            var paymentdata = new
            {
                Amount = taxedObject.Amount,
                SenderId = body.BuyerId,
                ReceiverId = body.SellerId
            };

            var apiUrl = configuration.GetValue<string>("BankApiUrl") + "/payment";

            var payJson = JsonConvert.SerializeObject(paymentdata);
            var payContent = new StringContent(payJson, Encoding.UTF8, "application/json");
            var chargeResponse = await _client.PostAsync(apiUrl, payContent);
            chargeResponse.EnsureSuccessStatusCode();

            return Ok();

        }


        public async Task<Tax> GetTaxAsync(Tax tax)
        {

            //get tax
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                var json = JsonConvert.SerializeObject(tax);

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var apiUrl = configuration.GetValue<string>("TaxApiUrl") + "/tax";

                var response = await _client.PostAsync(requestUri: apiUrl, content);
                response.EnsureSuccessStatusCode();
                var rcontent = await response.Content.ReadAsStringAsync();

                Tax value = JsonConvert.DeserializeObject<Tax>(rcontent);

                return value;

            }
        }
    }
    public class RequestBody
    {
        public int Price { get; set; }

        public string StockName { get; set; }

        public string SellerId { get; set; }

        public string BuyerId { get; set; }
    }

    public class Tax
    {
        public int Amount { set; get; }
        public string Id { set; get; }

        public Tax(int amount, string id)
        {
            Amount = amount;
            Id = id;
        }

    }

    public class PaymentDTO
    {

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



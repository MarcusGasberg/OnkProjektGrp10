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

namespace PaymentApi.Controllers
{


    [ApiController]
    [Route("[controller]")]
    public class PaymentController : ControllerBase
    {

        private HttpClient _client = new HttpClient();

        private readonly ILogger<PaymentController> _logger;

        public PaymentController(ILogger<PaymentController> logger)
        {
            _logger = logger;
        }

        //TODO might want to change this to target specific ports in Kubenetes  

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> PostPayment(RequestBody body)
        {

            //create HttpContent body

            var contentTax = new Tax(body.Price, body.BuyerID);
            
            //get tax

            var taxResponse = await _client.PostAsync(requestUri: "https://localhost:5003", contentTax);
            
            taxResponse.EnsureSuccessStatusCode();

            //call charge requester

            var payment = new PaymentDTO(body.Price,body.BuyerID,body.SellerId );

            var chargeResponse = await _client.PostAsync("http://localhost:5004/payment",payment);

            chargeResponse.EnsureSuccessStatusCode();

            return Ok();

        }
    }

    //https://stackoverflow.com/questions/20493197/posting-a-custom-type-with-httpclient
    public class RequestBody : HttpContent
    {

        private readonly MemoryStream _Stream = new MemoryStream();

        public int Price { get; set; }

        public string StockName { get; set; }

        public string SellerId { get; set; }

        public string BuyerID { get; set; }

        protected override Task SerializeToStreamAsync(Stream stream, TransportContext context)
        {
            _Stream.CopyTo(stream);
            var tcs = new TaskCompletionSource<object>();
            tcs.SetResult(null);
            return tcs.Task;
        }

        protected override bool TryComputeLength(out long length)
        {
            length = _Stream.Length;
            return true;
        }
    }
    public class Tax : HttpContent
    {

        private readonly MemoryStream _Stream = new MemoryStream();

        public int Amount { set; get; }
        public string Id { set; get; }

        public Tax(int amount, string id)
        {
            Amount = amount;
            Id = id;
        }
        protected override Task SerializeToStreamAsync(Stream stream, TransportContext context)
        {
            _Stream.CopyTo(stream);
            var tcs = new TaskCompletionSource<object>();
            tcs.SetResult(null);
            return tcs.Task;
        }

        protected override bool TryComputeLength(out long length)
        {
            length = _Stream.Length;
            return true;
        }
    }

    public class PaymentDTO : HttpContent
    {
        private readonly MemoryStream _Stream = new MemoryStream();

        public int Amount { get; set; }

        public string ReceiverId { get; set; }

        public string SenderId { get; set; }

        public PaymentDTO(int Amount, string ReceiverId, string SenderId)
        {
            this.Amount = Amount;
            this.ReceiverId = ReceiverId;
            this.SenderId = SenderId;
        }
        protected override Task SerializeToStreamAsync(Stream stream, TransportContext context)
        {
            _Stream.CopyTo(stream);
            var tcs = new TaskCompletionSource<object>();
            tcs.SetResult(null);
            return tcs.Task;
        }

        protected override bool TryComputeLength(out long length)
        {
            length = _Stream.Length;
            return true;
        }
    }

}



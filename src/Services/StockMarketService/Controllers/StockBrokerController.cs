using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using StockMarketService.Models;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;
using Microsoft.Extensions.Configuration;
using System.Text;

namespace StockMarketService
{


    [ApiController]
    [Route("[controller]")]
    public class StockBrokerController : Controller
    {

        public StockBrokerController(Commands commands, IConfiguration configuration)
        {
            this.commands = commands;
            this.configuration = configuration;
        }

        private Commands commands;
        private readonly IConfiguration configuration;

        [HttpPost]
        [Route("purchase")]
        public async Task<StatusCodeResult> SellStock([FromBody] TradeRequest request)
        {
            if (commands.GetSeller(request.StockName, request.Number) != null)
            {
                var stock = commands.GetStock(request.StockName);
                var seller = commands.GetSeller(request.StockName, request.Number);
                var buyerId = User.Claims.FirstOrDefault(c => c.Type.Equals(JwtClaimTypes.Name))?.Value;
                var accessToken = await HttpContext.GetTokenAsync("access_token");
                var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var data = new
                {
                    Price = stock.HistoricPrice.FirstOrDefault().Price,
                    StockName = stock.Name,
                    SellerId = seller.SellerId,
                    BuyerId = buyerId
                };

                var apiUrl = configuration.GetValue<string>("PaymentApiUrl");

                try
                {

                    var res = await client.PostAsync(apiUrl + "/payment", new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json"));

                    Console.WriteLine(await res.Content.ReadAsStringAsync());

                    if (res.StatusCode != HttpStatusCode.OK || !commands.BuyStock(request.StockName, request.Number))
                        return StatusCode(403);

                    return StatusCode(200);
                }
                catch (Exception e)
                {
                    throw e;
                }

            }
            return StatusCode(403);
        }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using StockMarketService.Models;
using Microsoft.Extensions.DependencyInjection;

namespace StockMarketService {
    
    
    [ApiController]
    [Route("[controller]")]
    public class StockBrokerController : Controller {

        public StockBrokerController(Commands commands)
        {
            this.commands = commands;
        }

        private Commands commands;


        [HttpPost]
        [Route("Purchase")]
        public async Task<StatusCodeResult> SellStock([FromBody] TradeRequest request) {
            if (commands.GetSeller(request.StockName, request.Number) != null) {
                var stock = commands.GetStock(request.StockName);
                var seller = commands.GetSeller(request.StockName, request.Number);
                var buyerId = User.Claims.FirstOrDefault(c => c.Type.Equals(JwtClaimTypes.Subject))?.Value;
                var accessToken = await HttpContext.GetTokenAsync("access_token");
                var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var data = new {
                    Price = stock.HistoricPrice.FirstOrDefault().Price,
                    StockName = stock.Name,
                    SellerId = "1" ,
                    BuyerId = "1"
                };

                var res = await client.PostAsync("payment/payment", new StringContent(JsonSerializer.Serialize(data)));

                if (res.StatusCode != HttpStatusCode.OK || !commands.BuyStock(request.StockName, request.Number)) return StatusCode(403);
                
                return StatusCode(200);

            }
            return StatusCode(403);
        }
         
    }
}
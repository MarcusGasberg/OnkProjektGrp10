using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using StockMarketService.Models;

namespace StockMarketService {
    
    
    [ApiController]
    [Route("[controller]")]
    public class StockBrokerController : Controller {
        
        
        [HttpPost]
        [Route("SellStock")]
        public async Task<ActionResult> SellStock([FromBody] TradeRequest request) {
            if (Commands.GetSeller(request.StockName, request.Number) != null) {
                var stock = Commands.GetStock(request.StockName);
                var seller = Commands.GetSeller(request.StockName, request.Number);
                var accessToken = await HttpContext.GetTokenAsync("access_token");
                var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var data = new {
                    Price = stock.HistoricPrice.FirstOrDefault().Price,
                    StockName = stock.Name,
                    SellerId = seller.Id
                };

                var res = await client.PostAsync("Some address", new StringContent(JsonSerializer.Serialize(data)));

                if (res.StatusCode != HttpStatusCode.OK) return StatusCode(403);
                Commands.SellStock(request.StockName, request.Number);
                return StatusCode(200);

            }
        }
         
    }
}
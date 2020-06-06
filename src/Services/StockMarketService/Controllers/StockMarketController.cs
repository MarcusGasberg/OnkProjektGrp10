using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using IdentityModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StockMarketService.Middleware;
using StockMarketService.Models;

namespace StockMarketService
{
    [ApiController]
    [Route("[controller]")]
    public class StockMarketController : ControllerBase
    {
        private readonly ILogger<StockMarketController> _logger;
        private readonly WebSocketConnectionManager _manager;
        private Commands commands;

        public StockMarketController(ILogger<StockMarketController> logger, WebSocketConnectionManager connectionManager, Commands commands)
        {
            _logger = logger;
            _manager = connectionManager;
            this.commands = commands;
        }

        [HttpGet]
        public List<Stock> Get() {
            return commands.GetStocks();
        }
        
        [HttpGet]
        [Route("{stockname}")]
        public Stock GetStock(string stockname) {
            stockname = stockname.Replace("_", " ");
            return commands.GetStock(stockname);
        }
        
        [HttpGet]
        [Route("userstock/{id}")]
        public List<Stock> UserStock(string id) {
            //return commands.GetUserStock(User.Claims.FirstOrDefault(c => c.Type.Equals(JwtClaimTypes.Subject))?.Value);
            return commands.GetUserStock(id);
        }
        
        [HttpPost]
        public ActionResult AddStock([FromBody] Stock stock) {
            commands.AddNewStock(stock);
            return StatusCode(200);
        }
        
        [HttpPost]
        [Route("sell")]
        public ActionResult SellStock([FromBody] TradeRequest stock)
        {
            commands.SellStock(stock.StockName, stock.Number, "2");
            return StatusCode(200);
        }
        
        [HttpPost]
        [Route("{update}")]
        public ActionResult UpdateClients() {
            commands.UpdateClients();
            return StatusCode(200);
        }
        
        [HttpPut]
        private ActionResult PutStock(Stock stock) {
            commands.UpdateStock(stock);
            return StatusCode(200);
        }

    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StockMarketService.Middleware;

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
        
        [HttpPost]
        public ActionResult AddStock([FromBody] Stock stock) {
            commands.AddNewStock(stock);
            return StatusCode(200);
        }
        
        [HttpPut]
        private ActionResult PutStock(Stock stock) {
            commands.UpdateStock(stock);
            return StatusCode(200);
        }

    }
}

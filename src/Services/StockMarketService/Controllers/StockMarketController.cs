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

        public StockMarketController(ILogger<StockMarketController> logger, WebSocketConnectionManager connectionManager)
        {
            _logger = logger;
            _manager = connectionManager;
        }

        [HttpGet]
        public List<Stock> Get() {
            return Commands.GetStocks();
        }
        
        [HttpGet]
        [Route("{stockname}")]
        public Stock GetStock(string stockname) {
            stockname = stockname.Replace("_", " ");
            return Commands.GetStock(stockname);
        }
        
        [HttpPost]
        public void AddStock([FromBody] Stock stock) {
            Commands.AddNewStock(stock, _manager);
        }
        
        [HttpPut]
        private void PutStock(Stock stock) {
            Commands.UpdateStock(stock, _manager);
        }
    }
}

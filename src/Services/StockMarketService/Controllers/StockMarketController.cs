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

        public StockMarketController(ILogger<StockMarketController> logger, WebSocketConnectionManager connectionManager, ApplicationDbContext dbContext)
        {
            _logger = logger;
            _manager = connectionManager;
            commands = new Commands(dbContext);
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
        public void AddStock([FromBody] Stock stock) {
            commands.AddNewStock(stock, _manager);
        }
        
        [HttpPut]
        private void PutStock(Stock stock) {
            commands.UpdateStock(stock, _manager);
        }

    }
}

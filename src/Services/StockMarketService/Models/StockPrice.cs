using System;
using System.ComponentModel.DataAnnotations;

namespace StockMarketService.Models
{
    public class StockPrice {
        public StockPrice(decimal price, DateTime updateTime)
        {
            this.Price = price;
            this.UpdateTime = updateTime;
        }
        public string Id{ get; set; }
        public StockPrice() { }
        public decimal Price { get; set; }
        public DateTime UpdateTime { get; set; }
        public Stock stock { get; set; }
    }
}
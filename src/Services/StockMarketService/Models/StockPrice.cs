using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StockMarketService.Models
{
    public class StockPrice {
        public StockPrice(decimal price, DateTime updateTime)
        {
            this.Price = price;
            this.UpdateTime = updateTime;
        }
        public string Id { get; set; }
        public decimal Price { get; set; }
        public DateTime UpdateTime { get; set; }
        public Stock stock { get; set; }
    }
}
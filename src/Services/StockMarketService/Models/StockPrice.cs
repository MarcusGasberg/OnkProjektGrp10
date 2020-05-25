using System;
using System.ComponentModel.DataAnnotations;

namespace StockMarketService.Models
{
    public class StockPrice
    {
        public StockPrice(decimal price, DateTime updateTime)
        {
            this.Price = price;
            this.UpdateTime = updateTime;
        }

        public StockPrice() { }

        public decimal Price { get; set; }
        [Key]
        public DateTime UpdateTime { get; set; }
    }
}
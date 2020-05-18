using System;

namespace StockMarketService.Models
{
    public class StockPrice
    {
        public StockPrice(decimal price, DateTime updateTime)
        {
            this.Price = price;
            this.UpdateTime = updateTime;
        }

        public decimal Price { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}
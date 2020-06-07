using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StockMarketService.Models
{
    [Serializable]
    public class StockPrice
    {
        public StockPrice(int price, DateTime updateTime)
        {
            this.Price = price;
            this.UpdateTime = updateTime;
        }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public int Price { get; set; }
        public DateTime UpdateTime { get; set; }
        public Stock stock { get; set; }
    }
}
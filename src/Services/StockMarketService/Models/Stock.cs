
using System;
using System.ComponentModel.DataAnnotations;

public class Stock
{
    [Key]
    public string name { get; set; }
    double currentPrice { get; set; }

    StockPrice[] historicPrice { get; set; }

    public class StockPrice
    {
        public decimal price { get; set; }
        public DateTime updateTime { get; set; }
    }
}
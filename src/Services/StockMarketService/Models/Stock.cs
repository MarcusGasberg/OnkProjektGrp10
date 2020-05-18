
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using StockMarketService.Models;

public class Stock
{
    [Key]
    public string Name { get; set; }
    public List<StockPrice> HistoricPrice { get; set; }

    public int AmountForSale { get; set; }
    
    
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using StockMarketService.Models;

public class Stock
{
    public string Id{ get; set; }

    public string Name { get; set; }
    public ICollection<StockPrice> HistoricPrice { get; set; }
    public ICollection<Seller> Seller { get; set; }

}
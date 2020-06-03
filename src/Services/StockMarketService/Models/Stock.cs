
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using StockMarketService.Models;

public class Stock
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string Id { get; set; }
    public string Name { get; set; }
    public ICollection<StockPrice> HistoricPrice { get; set; }
    public ICollection<Seller> Seller { get; set; }

}
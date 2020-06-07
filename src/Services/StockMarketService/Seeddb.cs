using System;
using System.Collections.Generic;
using StockMarketService.Models;

namespace StockMarketService
{
    public static class Seeddb
    {
        public static void run(ApplicationDbContext context)
        {
            var lst = new List<Stock>() {
                new Stock()
                {
                    Name = "Apple Inc.",
                    HistoricPrice = new List<StockPrice>()
                    {
                        new StockPrice(1166, DateTime.Now)
                    },
                    Seller = new List<Seller>()
                    {
                        new Seller()
                        {
                            SellerId = "Edward",
                            SellingAmount = 1000
                        }
                    }
                },
                new Stock()
                {
                    Name = "Microsoft Cooperation",
                    HistoricPrice = new List<StockPrice>()
                    {
                        new StockPrice(18479, DateTime.Now)
                    },
                    Seller = new List<Seller>()
                    {
                        new Seller()
                        {
                            SellerId = "Edward",
                            SellingAmount = 6231
                        }
                    }
                },
                new Stock()
                {
                    Name = "Starbucks Cooperation",
                    HistoricPrice = new List<StockPrice>()
                    {
                        new StockPrice(7952, DateTime.Now)
                    },
                    Seller = new List<Seller>()
                    {
                        new Seller()
                        {
                            SellerId = "Edward",
                            SellingAmount = 2345
                        }
                    }
                }
            };
            foreach (var stock in lst)
            {
                context.Stocks.Add(stock);
            }
        }
    }
}
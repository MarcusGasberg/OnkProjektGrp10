using System;
using System.Collections.Generic;
using System.Linq;
using StockMarketService.Models;

namespace StockMarketService
{
    public static class Commands
    {
        public static void AddNewStock(Stock stock)
        {
            using (var db = new ApplicationDbContext())
            {
                db.Stocks.Add(stock);
            }
        }

        public static void UpdateStockPrice(string stockName,decimal newStockPrice)
        {
            using (var db = new ApplicationDbContext())
            {
                var stock = db.Stocks.Find(stockName);
                stock.HistoricPrice.Add(new StockPrice(newStockPrice, DateTime.Now));
                db.Stocks.Update(stock);
            }
        }

        public static List<Stock> GetStocks()
        {
            using (var db = new ApplicationDbContext())
            {
                return db.Stocks.ToList();
            }
        }
        
        public static void UpdateStockAmountForSale(string stockName,int newtotal)
        {
            using (var db = new ApplicationDbContext())
            {
                var stock = db.Stocks.Find(stockName);
                stock.AmountForSale = newtotal;
                db.Stocks.Update(stock);
            }
        }
        
    }
}
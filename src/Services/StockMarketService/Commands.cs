using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using StockMarketService.Middleware;
using StockMarketService.Models;

namespace StockMarketService
{
    public static class Commands
    {
        public static void AddNewStock(Stock stock, WebSocketConnectionManager manager) {
            using var db = new ApplicationDbContext();
            Console.WriteLine(JsonSerializer.Serialize(stock));
            db.Stocks.Add(stock);
            db.SaveChanges();
            var updateTask = new Thread(async () => await manager.UpdateAllClients(GetStocks()));
            updateTask.Start();
        }

        public static void UpdateStockPrice(string stockName,decimal newStockPrice, WebSocketConnectionManager manager) {
            using var db = new ApplicationDbContext();
            var stock = db.Stocks.Find(stockName);
            stock.HistoricPrice.Add(new StockPrice(newStockPrice, DateTime.Now));
            db.Stocks.Update(stock);
            var updateTask = new Thread(async () => await manager.UpdateAllClients(GetStocks()));
            updateTask.Start();
        }

        public static List<Stock> GetStocks() {
            using var db = new ApplicationDbContext();
            return db.Stocks.Include(s => s.HistoricPrice).Include(s => s.Seller).ToList();
        }
        
        public static Stock GetStock(string name) {
            using var db = new ApplicationDbContext();
            return db.Stocks.Include(s => s.HistoricPrice).Include(s => s.Seller).FirstOrDefault(s => s.Name == name);
        }
        
        public static void UpdateStock(Stock stock, WebSocketConnectionManager manager) {
            using var db = new ApplicationDbContext();
            Console.WriteLine(JsonSerializer.Serialize(stock));
            db.Stocks.Update(stock);
            db.SaveChanges();
            var updateTask = new Thread(async () => await manager.UpdateAllClients(GetStocks()));
            updateTask.Start();
        }
        
        public static Seller GetSeller(string name, int number) {
            using var db = new ApplicationDbContext();
            Stock stock = GetStock(name);
            Seller seller = stock.Seller.FirstOrDefault(s => s.SellingAmount >= number);
            if (seller == null ) {
                return null;
            }
            return seller;
        }

        public static void SellStock(string name, int number) {
            using var db = new ApplicationDbContext();
            Stock stock = GetStock(name);
            Seller seller = stock.Seller.FirstOrDefault(s => s.SellingAmount >= number);
            if (seller == null ) {
                return false;
            }

            stock.Seller.Remove(seller);
            seller.SellingAmount = seller.SellingAmount - number;
            stock.Seller.Add(seller);
            db.SaveChanges();
            return true;
        }

    }
}
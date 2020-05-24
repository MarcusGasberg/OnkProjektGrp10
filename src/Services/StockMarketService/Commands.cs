using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using Microsoft.AspNetCore.Mvc;
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
            var updateTask = new Thread(async () => await manager.UpdateAllClients(JsonSerializer.Serialize(GetStocks())));
            updateTask.Start();
        }

        public static void UpdateStockPrice(string stockName,decimal newStockPrice, WebSocketConnectionManager manager) {
            using var db = new ApplicationDbContext();
            var stock = db.Stocks.Find(stockName);
            stock.HistoricPrice.Add(new StockPrice(newStockPrice, DateTime.Now));
            db.Stocks.Update(stock);
            var updateTask = new Thread(async () => await manager.UpdateAllClients(JsonSerializer.Serialize(GetStocks())));
            updateTask.Start();
        }

        public static List<Stock> GetStocks() {
            using var db = new ApplicationDbContext();
            return db.Stocks.ToList();
        }
        
        public static Stock GetStock(string name) {
            using var db = new ApplicationDbContext();
            return db.Stocks.Find(name);
        }
        
        public static void UpdateStock(Stock stock, WebSocketConnectionManager manager) {
            using var db = new ApplicationDbContext();
            Console.WriteLine(JsonSerializer.Serialize(stock));
            db.Stocks.Update(stock);
            db.SaveChanges();
            var updateTask = new Thread(async () => await manager.UpdateAllClients(JsonSerializer.Serialize(GetStocks())));
            updateTask.Start();
        }
        
    }
}
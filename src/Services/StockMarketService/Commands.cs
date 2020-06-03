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
    public class Commands
    {
        private ApplicationDbContext context;

        public Commands(ApplicationDbContext context)
        {
            this.context = context;
        }
        public void AddNewStock(Stock stock, WebSocketConnectionManager manager) {
            Console.WriteLine(JsonSerializer.Serialize(stock));
            context.Stocks.Add(stock);
            context.SaveChanges();
            var updateTask = new Thread(async () => await manager.UpdateAllClients(GetStocks()));
            updateTask.Start();
        }

        public void UpdateStockPrice(string stockName,decimal newStockPrice, WebSocketConnectionManager manager) {
            var stock = context.Stocks.Find(stockName);
            stock.HistoricPrice.Add(new StockPrice(newStockPrice, DateTime.Now));
            context.Stocks.Update(stock);
            var updateTask = new Thread(async () => await manager.UpdateAllClients(GetStocks()));
            updateTask.Start();
        }

        public List<Stock> GetStocks() {
            return context.Stocks.Include(s => s.HistoricPrice).Include(s => s.Seller).ToList();
        }
        
        public Stock GetStock(string name) {
            return context.Stocks.Include(s => s.HistoricPrice).Include(s => s.Seller).FirstOrDefault(s => s.Name == name);
        }
        
        public void UpdateStock(Stock stock, WebSocketConnectionManager manager) {
            Console.WriteLine(JsonSerializer.Serialize(stock));
            context.Stocks.Update(stock);
            context.SaveChanges();
            var updateTask = new Thread(async () => await manager.UpdateAllClients(GetStocks()));
            updateTask.Start();
        }
        
        public Seller GetSeller(string name, int number) {
            Stock stock = GetStock(name);
            Seller seller = stock.Seller.FirstOrDefault(s => s.SellingAmount >= number);
            if (seller == null ) {
                return null;
            }
            return seller;
        }

        public bool SellStock(string name, int number) {
            Stock stock = GetStock(name);
            Seller seller = stock.Seller.FirstOrDefault(s => s.SellingAmount >= number);
            if (seller == null ) {
                return false;
            }

            stock.Seller.Remove(seller);
            seller.SellingAmount = seller.SellingAmount - number;
            stock.Seller.Add(seller);
            context.Stocks.Update(stock);
            context.SaveChanges();
            return true;
        }

    }
}
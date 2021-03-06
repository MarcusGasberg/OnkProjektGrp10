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
        private WebSocketConnectionManager manager;

        public Commands(ApplicationDbContext context, WebSocketConnectionManager manager)
        {
            this.context = context;
            this.manager = manager;
        }
        public void AddNewStock(Stock stock)
        {
            Console.WriteLine(JsonSerializer.Serialize(stock));
            context.Stocks.Add(stock);
            context.SaveChanges();

            UpdateClients();

        }

        public void UpdateClients()
        {
            var allStocks = GetStocks();
            manager.UpdateAllClients(allStocks);
        }

        public void UpdateStockPrice(string stockName, int newStockPrice)
        {
            var stock = context.Stocks.Find(stockName);
            stock.HistoricPrice.Add(new StockPrice(newStockPrice, DateTime.Now));
            context.Stocks.Update(stock);
            UpdateClients();
        }

        public List<Stock> GetStocks()
        {
            return context.Stocks.Include(s => s.HistoricPrice).Include(s => s.Seller).ToList();
        }

        public Stock GetStock(string name)
        {
            return context.Stocks.Include(s => s.HistoricPrice).Include(s => s.Seller).FirstOrDefault(s => s.Name == name);
        }

        public List<Stock> GetUserStock(string userid)
        {
            return context.Stocks.Where(e => e.Seller.FirstOrDefault(s => s.SellerId == userid) != null).Include(s => s.HistoricPrice).Include(s => s.Seller).ToList();
        }

        public void UpdateStock(Stock stock)
        {
            Console.WriteLine(JsonSerializer.Serialize(stock));
            context.Stocks.Update(stock);
            context.SaveChanges();
            UpdateClients();
        }

        public Seller GetSeller(string name, int number)
        {
            Stock stock = GetStock(name);
            Seller seller = stock.Seller.FirstOrDefault(s => s.SellingAmount >= number);
            if (seller == null)
            {
                return null;
            }
            return seller;
        }

        public bool SellStock(string name, int number, string sellerid)
        {
            Stock stock = GetStock(name);
            context.Attach(stock);
            Seller seller = stock.Seller.FirstOrDefault(s => s.SellerId == sellerid);
            if (seller == null)
            {
                return false;
            }

            seller.SellingAmount += number;
            context.SaveChanges();
            return true;
        }

        public bool BuyStock(string name, int number)
        {
            Stock stock = GetStock(name);
            context.Attach(stock);
            Seller seller = stock.Seller.FirstOrDefault(s => s.SellingAmount >= number);
            if (seller == null)
            {
                return false;
            }
            seller.SellingAmount -= number;
            context.SaveChanges();
            return true;
        }

    }
}
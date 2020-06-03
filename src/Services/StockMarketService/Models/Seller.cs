namespace StockMarketService.Models {
    public class Seller {
        public string Id { get; set; }
        public string SellerId { get; set; }
        public int SellingAmount { get; set; }
        public Stock Stock { get; set; }
    }
}
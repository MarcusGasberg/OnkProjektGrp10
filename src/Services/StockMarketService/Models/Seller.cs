using System.ComponentModel.DataAnnotations.Schema;

namespace StockMarketService.Models {
    public class Seller {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string SellerId { get; set; }
        public int SellingAmount { get; set; }
        public Stock Stock { get; set; }
    }
}
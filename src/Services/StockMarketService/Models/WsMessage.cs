using System.Diagnostics.CodeAnalysis;
namespace StockMarketService.Models {
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class WsMessage {
        public string topic;
        public object? data;
        public string? action;
    }
}
using System;
using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StockMarketService.Middleware {
    public class WebSocketConnectionManager {
        private ConcurrentDictionary<string, WebSocket> _sockets = new ConcurrentDictionary<string, WebSocket>();

        public ConcurrentDictionary<string, WebSocket> GetAllSockets() {
            return _sockets;
        }

        public string AddSocket(WebSocket socket) {
            var id = Guid.NewGuid().ToString();
            _sockets.TryAdd(id, socket);
            return id;
        }
        
        public async Task UpdateAllClients(string msg) {
            var buffer = Encoding.UTF8.GetBytes(msg);
            foreach (var keyValue in _sockets) {
                await keyValue.Value.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }
    }
}
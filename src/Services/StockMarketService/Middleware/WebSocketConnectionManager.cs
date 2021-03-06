using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using StockMarketService.Models;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace StockMarketService.Middleware {
    public class WebSocketConnectionManager {
        private ConcurrentDictionary<string, WebSocket> _sockets = new ConcurrentDictionary<string, WebSocket>();

        public ConcurrentDictionary<string, WebSocket> GetAllSockets() {
            return _sockets;
        }

        public string AddSocket(WebSocket socket) {
            var id = Guid.NewGuid().ToString();
            Console.WriteLine("Adding");
            _sockets.TryAdd(id, socket);
            return id;
        }

        public async Task SendMessage(string wsId, string topic, string action, object data) {
            GetAllSockets().TryGetValue(wsId, out WebSocket socket);
            var msg = new WsMessage() {
                data = data,
                topic = topic,
                action = action
            };
            var json = JsonConvert.SerializeObject(msg, Formatting.None, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            var buffer = Encoding.UTF8.GetBytes(json);
            if (socket != null) await socket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
        }

        public void UpdateAllClients(List<Stock> stocks) {

            var msg = new WsMessage() {
                data = stocks,
                topic = "stocks",
                action = "update"
            };

            var json = JsonConvert.SerializeObject(msg, Formatting.None, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            var buffer = Encoding.UTF8.GetBytes(json);
            
            foreach (var keyValue in _sockets) {
                keyValue.Value.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }
        
        public async Task<WebSocket> RemoveSocket(WebSocket webSocket, WebSocketReceiveResult result) {
            var id = GetAllSockets().FirstOrDefault(socket => socket.Value == webSocket).Key;
            GetAllSockets().TryRemove(id, out var socket);
            return socket;
        }
    }
}
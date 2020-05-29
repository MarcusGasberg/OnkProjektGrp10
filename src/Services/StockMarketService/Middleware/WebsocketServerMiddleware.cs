using System;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using StockMarketService.Models;

namespace StockMarketService.Middleware {
    public class WebsocketServerMiddleware {
        private readonly RequestDelegate _next;
        private readonly WebSocketConnectionManager _manager;
        private Commands commands;

        public WebsocketServerMiddleware(RequestDelegate next, WebSocketConnectionManager manager, ApplicationDbContext dbContext)
        {
            this.commands = new Commands(dbContext);
            _next = next;
            _manager = manager;
        }

        public async Task InvokeAsync(HttpContext context) {
            if (context.WebSockets.IsWebSocketRequest) {
                var webSocket = await context.WebSockets.AcceptWebSocketAsync();
                Console.WriteLine("Connected");
                await HandleWebsocketMessage(webSocket, async (result, buffer) => {
                    switch (result.MessageType) {
                        case WebSocketMessageType.Text:
                            await HandleMessage(webSocket, buffer, result);
                            break;
                        case WebSocketMessageType.Close:
                            await HandleCloseMessage(webSocket, result);
                            break;
                        case WebSocketMessageType.Binary:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                });
            }

            else {
                await _next(context);
            }
        }

        private async Task HandleMessage(WebSocket webSocket, byte[] buffer, WebSocketReceiveResult result) {
            var parse = Encoding.UTF8.GetString(buffer, 0, result.Count);

            WsMessage msg = JsonConvert.DeserializeObject<WsMessage>(parse);
            if (msg.topic == "stocks" && msg.action == "subscribe") {
                var id = _manager.AddSocket(webSocket);
                await _manager.SendMessage(id, msg.topic, commands.GetStocks());
            }
        }

        private async Task HandleCloseMessage(WebSocket webSocket, WebSocketReceiveResult result) {
            var socket = await _manager.RemoveSocket(webSocket, result);
            if (result.CloseStatus != null) {
                await socket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription,
                    CancellationToken.None);
            }
        }

        private async Task HandleWebsocketMessage(WebSocket webSocket, Action<WebSocketReceiveResult, byte[]> handle) {
            var buffer = new byte[1024 * 4];
            while (webSocket.State == WebSocketState.Open) {
                var result =
                    await webSocket.ReceiveAsync(buffer: new ArraySegment<byte>(buffer), CancellationToken.None);

                handle(result, buffer);

            }
        }
    }
}
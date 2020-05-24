using System;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace StockMarketService.Middleware {
    public class WebsocketServerMiddleware {
        private readonly RequestDelegate _next;
        private readonly WebSocketConnectionManager _manager;

        public WebsocketServerMiddleware(RequestDelegate next, WebSocketConnectionManager manager) {
            _next = next;
            _manager = manager;
        }

        public async Task InvokeAsync(HttpContext context) {
            if (context.WebSockets.IsWebSocketRequest) {
                var webSocket = await context.WebSockets.AcceptWebSocketAsync();
                Console.WriteLine("Connected");
                _manager.AddSocket(webSocket);
                await HandleWebsocketMessage(webSocket, async (result, buffer) => {
                    switch (result.MessageType) {
                        case WebSocketMessageType.Text:
                            Console.WriteLine("Message Received: " + Encoding.UTF8.GetString(buffer, 0, result.Count));
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

        private async Task HandleCloseMessage(WebSocket webSocket, WebSocketReceiveResult result) {
            var id = _manager.GetAllSockets().FirstOrDefault(socket => socket.Value == webSocket).Key;
            _manager.GetAllSockets().TryRemove(id, out var socket);
            if (result.CloseStatus != null)
                await socket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription,
                    CancellationToken.None);
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
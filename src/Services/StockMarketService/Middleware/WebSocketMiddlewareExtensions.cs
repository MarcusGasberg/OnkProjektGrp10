using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace StockMarketService.Middleware {
    public static class WebSocketMiddlewareExtensions {
        public static IApplicationBuilder SetupWebsocketServer(this IApplicationBuilder builder) {
            return builder.UseMiddleware<WebsocketServerMiddleware>();
        }

        public static IServiceCollection AddWebsocketManager(this IServiceCollection serviceCollection) {
            serviceCollection.AddSingleton<WebSocketConnectionManager>();
            return serviceCollection;
        }
    }
}
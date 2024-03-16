using CryptoClient.Application.Interfaces;
using CryptoClient.Infrastructure.WebSocketTickerTakers;
using Microsoft.Extensions.DependencyInjection;

namespace CryptoClient.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddSingleton<IWebSocketTickerTaker, BinanceTickerTaker>();
            services.AddSingleton<IWebSocketTickerTaker, BybitTickerTaker>();
            services.AddSingleton<IWebSocketTickerTaker, KucoinTickerTaker>();
            services.AddSingleton<IWebSocketTickerTaker, BitgetTickerTaker>();
            return services;
        }
    }
}

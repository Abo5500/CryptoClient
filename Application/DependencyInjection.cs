using Microsoft.Extensions.DependencyInjection;
using CryptoClient.Application.Interfaces;
using CryptoClient.Application.ExchangeServices;

namespace CryptoClient.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IExchangesService, ExchangeService>();
            return services;
        }
    }
}

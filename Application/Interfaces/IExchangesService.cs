using CryptoClient.Application.Tickers;

namespace CryptoClient.Application.Interfaces
{
    public interface IExchangesService
    {
        /// <summary>
        /// Subscribe to all exchanges
        /// </summary>
        public Task SubscribeToSpotTickerUpdatesAsync(string tickerName, EventHandler<TokenTicker> eventHandler);
        /// <summary>
        /// Subscribe to a specific exchange
        /// </summary>
        public Task SubscribeToSpotTickerUpdatesAsync(string tickerName, string exchangeName, EventHandler<TokenTicker> eventHandler);
        /// <summary>
        /// Unsubscribe from all exchanges
        /// </summary>
        public Task UnsubscribeFromSpotTickerUpdatesAsync(string tickerName, EventHandler<TokenTicker> eventHandler);
        /// <summary>
        /// Unsubscribe from a specific exchange
        /// </summary>
        public Task UnsubscribeFromSpotTickerUpdatesAsync(string tickerName, string exchangeName, EventHandler<TokenTicker> eventHandler);

        public Task<List<SpotList>> GetAvailableSpots();
    }
}

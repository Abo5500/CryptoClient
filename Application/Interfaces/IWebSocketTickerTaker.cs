using CryptoClient.Application.Tickers;

namespace CryptoClient.Application.Interfaces
{
    public interface IWebSocketTickerTaker
    {
        public Task SubscribeToSpotTickerUpdatesAsync(string tickerName, EventHandler<TokenTicker> eventHandler);

        public Task UnsubscribeFromSpotTickerUpdatesAsync(string tickerName, EventHandler<TokenTicker> eventHandler);
        public Task<SpotList> GetAvailableSpots();
    }
}

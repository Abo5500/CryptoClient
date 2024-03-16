using CryptoClient.Application.Tickers;
using CryptoClient.Application.Interfaces;

namespace CryptoClient.Application.ExchangeServices
{
    public class ExchangeService : IExchangesService
    {
        private IEnumerable<IWebSocketTickerTaker> _tickerTakers;
        public ExchangeService(IEnumerable<IWebSocketTickerTaker> tickerTakers)
        {
            _tickerTakers = tickerTakers;
        }

        public async Task<List<SpotList>> GetAvailableSpots()
        {
            var spots = await Task.WhenAll(_tickerTakers.Select(x => x.GetAvailableSpots()));
            return spots.ToList();
        }

        public async Task SubscribeToSpotTickerUpdatesAsync(string tickerName, EventHandler<TokenTicker> eventHandler)
        {
            await Task.WhenAll(_tickerTakers.Select(x => x.SubscribeToSpotTickerUpdatesAsync(tickerName, eventHandler)));
        }

        public Task SubscribeToSpotTickerUpdatesAsync(string tickerName, string exchangeName, EventHandler<TokenTicker> eventHandler)
        {
            throw new NotImplementedException();
        }

        public async Task UnsubscribeFromSpotTickerUpdatesAsync(string tickerName, EventHandler<TokenTicker> eventHandler)
        {
            await Task.WhenAll(_tickerTakers.Select(x => x.UnsubscribeFromSpotTickerUpdatesAsync(tickerName, eventHandler)));
        }

        public Task UnsubscribeFromSpotTickerUpdatesAsync(string tickerName, string exchangeName, EventHandler<TokenTicker> eventHandler)
        {
            throw new NotImplementedException();
        }
    }
}

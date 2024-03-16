using CryptoClient.Application.Interfaces;
using CryptoClient.Application.Tickers;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;

namespace CryptoClient.Infrastructure.WebSocketTickerTakers.Base
{
    public abstract class BaseTickerTaker: IWebSocketTickerTaker
    {
        protected readonly Dictionary<string, int> TickerSubscriptions = new();
        protected readonly Dictionary<string, List<EventHandler<TokenTicker>>> TickerUpdateHandlers = new();
        public abstract string ExchangeName { get; }

        public async Task SubscribeToSpotTickerUpdatesAsync(string token, EventHandler<TokenTicker> eventHandler)
        {
            if (TickerSubscriptions.ContainsKey(token) && TickerUpdateHandlers.ContainsKey(token))
            {
                TickerUpdateHandlers[token].Add(eventHandler);
                return;
            }
            if(!TickerUpdateHandlers.ContainsKey(token))
            {
                TickerUpdateHandlers.Add(token, new List<EventHandler<TokenTicker>> { eventHandler });
            }
            var callResult = await ExchangeSubscribeToSpotTickerUpdatesAsync(token);
            if(callResult.Data is not  null)
            {
                TickerSubscriptions.Add(token, callResult.Data.Id);
            }
        }
        protected abstract Task<CallResult<UpdateSubscription>> ExchangeSubscribeToSpotTickerUpdatesAsync(string token);
        protected abstract Task ExchangeUnsubscribeFromSpotTickerUpdatesAsync(int subscriptionId);
        public async Task UnsubscribeFromSpotTickerUpdatesAsync(string tickerName, EventHandler<TokenTicker> eventHandler)
        {
            var handlers = TickerUpdateHandlers.GetValueOrDefault(tickerName);
            if (handlers != null && handlers.Contains(eventHandler))
            {
                TickerUpdateHandlers[tickerName].Remove(eventHandler);
            }
            if (TickerUpdateHandlers.ContainsKey(tickerName) && TickerUpdateHandlers[tickerName].Count == 0)
            {
                if (TickerSubscriptions.ContainsKey(tickerName))
                {
                    await ExchangeUnsubscribeFromSpotTickerUpdatesAsync(TickerSubscriptions[tickerName]);
                    TickerSubscriptions.Remove(tickerName);
                }
                TickerUpdateHandlers.Remove(tickerName);
            }
        }
        protected void HandleTickerUpdate(TokenTicker ticker)
        {
            //Срабатывает апдейт во время того, как удаляется токен из списка, придумать решение без try catch
            try
            {
                foreach (var handler in TickerUpdateHandlers[ticker.Token])
                {
                    handler.Invoke(this, ticker);
                }
            }
            catch
            {

            }
        }

        public abstract Task<SpotList> GetAvailableSpots();
    }
}

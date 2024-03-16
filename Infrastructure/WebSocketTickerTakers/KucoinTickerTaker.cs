using CryptoClient.Application.Tickers;
using Kucoin.Net.Clients;
using CryptoExchange.Net;
using CryptoClient.Infrastructure.WebSocketTickerTakers.Base;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;

namespace CryptoClient.Infrastructure.WebSocketTickerTakers
{
    public class KucoinTickerTaker : BaseTickerTaker
    {
        protected KucoinSocketClient SocketClient = new KucoinSocketClient();
        public override string ExchangeName => "Kucoin";

        public override async Task<SpotList> GetAvailableSpots()
        {
            using KucoinRestClient kucoinRestClient = new KucoinRestClient();
            var symbols = await kucoinRestClient.SpotApi.ExchangeData.GetSymbolsAsync();
            return new SpotList() { ExchangeName = ExchangeName, Spots = symbols.Data.Select(x => x.Name) };
        }

        protected override async Task<CallResult<UpdateSubscription>> ExchangeSubscribeToSpotTickerUpdatesAsync(string token)
        {
            return await SocketClient.SpotApi.SubscribeToTickerUpdatesAsync(token.Replace("/","-"), update =>
            {
                HandleTickerUpdate(new TokenTicker() { Exchange = ExchangeName, Token = token, Price = (decimal)update.Data.LastPrice?.Normalize() });
            });
        }

        protected override async Task ExchangeUnsubscribeFromSpotTickerUpdatesAsync(int subscriptionId)
        {
           await SocketClient.SpotApi.UnsubscribeAsync(subscriptionId);
        }
    }
}

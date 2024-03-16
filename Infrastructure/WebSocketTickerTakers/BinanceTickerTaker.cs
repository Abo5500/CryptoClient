using Binance.Net.Clients;
using CryptoClient.Application.Tickers;
using CryptoClient.Infrastructure.WebSocketTickerTakers.Base;
using CryptoExchange.Net;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;

namespace CryptoClient.Infrastructure.WebSocketTickerTakers
{
    public class BinanceTickerTaker : BaseTickerTaker
    {
        protected BinanceSocketClient SocketClient = new BinanceSocketClient();

        public override string ExchangeName => "Binance";

        public override async Task<SpotList> GetAvailableSpots()
        {
            var symbols = await SocketClient.SpotApi.ExchangeData.GetExchangeInfoAsync();
            return new SpotList() { ExchangeName = ExchangeName, Spots = symbols.Data.Result.Symbols.Select(x => x.Name) };
        }

        protected override async Task<CallResult<UpdateSubscription>> ExchangeSubscribeToSpotTickerUpdatesAsync(string token)
        {
            return await SocketClient.SpotApi.ExchangeData.SubscribeToTickerUpdatesAsync(token.Replace("/", ""), update =>
            {
                HandleTickerUpdate(new TokenTicker() { Exchange = ExchangeName, Token = token, Price = update.Data.LastPrice.Normalize() });
            });
        }

        protected override async Task ExchangeUnsubscribeFromSpotTickerUpdatesAsync(int subscriptionId)
        {
            await SocketClient.SpotApi.UnsubscribeAsync(subscriptionId);
        }
    }
}

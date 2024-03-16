using Bybit.Net.Clients;
using CryptoClient.Application.Tickers;
using CryptoClient.Infrastructure.WebSocketTickerTakers.Base;
using CryptoExchange.Net;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;

namespace CryptoClient.Infrastructure.WebSocketTickerTakers
{
    public class BybitTickerTaker : BaseTickerTaker
    {
        protected BybitSocketClient SocketClient = new BybitSocketClient();
        public override string ExchangeName => "Bybit";

        public override async Task<SpotList> GetAvailableSpots()
        {
            using BybitRestClient client = new BybitRestClient();
            var symbols = await client.V5Api.ExchangeData.GetSpotSymbolsAsync();
            return new SpotList() { ExchangeName = ExchangeName, Spots = symbols.Data.List.Select(x => x.Name) };
        }

        protected override async Task<CallResult<UpdateSubscription>> ExchangeSubscribeToSpotTickerUpdatesAsync(string token)
        {
            return await SocketClient.V5SpotApi.SubscribeToTickerUpdatesAsync(token.Replace("/", ""), update =>
            {
                HandleTickerUpdate(new TokenTicker() { Exchange = ExchangeName, Token = token, Price = update.Data.LastPrice.Normalize() });
            });
        }

        protected override async Task ExchangeUnsubscribeFromSpotTickerUpdatesAsync(int subscriptionId)
        {
            await SocketClient.V5SpotApi.UnsubscribeAsync(subscriptionId);
        }
    }
}

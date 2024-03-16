using Bitget.Net.Clients;
using CryptoClient.Application.Tickers;
using CryptoClient.Infrastructure.WebSocketTickerTakers.Base;
using CryptoExchange.Net;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;

namespace CryptoClient.Infrastructure.WebSocketTickerTakers
{
    public class BitgetTickerTaker : BaseTickerTaker
    {
        protected BitgetSocketClient SocketClient = new BitgetSocketClient();
        public override string ExchangeName => "Bitget";

        public override async Task<SpotList> GetAvailableSpots()
        {
            using BitgetRestClient client = new BitgetRestClient();
            var symbols = await client.SpotApi.ExchangeData.GetSymbolsAsync();
            return new SpotList() { ExchangeName = ExchangeName, Spots = symbols.Data.Select(x => x.Name) };
        }

        protected override async Task<CallResult<UpdateSubscription>> ExchangeSubscribeToSpotTickerUpdatesAsync(string token)
        {
            return await SocketClient.SpotApi.SubscribeToTickerUpdatesAsync(token.Replace("/", ""), update =>
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

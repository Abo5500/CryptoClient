namespace CryptoClient.Application.Tickers
{
    public class SpotList
    {
        public string ExchangeName { get; set; }
        public IEnumerable<string> Spots { get; set; }
    }
}

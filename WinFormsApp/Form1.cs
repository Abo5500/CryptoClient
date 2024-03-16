using CryptoClient.Application.Interfaces;
using CryptoClient.Application.Tickers;
using CryptoClient.WinFormsApp.Extensions;
using System.ComponentModel;

namespace CryptoClient.WinFormsApp
{
    //TODO: рефакторинг, сделать выпадающий список спотов (в сервисе реализовано),
    //сделать добавление и удаление спота только с определенной биржи (в сервисе реализовано)
    //Не добавлять в список биржи тикер, который не поддерживается (проверять в списке всех доступных токенов бирж (_ExchangeService.GetAvailableSpots()))
    public partial class Form1 : Form
    {
        private readonly Dictionary<string, BindingList<TokenTicker>> TokenTickers = new()
        {
            { "Binance", new BindingList<TokenTicker>() },
            { "Bybit", new BindingList<TokenTicker> () },
            { "Kucoin", new BindingList<TokenTicker> () },
            { "Bitget", new BindingList<TokenTicker> () },
        };
        private readonly IExchangesService _exchangesService;
        public Form1(IExchangesService exchangesService)
        {
            InitializeComponent();
            _exchangesService = exchangesService;
            dataGridView1.InitializeExchanges(TokenTickers["Binance"]);
            dataGridView2.InitializeExchanges(TokenTickers["Bybit"]);
            dataGridView3.InitializeExchanges(TokenTickers["Kucoin"]);
            dataGridView4.InitializeExchanges(TokenTickers["Bitget"]);
        }

        private void SetPrice(object? sender, TokenTicker tokenTicker)
        {
            var token = TokenTickers[tokenTicker.Exchange].FirstOrDefault(x => x.Token == tokenTicker.Token);
            if (token is null)
                return; //Sdelat' tut unsubscribe
            token.Price = tokenTicker.Price;
            switch (token.Exchange)
            {
                case "Binance":
                    Invoke(() => dataGridView1.UpdateCellValue(1, TokenTickers[tokenTicker.Exchange].IndexOf(token)));
                    break;
                case "Bybit":
                    Invoke(() => dataGridView2.UpdateCellValue(1, TokenTickers[tokenTicker.Exchange].IndexOf(token)));
                    break;
                case "Kucoin":
                    Invoke(() => dataGridView3.UpdateCellValue(1, TokenTickers[tokenTicker.Exchange].IndexOf(token)));
                    break;
                case "Bitget":
                    Invoke(() => dataGridView4.UpdateCellValue(1, TokenTickers[tokenTicker.Exchange].IndexOf(token)));
                    break;
            }
        }
        private async Task AddTickerSubscribeToAllAsync(string name)
        {
            foreach (var exchange in TokenTickers)
            {
                var token = exchange.Value.FirstOrDefault(x => x.Token == name);

                if (token is null)
                {
                    exchange.Value.Add(new TokenTicker() { Token = name, Exchange = exchange.Key });
                }
            }
            await _exchangesService.SubscribeToSpotTickerUpdatesAsync(name, SetPrice);
        }
        private async void button1_Click(object sender, EventArgs e)
        {
            await AddTickerSubscribeToAllAsync(textBox1.Text);
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            await _exchangesService.UnsubscribeFromSpotTickerUpdatesAsync(textBox2.Text, SetPrice);
            foreach (var exchange in TokenTickers)
            {
                var token = exchange.Value.FirstOrDefault(x => x.Token == textBox2.Text);
                if (token is not null)
                {
                    exchange.Value.Remove(token);
                }
            }
        }
    }
}

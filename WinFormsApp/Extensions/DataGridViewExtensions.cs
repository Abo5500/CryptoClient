using CryptoClient.Application.Tickers;
using System.ComponentModel;

namespace CryptoClient.WinFormsApp.Extensions
{
    public static class DataGridViewExtensions
    {
        public static DataGridView InitializeExchanges(this DataGridView dataGridView, BindingList<TokenTicker> tokenTickers)
        {
            dataGridView.DataSource = tokenTickers;
            dataGridView.ReadOnly = true;
            dataGridView.Columns["Token"].Width = 84;
            dataGridView.Columns["Price"].Width = 144;
            dataGridView.Columns["Exchange"].Visible = false;
            return dataGridView;
        }
    }
}

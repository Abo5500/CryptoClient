using CryptoClient.Application;
using CryptoClient.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
namespace CryptoClient.WinFormsApp
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            HostApplicationBuilder builder = Host.CreateApplicationBuilder();
            builder.Services.AddApplicationServices();
            builder.Services.AddInfrastructureServices();
            builder.Services.AddTransient<Form1>();
            using IHost host = builder.Build();
            System.Windows.Forms.Application.Run(host.Services.GetRequiredService<Form1>());
            
            
        }
    }
}
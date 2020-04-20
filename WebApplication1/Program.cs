using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging.Console;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace WebApplication1
{
    public class Program
    {

        public static void Main(string[] args)
        {
            HubConnection connection = new HubConnectionBuilder().WithUrl("http://localhost:5000/chatHub").Build();

            Random random = new Random();

            int id = random.Next(1, 1000);

            string user = $"client{id}";

            connection.On("ReceiveMessage", (string user, string message) =>
            {
                Console.WriteLine($"{user}: {message}");
            });

            connection.On("Typing", (string user, string message) =>
            {
                Console.WriteLine($"{user} is typing: {message}");
            });


            Task host;

            while (true)
            {
                connection.StartAsync();
                if (connection.State == HubConnectionState.Connected)
                {
                    char letter;
                    string message = "";
                    do
                    {

                        letter = Console.ReadKey().KeyChar;
                        message += letter;
                        connection.InvokeAsync("Typing", user, message);


                    } while (letter != '\r');

                    connection.InvokeAsync("SendMessage", user, message);
                }
                else
                {
                    host = CreateHostBuilder(args).Build().RunAsync();
                    Console.Clear();
                    if (host.Status == TaskStatus.Faulted)
                        Console.WriteLine("Connected.");
                    else
                        Console.WriteLine("You're hosting now.");
                    Thread.Sleep(1500);
                }
                connection.StartAsync();
            }
        }



            public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}

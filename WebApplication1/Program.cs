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
            connection.On("AddLetter", (char letter) =>
            {
                Console.Write(letter);
            });
            connection.On("DeleteLetter", (string userName) =>
            {
                int x;
                if (userName != user)               //при нажатии backspace у вызывающего в консоли курсор 
                    x = Console.CursorLeft - 1;     //автоматически переводится на 1 пункт влево
                else
                    x = Console.CursorLeft;
                int y = Console.CursorTop;
                Console.SetCursorPosition(x, y);
                Console.Write(' ');
                Console.SetCursorPosition(x, y);
            });

            Task check;
            int freezeDelay = random.Next(300, 400);

            while (true)
            {
                connection.StartAsync();
                Thread.Sleep(500); //чтобы соединение успело установиться
                while (connection.State == HubConnectionState.Connected)
                {
                    char letter;
                    string message = "";
                    do
                    {
                        letter = Console.ReadKey().KeyChar;
                        if(letter == '\b')
                        {
                            check = connection.InvokeAsync("DeleteLetter", user);
                            Thread.Sleep(100);
                            if(check.Status == TaskStatus.Faulted)
                                break;
                        }
                        else
                        {
                            check = connection.InvokeAsync("AddLetter", letter);
                            Thread.Sleep(100);
                            if (check.Status == TaskStatus.Faulted)
                                break;
                            message += letter;
                        }

                    } while (letter != '\r');

                    if (check.Status == TaskStatus.Faulted)
                        break;

                    connection.InvokeAsync("SendMessage", user, message);
                }
                try
                {
                    Thread.Sleep(freezeDelay);
                    check = CreateHostBuilder(args).Build().RunAsync();
                    Console.Clear();
                    if (check.Status == TaskStatus.Faulted)
                        Console.WriteLine("Connected.");
                    else
                        Console.WriteLine("You're hosting now.");
                    Thread.Sleep(1500);
                }
                catch
                {
                    connection.StartAsync();
                }
            }
        }

        public static void StartHost(string[] args)
        {

        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
    }
}

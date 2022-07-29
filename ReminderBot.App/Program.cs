using System;
using System.Reflection;
using System.Threading.Tasks;
using AsyncAwaitBestPractices;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using ReminderBot.App.Repositories;
using ReminderBot.App.Services;

namespace ReminderBot.App
{
    class Program
    {
        static void Main(string[] args)
        {
            RunBotAsync().GetAwaiter().GetResult();
            Console.ReadKey();
        }

        private static DiscordSocketClient _client;
        private static CommandService _commands;
        private static IServiceProvider _services;

        public static async Task RunBotAsync()
        {
            var reminderRepository = new ReminderRepository();
            
            _client = new DiscordSocketClient();
            _commands = new CommandService();
            _services = new ServiceCollection()
                .AddSingleton(_client)
                .AddSingleton(_commands)
                .AddSingleton(_ => reminderRepository)
                .BuildServiceProvider();

            new ReminderService(reminderRepository).CheckReminders().SafeFireAndForget();
            
            const string token = "MTAwMjM3NDk2MDEyOTUwNzM1OA.G-wX0z.9NtiulwvH5q5ljLlrqBYYpqaRsHx67ohwKNn60";

            _client.Log += Log;

            await RegisterCommandsAsync();

            await _client.LoginAsync(TokenType.Bot, token);

            await _client.StartAsync();

            await Task.Delay(-1);
        }

        private static Task Log(LogMessage arg)
        {
            Console.WriteLine(arg);
            return Task.CompletedTask;
        }
        
        private static async Task RegisterCommandsAsync()
        {
            _client.MessageReceived += HandleCommandAsync;
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
        }

        private static async Task HandleCommandAsync(SocketMessage socketMessage)
        {
            var message = socketMessage as SocketUserMessage;
            var context = new SocketCommandContext(_client, message);
            if (message.Author.IsBot) return;

            int argPos = 0;
            if (message.HasStringPrefix("reminderbot ", ref argPos))
            {
                await Execute();
                return;
            }
            argPos = 0;
            if (message.HasStringPrefix("remindme ", ref argPos))
            {
                await Execute();
                return;
            }
            argPos = 0;
            if (message.HasStringPrefix("<@1002374960129507358> ", ref argPos))
            {
                await Execute();
                return;
            }

            async Task Execute()
            {
                var result = await _commands.ExecuteAsync(context, argPos, _services);
                if (!result.IsSuccess)
                    Console.WriteLine(result.ErrorReason);
            }
        }
    }
}
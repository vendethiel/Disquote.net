using System;
using System.Linq;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using Microsoft.Extensions.DependencyInjection;

namespace Disquote.net
{
    class Program
    {
        static void Main(string[] args)
        {
            MainAsync().GetAwaiter().GetResult();
        }

        static async Task MainAsync()
        {
            var token = Environment.GetEnvironmentVariable("TOKEN");
            if (token == null)
                throw new ArgumentNullException("TOKEN", "Missing TOKEN environment variable");
            
            var bot = new DiscordClient(new DiscordConfiguration()
            {
                Token = token,
                TokenType = TokenType.Bot,
                Intents = DiscordIntents.AllUnprivileged
            });

            var services = new ServiceCollection()
                .AddSingleton<Manager.QuoteManager>()
                .BuildServiceProvider();
            
            var commands = bot.UseCommandsNext(new CommandsNextConfiguration()
            {
                StringPrefixes = new[] { "q!" },
                Services = services
            });
            commands.RegisterCommands<Commands.QuoteCommands>();
            // commands.RegisterCommands(Assembly.GetExecutingAssembly());

            await bot.ConnectAsync();
            await Task.Delay(-1);
        }   

    }
}
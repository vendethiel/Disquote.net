using System;
using System.Linq;
using System.Threading.Tasks;
using Disquote.net.Data;
using Disquote.net.Manager;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Disquote.net
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            var token = Environment.GetEnvironmentVariable("TOKEN");
            if (token == null)
                throw new ArgumentNullException("TOKEN", "Missing TOKEN environment variable");
            
            var bot = new DiscordClient(new DiscordConfiguration()
            {
                Token = token,
                TokenType = TokenType.Bot,
                Intents = DiscordIntents.AllUnprivileged | DiscordIntents.MessageContents,
                MinimumLogLevel = LogLevel.Debug
            });

            bot.ClientErrored += async (_, eventArgs) => bot.Logger.LogCritical(eventArgs.Exception, "client error");

            var context = new Context();
            var qm = new QuoteManager(context);
            
            var services = new ServiceCollection()
                .AddSingleton(qm)
                .BuildServiceProvider();
            
            var commands = bot.UseCommandsNext(new CommandsNextConfiguration()
            {
                StringPrefixes = new[] { "q!" },
                Services = services,
            });
            commands.CommandErrored += async (_, eventArgs) => bot.Logger.LogCritical(eventArgs.Exception, "command error");
            commands.RegisterCommands<Commands.QuoteCommands>();
            // commands.RegisterCommands(Assembly.GetExecutingAssembly());

            await bot.ConnectAsync();
            await Task.Delay(-1);
        }   

    }
}
using System;
using System.Linq;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace Disquote.net.Commands
{
    public class QuoteCommands : BaseCommandModule
    {
        public Manager.QuoteManager Manager { private get; set; }
        
        [Command("add")]
        public async Task AddMultiCommand(CommandContext context, [RemainingText] string quote)
        {
            var id = Manager.AddMulti(quote);
            await context.RespondAsync("Added quote #" + id);
        }

        [Command("add")]
        public async Task AddMultiCommand(CommandContext context, DiscordUser quotee, [RemainingText] string quote)
        {
            var id = Manager.AddTargeted(quotee.Username, quote);
            await context.RespondAsync("Added quote #" + id);
        }

        [Command("search")]
        public async Task SearchCommand(CommandContext context, [RemainingText] string search)
        {
            var indices = Manager.Search(search);
            switch (indices.Count)
            {
                case 0:
                    await context.RespondAsync("No quote found");
                    break;
                
                case 1:
                    var id = indices[0];
                    var quote = Manager.Get(id);
                    await context.RespondAsync("Found quote #" + id + "\n" + MessageUtil.WrapInQuoteBlock(quote));
                    break;
                
                default:
                    var found = String.Join(", ", indices.Select(i => "#" + (i + 1)));
                    await context.RespondAsync("Found " + indices.Count + " quotes: " + found);
                    break;
            }
        }

        [Command("remove")]
        public async Task RemoveCommand(CommandContext context) { }

        [Command("show")]
        public async Task ShowCommand(CommandContext context, int id)
        {
            var quote = Manager.Get(id);
            if (quote != null)
                await context.RespondAsync("Quote #" + id + "\n" + MessageUtil.WrapInQuoteBlock(quote));
            else
                await context.RespondAsync("Quote #" + id + " does not exist");
        }
    }
}
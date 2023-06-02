using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Disquote.net.Data;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace Disquote.net.Commands
{
    public class QuoteCommands : BaseCommandModule
    {
        public Manager.QuoteManager Manager { private get; set; }
        
        [Command("add")]
        public async Task AddMultiCommand(CommandContext context, [RemainingText] string text)
        {
            var id = Manager.AddMulti(context.Guild, context.Channel, context.Member, text);
            await context.RespondAsync("Added quote #" + id);
        }

        [Command("add")]
        public async Task AddMultiCommand(CommandContext context, DiscordUser quotee, [RemainingText] string quote)
        {
            var id = Manager.AddTargeted(context.Guild, context.Channel, context.Member, quotee, quote);
            await context.RespondAsync("Added quote #" + id);
        }

        [Command("search")]
        public async Task SearchCommand(CommandContext context, [RemainingText] string search)
        {
            var indices = Manager.Search(context.Guild, search);
            switch (indices.Count)
            {
                case 0:
                    await context.RespondAsync("No quote found");
                    break;
                
                case 1:
                    var id = indices[0];
                    var quote = Manager.Get(context.Guild, id);
                    if (quote != null)
                        await context.RespondAsync("Found quote #" + id + "\n" + Stringify(context, quote));
                    else // Should never happen, except maybe in a race condition
                        await context.RespondAsync("Quote #" + id + " not found!");
                    break;
                
                default:
                    var found = String.Join(", ", indices.Select(i => "#" + (i + 1)));
                    await context.RespondAsync("Found " + indices.Count + " quotes: " + found);
                    break;
            }
        }

        [Command("remove")]
        public async Task RemoveCommand(CommandContext context, int id)
        {
            // Manager.Delete(context.Guild, id);
        }

        [Command("show")]
        public async Task ShowCommand(CommandContext context, int id)
        {
            var quote = await Manager.Get(context.Guild, id);
            if (quote != null)
                await context.RespondAsync("Quote #" + id + "\n" + Stringify(context, quote));
            else
                await context.RespondAsync("Quote #" + id + " does not exist");
        }

        private string Stringify(CommandContext context, Quote quote)
        {
            // TODO QuoteData (ulong; ulong; ...) vs QuoteInContext?
            var channel = context.Guild.Channels.GetValueOrDefault(quote.Channel);
            var author = context.Guild.Members.GetValueOrDefault(quote.Author);
            var quoteeId = quote.Quotee;
            var quotee = quoteeId.HasValue ? context.Guild.Members.GetValueOrDefault(quoteeId.Value) : null;
            var text = MessageUtil.WrapInQuoteBlock(quote.Text);
            return text;
        }
    }
}
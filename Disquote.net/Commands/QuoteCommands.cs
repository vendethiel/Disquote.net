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
    // Created internally by D#+
    // ReSharper disable once ClassNeverInstantiated.Global
    public class QuoteCommands : BaseCommandModule
    {
        public Manager.QuoteManager Manager { private get; set; } = null!;

        [Command("add")]
        public async Task AddMultiCommand(CommandContext context, [RemainingText] string text)
        {
            var id = await Manager.AddMulti(context.Guild, context.Channel, context.Member, text);
            await context.RespondAsync("Added quote #" + id);
        }

        [Command("add")]
        public async Task AddTargetedCommand(CommandContext context, DiscordUser quotee, [RemainingText] string quote)
        {
            var id = await Manager.AddTargeted(context.Guild, context.Channel, context.Member, quotee, quote);
            await context.RespondAsync("Added quote #" + id);
        }

        [Command("search")]
        public async Task SearchCommand(CommandContext context, [RemainingText] string search)
        {
            var indices = await Manager.Search(context.Guild, search);
            switch (indices.Count)
            {
                case 0:
                    await context.RespondAsync("No quote found");
                    break;

                case 1:
                    var quote = indices[0];
                    await context.RespondAsync("Found quote #" + quote.Id + "\n" + Stringify(context, quote));
                    break;

                default:
                    var found = String.Join(", ", indices.Select(q => "#" + q.Id));
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

        private static string Stringify(CommandContext context, Quote quote)
        {
            var channel = context.Guild.Channels.GetValueOrDefault(quote.ChannelId);
            var author = context.Guild.Members.GetValueOrDefault(quote.AuthorId);
            var quoteeId = quote.QuoteeId;
            var quotee = quoteeId.HasValue ? context.Guild.Members.GetValueOrDefault(quoteeId.Value) : null;
            var text = MessageUtil.WrapInQuoteBlock(quote.Text);
            return text;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Disquote.net.Data;
using DSharpPlus.Entities;

namespace Disquote.net.Manager
{
    public class QuoteManager
    {
        // TODO Dictionary<Guild Snowflake, List<Quote>>
        private readonly Dictionary<ulong, List<Quote>> _quotes = new();

        private List<Quote> QuotesFor(DiscordGuild guild)
        {
            var id = guild.Id;
            if (!_quotes.ContainsKey(id))
                _quotes.Add(id, new List<Quote>());
            return _quotes[id];            
        }
        
        public async Task<int> AddMulti(DiscordGuild guild, DiscordChannel channel, DiscordUser user, string text)
        {
            // ???
            // var quote = new Quote(text, channel.Id, user.Id);
            var guildQuotes = QuotesFor(guild);
            return guildQuotes.Count;
        }

        public async Task<int> AddTargeted(DiscordGuild guild, DiscordChannel channel, DiscordUser user, DiscordUser quotee, string text)
        {
            var quoteAuthor = QuoteUser.FromDiscordUser(user);
            var quoteQuotee = QuoteUser.FromDiscordUser(quotee);
            var quote = new Quote(text, channel.Id, quoteAuthor, quoteQuotee, false);
            QuotesFor(guild).Add(quote);
            return _quotes.Count;
        }

        public async Task<Quote?> Get(DiscordGuild guild, int id)
        {
            var guildQuotes = QuotesFor(guild);
            if (id > guildQuotes.Count)
                return null;
            var quote = guildQuotes[id - 1];
            return quote.Deleted ? null : quote;
        }

        public async Task<List<int>> Search(DiscordGuild guild, string query)
        {
            return QuotesFor(guild)
                .Select((quote, idx) => (quote, idx))
                .Where(t => !t.quote.Deleted)
                .Where(t => t.quote.Text.Contains(query))
                .Select(i => i.idx)
                .ToList();
        }

    }
}
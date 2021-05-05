using System;
using System.Collections.Generic;
using System.Linq;
using Disquote.net.Data;
using DSharpPlus.Entities;

namespace Disquote.net.Manager
{
    public class QuoteManager
    {
        // TODO Dictionary<Guild Snowflake, List<Quote>>
        private Dictionary<ulong, List<Quote>> quotes = new();

        public List<Quote> QuotesFor(DiscordGuild guild)
        {
            var id = guild.Id;
            if (!quotes.ContainsKey(id))
                quotes.Add(id, new List<Quote>());
            return quotes[id];            
        }
        
        public int AddMulti(DiscordGuild guild, DiscordChannel channel, DiscordUser user, string text)
        {
            var quote = new Quote(text, channel.Id, user.Id);
            var guildQuotes = QuotesFor(guild);
            return guildQuotes.Count;
        }

        public int AddTargeted(DiscordGuild guild, DiscordChannel channel, DiscordUser user, DiscordUser quotee, string text)
        {
            var quote = new Quote(text, channel.Id, user.Id, quotee.Id);
            QuotesFor(guild).Add(quote);
            return quotes.Count;
        }

        public Quote? Get(DiscordGuild guild, int id)
        {
            var guildQuotes = QuotesFor(guild);
            if (id > guildQuotes.Count)
                return null;
            var quote = guildQuotes[id - 1];
            return quote.Deleted ? null : quote;
        }

        public List<int> Search(DiscordGuild guild, String query)
        {
            return QuotesFor(guild)
                .Select((quote, idx) => (idx, quote))
                .Where(t => !t.quote.Deleted)
                .Where(t => t.quote.Text.Contains(query))
                .Select(i => i.idx)
                .ToList();
        }

    }
}
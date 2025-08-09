using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Disquote.net.Data;
using DSharpPlus.Entities;
using Microsoft.EntityFrameworkCore;

namespace Disquote.net.Manager
{
    public class QuoteManager
    {
        private readonly Context _dbContext;

        public QuoteManager(Context dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> AddMulti(DiscordGuild guild, DiscordChannel channel, DiscordUser user, string text)
        {
            var quote = new Quote
            {
                GuildId = guild.Id,
                Text = text,
                ChannelId = channel.Id,
                ChannelName = channel.Name,
                AuthorId = user.Id,
                AuthorName = user.Username,
            };

            return await InsertQuote(guild, quote);
        }

        public async Task<int> AddTargeted(DiscordGuild guild, DiscordChannel channel, DiscordUser user,
            DiscordUser quotee, string text)
        {
            var quote = new Quote
            {
                GuildId = guild.Id,
                Text = text,
                ChannelId = channel.Id,
                ChannelName = channel.Name,
                AuthorId = user.Id,
                AuthorName = user.Username,
                QuoteeId = quotee.Id,
                QuoteeName = quotee.Username
            };
            return await InsertQuote(guild, quote);
        }

        private async Task<int> InsertQuote(DiscordGuild guild, Quote quote)
        {
            // Data races may happen, try several times, TODO better than this shit
            for (var i = 0; i < 50; ++i)
            {
                try
                {
                    var max = await HighestQuote(guild);
                    quote.Id = max + 1;
                    _dbContext.Add(quote);
                    await _dbContext.SaveChangesAsync();
                    return quote.Id;
                }
                catch (Exception e) when (e is DbUpdateConcurrencyException or DbUpdateException)
                {
                }
            }

            throw new InvalidOperationException("TODO better DB insertion");
        }

        private async Task<int> HighestQuote(DiscordGuild guild)
        {
            return await _dbContext.Quotes
                .Where(q => q.GuildId == guild.Id)
                .Select(q => q.Id)
                .DefaultIfEmpty(0)
                .MaxAsync();
        }

        public async Task<Quote?> Get(DiscordGuild guild, int id)
        {
            Console.WriteLine("zz");
            return await _dbContext.Quotes
                .Where(q => !q.Deleted)
                .Where(q => q.GuildId == guild.Id)
                .Where(q => q.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<List<Quote>> Search(DiscordGuild guild, string query)
        {
            return await _dbContext.Quotes
                .Where(q => !q.Deleted)
                .Where(q => q.GuildId == guild.Id)
                .Where(q => q.Text.Contains(query))
                .ToListAsync();
        }

        public async Task<Quote?> Random(DiscordGuild guild)
        {
            var quotes = await _dbContext.Quotes.Where(q => !q.Deleted).Where(q => q.GuildId == guild.Id).ToListAsync();
            if (quotes.Count == 0)
                return null;
            return quotes[new Random().Next(0, quotes.Count)];
        }
    }
}
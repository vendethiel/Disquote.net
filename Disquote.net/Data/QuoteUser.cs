using DSharpPlus.Entities;

namespace Disquote.net.Data
{
    public record QuoteUser(ulong Id, string Username)
    {
        public static QuoteUser FromDiscordUser(DiscordUser user)
        {
            return new QuoteUser(user.Id, user.Username);
        }
    }
}
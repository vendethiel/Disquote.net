using DSharpPlus.Entities;

namespace Disquote.net.Data
{
    public class Quote
    {
        public string Text { get; }
        public DiscordUser Author { get; }
        public DiscordUser? Quotee { get;  }
    }
}
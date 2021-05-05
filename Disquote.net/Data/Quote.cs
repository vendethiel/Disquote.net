using DSharpPlus.Entities;

namespace Disquote.net.Data
{
    public class Quote
    {
        public Quote(string text, ulong channel, ulong author, ulong quotee)
        {
            Text = text;
            Channel = channel;
            Author = author;
            Quotee = quotee;
        }

        public Quote(string text, ulong channel, ulong author)
        {
            Text = text;
            Channel = channel;
            Author = author;
        }

        public string Text { get; }
        public ulong Channel { get; }
        public ulong Author { get; }
        public ulong? Quotee { get; } = null;
        public bool Deleted { get; } = false;
    }
}
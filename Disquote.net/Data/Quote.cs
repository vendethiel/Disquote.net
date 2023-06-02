namespace Disquote.net.Data
{
    public class Quote
    {
        public Quote(string text, ulong channel, QuoteUser author, QuoteUser? quotee, bool deleted)
        {
            Text = text;
            Channel = channel;
            Author = author;
            Quotee = quotee;
            Deleted = deleted;
        }

        public string Text { get; }
        public ulong Channel { get; }
        public QuoteUser Author { get; }
        public QuoteUser? Quotee { get; }
        public bool Deleted { get; }
    }
}
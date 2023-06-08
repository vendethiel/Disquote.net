namespace Disquote.net.Data
{
    public class Quote
    {
        public int Id { get; set; }
        public ulong GuildId { get; set; }
        public string Text { get; set; }
        public ulong ChannelId { get; set; }
        public string ChannelName { get; set; }
        public ulong AuthorId { get; set; }
        public string AuthorName { get; set; }
        public ulong? QuoteeId { get; set; }
        public string? QuoteeName { get; set; }
        public bool Deleted { get; set; }
    }
}
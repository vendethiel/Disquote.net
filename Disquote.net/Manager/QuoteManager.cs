using System;
using System.Collections.Generic;
using System.Linq;

namespace Disquote.net.Manager
{
    public class QuoteManager
    {
        // TODO Dictionary<Guild Snowflake, List<Quote>>
        private List<string> quotes = new();
        
        public int AddMulti(string quote)
        {
            quotes.Add(quote);
            return quotes.Count;
        }

        public int AddTargeted(string user, string quote)
        {
            quotes.Add(user + ": " + quote);
            return quotes.Count;
        }

        public string? Get(int id)
        {
            return id <= quotes.Count ? quotes[id - 1] : null;
        }

        public List<int> Search(String query)
        {
            return quotes
                .Select((text, idx) => (idx, text))
                .Where(t => t.text.Contains(query))
                .Select(i => i.idx)
                .ToList();
        }
    }
}
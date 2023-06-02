using System;
using System.Linq;

namespace Disquote.net
{
    public static class MessageUtil
    {
        public static string WrapInQuoteBlock(string s)
        {
            return string.Join('\n', s.Split("\n").Select(l => "> " + l));
        }
    }
}
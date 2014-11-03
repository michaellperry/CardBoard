using System;
using System.Text.RegularExpressions;

namespace CardBoard.Common
{
    public class Utilities
    {
        private static readonly Regex Punctuation = new Regex(@"[{}\-]");

        public static string GenerateRandomId()
        {
            return Punctuation.Replace(Guid.NewGuid().ToString(), String.Empty).ToLower();
        }
    }
}

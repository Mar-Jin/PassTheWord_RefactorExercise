using PassTheWord.Requirements;
using RND = System.Security.Cryptography.RandomNumberGenerator;
using System.Text;
using System.Linq;

namespace PassTheWord.Strategies;

public class RandomCharacterStrategy(PasswordOptions options): BasePasswordStrategy(options)
{
    protected override int FillBuffer(Span<char> buf)
    {
        int len = 0;
        string alphabet = BuildAlphabet();
        
        if (string.IsNullOrEmpty(alphabet))
        {
            throw new InvalidOperationException("The alphabet is empty. Check your password options.");
        }

        if (Requirements.NeedsUpper)
        {
            string uppers = string.Concat(Options.Alphabets.Select(a => a.GetUppercase(Options.ExcludeSimilar)));
            if (!string.IsNullOrEmpty(uppers)) buf[len++] = RND.GetString(uppers, 1)[0];
        }

        if (Requirements.NeedsDigit)
        {
            string digits = string.Concat(Options.Alphabets.Select(a => a.GetDigits(Options.ExcludeSimilar)));
            if (!string.IsNullOrEmpty(digits)) buf[len++] = RND.GetString(digits, 1)[0];
        }

        if (Requirements.NeedsSymbol)
        {
            string symbols = string.Concat(Options.Alphabets.Select(a => a.GetSymbols(Options.ExcludeSimilar)));
            if (!string.IsNullOrEmpty(symbols)) buf[len++] = RND.GetString(symbols, 1)[0];
        }
        
        int charsToAdd = Options.MinLength - len;

        if (charsToAdd > 0)
        {
            RND.GetItems(alphabet, buf.Slice(len, charsToAdd));
            len += charsToAdd;
        }
        
        RND.Shuffle(buf.Slice(0, len));

        return len;
    }
}
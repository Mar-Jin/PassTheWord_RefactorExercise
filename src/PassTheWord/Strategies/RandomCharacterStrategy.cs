using PassTheWord.Requirements;
using RND = System.Security.Cryptography.RandomNumberGenerator;

namespace PassTheWord.Strategies;

public class RandomCharacterStrategy(PasswordOptions options): BasePasswordStrategy(options)
{
    protected override int FillBuffer(Span<char> buf)
    {
        int len = 0;
        string alphabet = BuildAlphabet();
        Console.WriteLine(alphabet);

        /*Console.WriteLine("Log - String Builder");
        StringBuilder sb = new();
        for (int i = 0; i < minlength; i++) {
            string s = RND.GetString(alphabet, 1);
            if (Char.IsUpper(s[0])) _reqUpper = false;
            if (Char.IsDigit(s[0])) _reqDigit = false;
            if (Char.IsSymbol(s[0])) _reqSymbol = false;
            Console.WriteLine(s);
            sb.Append(s);
        }
        Console.WriteLine(sb);

        if (_reqUpper)
            sb.Insert(rnd.Next(sb.Length), RND.GetString("ABCDEFGHIJKLMNOPQRSTUVWXYZ", 1));
        if (_reqDigit)
            sb.Insert(rnd.Next(sb.Length), RND.GetString("0123456789", 1));
        if (_reqSymbol)
            sb.Insert(rnd.Next(sb.Length), RND.GetString("!@#$%^&*()_+-=,./?~", 1));

        if (ContainsParadoxicalRequest(_reqUpper, _reqDigit, _reqSymbol, _uppercase, _digits, _symbols)) return -1;

        sb.Clear();*/
        
        if (Options.ExcludeSimilar)
        {
            if (Requirements.NeedsUpper) buf[len++] = RND.GetString("ABCDEFGHJKLMNPQRSTUVWXYZ", 1)[0];
            if (Requirements.NeedsDigit) buf[len++] = RND.GetString("23456789", 1)[0];
        }
        else
        {
            if (Requirements.NeedsUpper) buf[len++] = RND.GetString("ABCDEFGHJKLMNPQRSTUVWXYZIO", 1)[0];
            if (Requirements.NeedsDigit) buf[len++] = RND.GetString("2345678901", 1)[0];
        }

        if (Requirements.NeedsSymbol) buf[len++] = RND.GetString("!@#$%^&*()_+-=,./?~", 1)[0];
        
        int charsToAdd = Options.MinLength - len;

        if (charsToAdd > 0)
        {
            RND.GetItems(alphabet, buf.Slice(len, Options.MinLength - len));
            len += charsToAdd;
        }
        
        RND.Shuffle(buf.Slice(0, Options.MinLength));

        return len;
    }
}
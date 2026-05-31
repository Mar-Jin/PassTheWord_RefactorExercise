using RND = System.Security.Cryptography.RandomNumberGenerator;

namespace PassTheWord.Strategies;

public class InteractivePassphraseStrategy(PasswordOptions options): BasePasswordStrategy(options)
{
    protected override int FillBuffer(Span<char> buf)
    {
        int len = 0;

        string alphabet = BuildAlphabet();

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
        string? s1 = null;
        do
        {
            //FIXME We need to be more flexible regarding input options (e.g. GUI)
            Console.Write("Enter passphrase: ");
            s1 = Console.ReadLine();
            //TODO assert Unicode MLP, reqX
        } while (s1.Length < Requirements.MinLength || s1.Length > buf.Length);

        len = s1.Length;
        s1.TryCopyTo(buf);

        for (int i = 0; i < len; i++)
        {
            char currentCharacter = buf[i];
            if (!options.Replacements.ContainsKey(currentCharacter)) continue;
            bool shouldReplace = RND.GetInt32(0, 100) < 50;

            if (shouldReplace)
            {
                buf[i] = options.Replacements[currentCharacter];
            }
        }

        return len;
    }
}
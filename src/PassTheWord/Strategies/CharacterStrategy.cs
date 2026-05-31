using RND = System.Security.Cryptography.RandomNumberGenerator;

namespace PassTheWord.Strategies;

public class CharacterStrategy: IPasswordStrategy
{
    private bool _interactive;
    private bool _excludeSimilar;
    private bool _uppercase;
    private bool _lowercase;
    private bool _digits;
    private bool _symbols;
    private bool _reqUpper;
    private bool _reqDigit;
    private bool _reqSymbol;
    
    public CharacterStrategy(bool interactive, bool excludeSimilar, bool uppercase, bool lowercase, bool digits, bool symbols, bool reqUpper, bool reqDigit, bool reqSymbol)
    {
        Console.WriteLine("Succesfully created CharacterStrategy");

        _interactive = interactive;
        _excludeSimilar = excludeSimilar;
        _uppercase = uppercase;
        _lowercase = lowercase;
        _digits = digits;
        _symbols = symbols;
        _reqUpper = reqUpper;
        _reqDigit = reqDigit;
        _reqSymbol = reqSymbol;
    }
    
    public (int len, char[] buf) Generate(Span<char> buf, int minLength, int maxLength, Dictionary<char, char> replacements)
    {
        int len = 0;
        
        string alphabet = "";

        if (_uppercase) alphabet += _excludeSimilar ? "ABCDEFGHJKLMNPQRSTUVWXYZ" : "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        if (_lowercase) alphabet += _excludeSimilar ? "abcdefghijkmnopqrstuvwxyz" : "abcdefghijklmnopqrstuvwxyz";
        if (_digits) alphabet += !_excludeSimilar ? "0123456789" : "23456789";
        if (_symbols) alphabet += "!@#$%^&*()_+-=,./?~";

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

        if (!_interactive)
        {
            if (_excludeSimilar)
            {
                if (_reqUpper) buf[len++] = RND.GetString("ABCDEFGHJKLMNPQRSTUVWXYZ", 1)[0];
                if (_reqDigit) buf[len++] = RND.GetString("23456789", 1)[0];
            }
            else
            {
                if (_reqUpper) buf[len++] = RND.GetString("ABCDEFGHJKLMNPQRSTUVWXYZIO", 1)[0];
                if (_reqDigit) buf[len++] = RND.GetString("2345678901", 1)[0];
            }

            if (_reqSymbol) buf[len++] = RND.GetString("!@#$%^&*()_+-=,./?~", 1)[0];

            foreach (char c in buf)
            {
                Console.WriteLine(c);
            }

            RND.GetItems(alphabet, buf.Slice(len, minLength - len));
            RND.Shuffle(buf.Slice(0, minLength));
        }
        else
        {
            string? s1 = null;
            do
            {
                //FIXME We need to be more flexible regarding input options (e.g. GUI)
                Console.Write("Enter passphrase: ");
                s1 = Console.ReadLine();
                //TODO assert Unicode MLP, reqX
            } while (s1.Length < minLength || s1.Length > buf.Length);

            len = s1.Length;
            s1.TryCopyTo(buf);
        }

        for (int i = 0; i < len; i++)
        {
            char currentCharacter = buf[i];
            if (!replacements.ContainsKey(currentCharacter)) continue;
            bool shouldReplace = RND.GetInt32(0, 100) < 50;

            if (shouldReplace)
            {
                buf[i] = replacements[currentCharacter];
            }
        }

        return (len, buf.ToArray());
    }
}
using System.Text;
using RND = System.Security.Cryptography.RandomNumberGenerator;
using PassTheWord.Strategies;
using Microsoft.Extensions.Logging;

namespace PassTheWord;

public class PasswordFacade
{
    public PasswordFacade()
    {
        
    }

    public (int, char[]) GeneratePassword(
        Dictionary<char, char> replacements,
        bool excludeSimilar = false,
        List<string>? dictionary = null,
        Span<char> buf = default, // Clean this!
        bool interactive = false,
        int minlength = 8,
        int maxlength = 20,
        bool uppercase = false,
        bool lowercase = false,
        bool digits = false,
        bool symbols = false,
        bool reqUpper = false,
        bool reqDigit = false,
        bool reqSymbol = false)
    {
        Random rnd = new();
        int len = 0;

        // check buf.Length >= minlength
        if (!(buf.Length >= minlength))
            throw new ArgumentException("The length of the buffer is less than the minimum length requested");

        IPasswordStrategy strategy;
        
        if (dictionary != null && dictionary.Count > 0)
        {
            strategy = new WordListStrategy(dictionary);

            return strategy.Generate(buf, minlength, maxlength, replacements);
        }
        
        else
        {
            string alphabet = "";

            if (uppercase) alphabet += excludeSimilar ? "ABCDEFGHJKLMNPQRSTUVWXYZ" : "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            if (lowercase) alphabet += excludeSimilar ? "abcdefghijkmnopqrstuvwxyz" : "abcdefghijklmnopqrstuvwxyz";
            if (digits) alphabet += !excludeSimilar ? "0123456789" : "23456789";
            if (symbols) alphabet += "!@#$%^&*()_+-=,./?~";

            /*Console.WriteLine("Log - String Builder");
            StringBuilder sb = new();
            for (int i = 0; i < minlength; i++) {
                string s = RND.GetString(alphabet, 1);
                if (Char.IsUpper(s[0])) reqUpper = false;
                if (Char.IsDigit(s[0])) reqDigit = false;
                if (Char.IsSymbol(s[0])) reqSymbol = false;
                Console.WriteLine(s);
                sb.Append(s);
            }
            Console.WriteLine(sb);

            if (reqUpper)
                sb.Insert(rnd.Next(sb.Length), RND.GetString("ABCDEFGHIJKLMNOPQRSTUVWXYZ", 1));
            if (reqDigit)
                sb.Insert(rnd.Next(sb.Length), RND.GetString("0123456789", 1));
            if (reqSymbol)
                sb.Insert(rnd.Next(sb.Length), RND.GetString("!@#$%^&*()_+-=,./?~", 1));

            if (ContainsParadoxicalRequest(reqUpper, reqDigit, reqSymbol, uppercase, digits, symbols)) return -1;

            sb.Clear();*/

            if (!interactive)
            {
                if (excludeSimilar)
                {
                    if (reqUpper) buf[len++] = RND.GetString("ABCDEFGHJKLMNPQRSTUVWXYZ", 1)[0];
                    if (reqDigit) buf[len++] = RND.GetString("23456789", 1)[0];
                }
                else
                {
                    if (reqUpper) buf[len++] = RND.GetString("ABCDEFGHJKLMNPQRSTUVWXYZIO", 1)[0];
                    if (reqDigit) buf[len++] = RND.GetString("2345678901", 1)[0];
                }

                if (reqSymbol) buf[len++] = RND.GetString("!@#$%^&*()_+-=,./?~", 1)[0];

                foreach (char c in buf)
                {
                    Console.WriteLine(c);
                }

                RND.GetItems(alphabet, buf.Slice(len, minlength - len));
                RND.Shuffle(buf.Slice(0, minlength));
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
                } while (s1.Length < minlength || s1.Length > buf.Length);

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
        }

        return (len, buf.ToArray());
    }

    private static void ValidateDictionary(List<string> dictionary)
    {
        foreach (string s in dictionary)
        {
            foreach (char c in s)
            {
                if (Char.IsSurrogate(c))
                {
                    throw new ArgumentException("The dictionary holds one or more words that contain surrogate characters.");
                }
            }
        }
    }
}
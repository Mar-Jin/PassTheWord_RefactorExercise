using System.Text;
using RND = System.Security.Cryptography.RandomNumberGenerator;

namespace PassTheWord {
    internal class Program {

        static int GeneratePassword(
                bool interactive,
                Span<char> buf,  // Clean this!
                int minlength,
                int maxlength,
                bool uppercase,
                bool lowercase,
                bool digits,
                bool symbols,
                bool reqUpper,
                bool reqDigit,
                bool reqSymbol,
                bool excludeSimilar,
                Dictionary<char,char> replacements,
                List<string>? dictionary) {

            Random rnd = new();
            int len = 0;

            // check buf.Length >= minlength
            // check nothing in replacements is surrogate

            // Check Words Dictionary Given
            if (dictionary != null)
            {
                Console.WriteLine("Log - Checking Dictionary for Surrogates:");
                if (ContainsSurrogate(dictionary)) return -1;

                while (len < minlength) {
                    string w = dictionary[RND.GetInt32(dictionary.Count)];
                    Console.WriteLine(w);
                    
                    if (!w.TryCopyTo(buf.Slice(len)))
                    {
                        return -1;
                    }
                    len += w.Length;
                }

                for (int i = 0; i < len; i++) {
                    if (replacements.ContainsKey(buf[i])) {
                        if (RND.GetInt32(2147483647) > 0x3FFFFFFF) {
                            buf[i] = replacements[buf[i]];
                        }
                    }
                }

                return len;
            }

            string alphabet = "";

            if (uppercase) alphabet += excludeSimilar ? "ABCDEFGHJKLMNPQRSTUVWXYZ" : "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            if (lowercase) alphabet += excludeSimilar ? "abcdefghijkmnopqrstuvwxyz" : "abcdefghijklmnopqrstuvwxyz";
            if (digits)    alphabet += !excludeSimilar ? "0123456789" : "23456789";
            if (symbols)   alphabet += "!@#$%^&*()_+-=,./?~";

            StringBuilder sb = new();
            /*for (int i = 0; i < minlength; i++) {
                string s = RND.GetString(alphabet, 1);
                if (Char.IsUpper(s[0])) reqUpper = false;
                if (Char.IsDigit(s[0])) reqDigit = false;
                if (Char.IsSymbol(s[0])) reqSymbol = false;
                sb.Append(s);
            }

            if (reqUpper)
                sb.Insert(rnd.Next(sb.Length), RND.GetString("ABCDEFGHIJKLMNOPQRSTUVWXYZ", 1));
            if (reqDigit)
                sb.Insert(rnd.Next(sb.Length), RND.GetString("0123456789", 1));
            if (reqSymbol)
                sb.Insert(rnd.Next(sb.Length), RND.GetString("!@#$%^&*()_+-=,./?~", 1));
            */

            /*check if (reqUpper && !uppercase  ||  reqDigit && !digits  ||  reqSymbol && !symbols)
                return -1;*/

            sb.Clear();
            
            if (!interactive) {
                if (excludeSimilar) {
                    if (reqUpper) buf[len++] = RND.GetString("ABCDEFGHJKLMNPQRSTUVWXYZ", 1)[0];
                    if (reqDigit) buf[len++] = RND.GetString("23456789", 1)[0];
                    if (reqSymbol) buf[len++] = RND.GetString("!@#$%^&*()_+-=,./?~", 1)[0];
                } else {
                    if (reqUpper) buf[len++] = RND.GetString("ABCDEFGHJKLMNPQRSTUVWXYZIO", 1)[0];
                    if (reqDigit) buf[len++] = RND.GetString("2345678901", 1)[0];
                    if (reqSymbol) buf[len++] = RND.GetString("!@#$%^&*()_+-=,./?~", 1)[0];
                }
                RND.GetItems(alphabet, buf.Slice(len, minlength - len));
                RND.Shuffle(buf.Slice(0, minlength));
            }
            else
            {
                string? s1 = null;
                do {
                    //FIXME We need to be more flexible regarding input options (e.g. GUI)
                    Console.Write("Enter passphrase: ");
                    s1 = Console.ReadLine();
                    //TODO assert unicode MLP, reqX
                } while (s1.Length < minlength  ||  s1.Length > buf.Length);
                len = s1.Length;
                s1.TryCopyTo(buf);
                
            }

            for (int i = 0; i < len; i++)
            {
                char currentCharacter = buf[i];
                if (replacements.ContainsKey(currentCharacter))
                {
                    bool shouldReplace = RND.GetInt32(0, 100) < 50;
                    
                    if (shouldReplace) {
                        buf[i] = replacements[currentCharacter];
                    }
                }
            }

            return len;
        }

        static void Main(string[] args) {
            List<string> words = new() { "hallo", "kat", "hond", "paard", "wei", "accu", "batterij", "doei"};
            List<string> longWords = new() { "kindercarnavalsoptochtvoorbereidingswerkzaamheden", "aansprakelijkheidswaardevaststellingsveranderingen",
                "Hottentottensoldatententententoonstellingsbouwterrein",
                "elektriciteitsproductiemaatschappijbuitenlandbelangen", 
                "geneesmiddelenvergoedingssysteemorganisatiestructuur" };
            List<string> wordsWithSurrogate = new() { "𠜎" };
            Dictionary<char, char> subs = new() { { 'o', '0' }, { 'i', '1' }, { 's', '$' } };
            char[] buf = new char[100];
            int len = 0;

            /*len = GeneratePassword(true, buf, 8, 20, false, false, false, false, false, false, false, false, subs, null);
            Console.WriteLine($"{len}: {new string(buf[0..len])}");*/
            len = GeneratePassword(false, buf, 8, 20, false, false, false, false, false, false, false, false, subs, words);
            Console.WriteLine($"{len}: {new string(buf[0..len])}");
            /*len = GeneratePassword(false, buf, 8, 20, true, true, false, true, false, false, false, true, new Dictionary<char, char>(), null);
            Console.WriteLine($"{len}: {new string(buf[0..len])}");*/
        }

        private static bool ContainsSurrogate(List<string> dictionary)
        {
            foreach (string s in dictionary) {
                foreach (char c in s)
                {
                    if (Char.IsSurrogate(c))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}

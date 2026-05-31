using RND = System.Security.Cryptography.RandomNumberGenerator;

namespace PassTheWord.Strategies;

public class WordListStrategy: IPasswordStrategy
{
    private List<string> _dictionary;
    
    public WordListStrategy(List<string>? dictionary)
    {
        _dictionary = dictionary;
    }
    
    public (int len, char[] buf) Generate(Span<char> buf, int minLength, int maxLength, Dictionary<char, char> replacements)
    {
        int len = 0;

        Console.WriteLine("Log - Checking Dictionary for Surrogates:");
        ValidateDictionary(_dictionary);

        Console.WriteLine("Log - Adding words to the result");
        while (len < minLength)
        {
            string w = _dictionary[RND.GetInt32(_dictionary.Count)];
            Console.WriteLine(w);

            if (!w.TryCopyTo(buf.Slice(len)))
            {
                throw new ArgumentException("Cannot copy word to buffer");
            }

            len += w.Length;
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
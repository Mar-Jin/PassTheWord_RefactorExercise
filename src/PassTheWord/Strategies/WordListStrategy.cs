using RND = System.Security.Cryptography.RandomNumberGenerator;

namespace PassTheWord.Strategies;

public class WordListStrategy: IPasswordStrategy
{
    private readonly PasswordOptions _options;
    private List<string> _dictionary;
    
    public WordListStrategy(PasswordOptions options)
    {
        _options = options;
        _dictionary = options.Dictionary;
    }
    
    public (int len, char[] buf) Generate()
    {
        Span<char> buf = new char[_options.MaxLength];
        int len = 0;

        Console.WriteLine("Log - Checking Dictionary for Surrogates:");
        ValidateDictionary();

        Console.WriteLine("Log - Adding words to the result");
        while (len < _options.MinLength)
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
            if (!_options.Replacements.ContainsKey(currentCharacter)) continue;
            bool shouldReplace = RND.GetInt32(0, 100) < 50;

            if (shouldReplace)
            {
                buf[i] = _options.Replacements[currentCharacter];
            }
        }

        return (len, buf.ToArray());
    }
    
    private void ValidateDictionary()
    {
        foreach (string s in _dictionary)
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
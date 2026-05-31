using RND = System.Security.Cryptography.RandomNumberGenerator;

namespace PassTheWord.Strategies;

public class WordListStrategy(PasswordOptions options): BasePasswordStrategy(options)
{
    protected override int FillBuffer(Span<char> buf)
    {
        Console.WriteLine("Log - Checking Dictionary for Surrogates:");
        ValidateDictionary();

        Console.WriteLine("Log - Adding words to the result");
        int len = 0;
        
        while (len < Requirements.MinLength)
        {
            string w = options.Dictionary[RND.GetInt32(options.Dictionary.Count)];
            Console.WriteLine(w);

            if (!w.TryCopyTo(buf.Slice(len)))
            {
                throw new ArgumentException("Cannot copy word to buffer");
            }
            len += w.Length;
        }

        return len;
    }
    
    private void ValidateDictionary()
    {
        foreach (string s in options.Dictionary)
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
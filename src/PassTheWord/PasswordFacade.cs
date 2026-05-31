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
        }
        else
        {
            strategy = new CharacterStrategy(interactive, excludeSimilar, uppercase, lowercase, digits, symbols, reqUpper, reqDigit, reqSymbol);
        }

        return strategy.Generate(buf, minlength, maxlength, replacements);
    }
}
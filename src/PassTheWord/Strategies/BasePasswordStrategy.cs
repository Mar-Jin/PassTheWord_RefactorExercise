using System.Text;
using System.Security.Cryptography;
using System.Linq;
using PassTheWord.Requirements;
using RND = System.Security.Cryptography.RandomNumberGenerator;

namespace PassTheWord.Strategies;

public abstract class BasePasswordStrategy : IPasswordStrategy
{
    protected readonly PasswordOptions Options;
    protected RequirementCharacterVisitor Requirements { get; private set; }

    protected BasePasswordStrategy(PasswordOptions options)
    {
        Options = options;
        Requirements = new RequirementCharacterVisitor();
        Options.Requirements.Accept(Requirements);
    }

    public (int len, char[] buf) Generate()
    {
        char[] buffer = new char[Options.MaxLength];
        int len;
        string password;

        do {
            len = FillBuffer(buffer); 
            ApplyReplacements(buffer, len);
            password = new string(buffer, 0, len);
        } while (!Options.Requirements.IsSatisfiedBy(password) || !VerifyExternal(password)); 

        return (len, buffer[..len]);
    }

    protected abstract int FillBuffer(Span<char> buf);

    private void ApplyReplacements(char[] buf, int len)
    {
        for (int i = 0; i < len; i++)
        {
            char currentCharacter = buf[i];
            if (!Options.Replacements.TryGetValue(currentCharacter, out char replacement)) continue;
            
            if (RND.GetInt32(0, 100) < 50)
            {
                buf[i] = replacement;
            }
        }
    }

    private bool VerifyExternal(string password)
    {
        if (Options.Verifiers.Count == 0) return true;

        byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
        var groups = Options.Verifiers.GroupBy(v => v.HashAlgorithmName);

        foreach (var group in groups)
        {
            using var algorithm = IncrementalHash.CreateHash(new HashAlgorithmName(group.Key));
            algorithm.AppendData(passwordBytes);
            byte[] hash = algorithm.GetHashAndReset();

            if (group.Any(verifier => !verifier.IsSafe(hash)))
            {
                return false;
            }
        }

        return true;
    }
    
    protected string BuildAlphabet()
    {
        StringBuilder alphabet = new();
        foreach (var a in Options.Alphabets)
        {
            if (Options.Uppercase) alphabet.Append(a.GetUppercase(Options.ExcludeSimilar));
            if (Options.Lowercase) alphabet.Append(a.GetLowercase(Options.ExcludeSimilar));
            if (Options.Digits)    alphabet.Append(a.GetDigits(Options.ExcludeSimilar));
            if (Options.Symbols)   alphabet.Append(a.GetSymbols(Options.ExcludeSimilar));
        }
        return alphabet.ToString();
    }
}
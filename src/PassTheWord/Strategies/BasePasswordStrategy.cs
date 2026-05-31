using System.Text;
using System.Security.Cryptography;
using System.Linq;
using PassTheWord.Requirements;
using RND = System.Security.Cryptography.RandomNumberGenerator;

namespace PassTheWord.Strategies;

/// <summary>
/// Provides a base implementation for password generation strategies,
/// handling common tasks like requirement checking, replacements, and external verification.
/// </summary>
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

    /// <summary>
    /// Generates a password by repeatedly filling the buffer until all requirements 
    /// and external verifications are satisfied.
    /// </summary>
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

    /// <summary>
    /// Fills the buffer with initial characters according to the specific strategy.
    /// </summary>
    /// <param name="buf">The buffer to fill.</param>
    /// <returns>The number of characters written to the buffer.</returns>
    protected abstract int FillBuffer(Span<char> buf);

    /// <summary>
    /// Applies statistical replacements based on the configuration in <see cref="Options"/>.
    /// </summary>
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

    /// <summary>
    /// Validates the generated password against all configured external verifiers.
    /// </summary>
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
    
    /// <summary>
    /// Aggregates the character pools from all configured alphabets.
    /// </summary>
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
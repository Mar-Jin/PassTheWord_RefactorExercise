using PassTheWord.Requirements;
using PassTheWord.Alphabets;
using PassTheWord.Verification;

namespace PassTheWord;

/// <summary>
/// Represents the configuration settings for a password generation request.
/// Use <see cref="PasswordOptionsBuilder"/> to create instances of this class.
/// </summary>
public class PasswordOptions
{
    /// <summary>
    /// Gets the list of alphabets to use for generating characters.
    /// Default is Latin alphabet only.
    /// </summary>
    public List<IAlphabet> Alphabets { get; init; } = new() { new LatinAlphabet() };

    /// <summary>
    /// Gets the list of external verifiers to validate the generated password.
    /// </summary>
    public List<IPasswordVerifier> Verifiers { get; init; } = new();

    /// <summary>
    /// Gets a dictionary of character replacements (e.g., 'a' -> '@').
    /// These are applied statistically after initial generation.
    /// </summary>
    public Dictionary<char, char> Replacements { get; init; } = new();

    /// <summary>
    /// Gets the word list used by the <see cref="PassTheWord.Strategies.WordListStrategy"/>.
    /// </summary>
    public List<string>? Dictionary { get; init; }

    /// <summary>
    /// Gets the collection of requirements that the password must satisfy.
    /// </summary>
    public RequirementCollection Requirements { get; init; } = new();
    
    /// <summary>
    /// Gets the minimum requested length of the password.
    /// </summary>
    public int MinLength { get; init; } = 8;

    /// <summary>
    /// Gets the maximum requested length of the password.
    /// </summary>
    public int MaxLength { get; init; } = 20;

    /// <summary>
    /// Gets a value indicating whether similar characters (like 'I' and 'l') should be excluded.
    /// </summary>
    public bool ExcludeSimilar { get; init; }

    /// <summary>
    /// Gets a value indicating whether the password should be entered interactively by the user.
    /// </summary>
    public bool Interactive { get; init; }

    /// <summary>
    /// Gets a value indicating whether uppercase characters should be included in the pool.
    /// </summary>
    public bool Uppercase { get; init; }

    /// <summary>
    /// Gets a value indicating whether lowercase characters should be included in the pool.
    /// </summary>
    public bool Lowercase { get; init; }

    /// <summary>
    /// Gets a value indicating whether digits should be included in the pool.
    /// </summary>
    public bool Digits { get; init; }

    /// <summary>
    /// Gets a value indicating whether symbols should be included in the pool.
    /// </summary>
    public bool Symbols { get; init; }
}
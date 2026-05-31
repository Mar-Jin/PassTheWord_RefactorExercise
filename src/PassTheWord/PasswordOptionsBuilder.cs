using PassTheWord.Requirements;
using PassTheWord.Alphabets;
using PassTheWord.Verification;

namespace PassTheWord;

/// <summary>
/// A builder class for creating <see cref="PasswordOptions"/> instances using a fluent API.
/// </summary>
public class PasswordOptionsBuilder
{
    private List<IAlphabet> _alphabets = new();
    private List<IPasswordVerifier> _verifiers = new();
    private Dictionary<char, char> _replacements = new();
    private List<string>? _dictionary;
    
    private RequirementCollection _requirements = new();
    
    private int _minLength = 8;
    private int _maxLength = 20;
    private bool _excludeSimilar;
    private bool _interactive;
    private bool _uppercase;
    private bool _lowercase;
    private bool _digits;
    private bool _symbols;

    /// <summary>
    /// Sets the requested length range for the password and adds corresponding requirements.
    /// </summary>
    public PasswordOptionsBuilder WithLength(int min, int max)
    {
        _minLength = min;
        _maxLength = max;
        _requirements.Add(new MinLengthRequirement(min));
        _requirements.Add(new MaxLengthRequirement(max));
        return this;
    }

    /// <summary>
    /// Adds character replacements to be applied after generation.
    /// </summary>
    public PasswordOptionsBuilder WithReplacements(Dictionary<char, char> replacements)
    {
        _replacements = replacements;
        return this;
    }

    /// <summary>
    /// Configures the generator to use a word list strategy.
    /// </summary>
    public PasswordOptionsBuilder WithDictionary(List<string> dictionary)
    {
        _dictionary = dictionary;
        return this;
    }

    /// <summary>
    /// Configures the generator to request a passphrase from the user.
    /// </summary>
    public PasswordOptionsBuilder AsInteractive()
    {
        _interactive = true;
        return this;
    }

    /// <summary>
    /// Excludes characters that look similar (e.g., '1', 'l', 'I').
    /// </summary>
    public PasswordOptionsBuilder ExcludeSimilarCharacters()
    {
        _excludeSimilar = true;
        return this;
    }

    /// <summary> Adds uppercase letters to the character pool. </summary>
    public PasswordOptionsBuilder AllowUppercase() { _uppercase = true; return this; }
    /// <summary> Adds lowercase letters to the character pool. </summary>
    public PasswordOptionsBuilder AllowLowercase() { _lowercase = true; return this; }
    /// <summary> Adds digits to the character pool. </summary>
    public PasswordOptionsBuilder AllowDigits()    { _digits = true; return this; }
    /// <summary> Adds symbols to the character pool. </summary>
    public PasswordOptionsBuilder AllowSymbols()   { _symbols = true; return this; }

    /// <summary>
    /// Adds a requirement that the password must contain at least one uppercase letter.
    /// </summary>
    public PasswordOptionsBuilder RequireUppercase()
    {
        _requirements.Add(new UppercaseRequirement());
        return this;
    }

    /// <summary>
    /// Adds a requirement that the password must contain at least one digit.
    /// </summary>
    public PasswordOptionsBuilder RequireDigit()
    {
        _requirements.Add(new DigitRequirement());
        return this;
    }

    /// <summary>
    /// Adds a requirement that the password must contain at least one symbol.
    /// </summary>
    public PasswordOptionsBuilder RequireSymbol()
    {
        _requirements.Add(new SymbolRequirement());
        return this;
    }

    /// <summary>
    /// Adds a character set (alphabet) to the generation pool.
    /// </summary>
    public PasswordOptionsBuilder AddAlphabet(IAlphabet alphabet)
    {
        _alphabets.Add(alphabet);
        return this;
    }

    /// <summary>
    /// Adds an external verifier to validate the generated password.
    /// </summary>
    public PasswordOptionsBuilder AddVerifier(IPasswordVerifier verifier)
    {
        _verifiers.Add(verifier);
        return this;
    }

    /// <summary>
    /// Builds the final <see cref="PasswordOptions"/> instance.
    /// </summary>
    /// <returns>A configured PasswordOptions object.</returns>
    /// <exception cref="InvalidOperationException">Thrown if MinLength is greater than MaxLength.</exception>
    public PasswordOptions Build()
    {
        if (_minLength > _maxLength)
        {
            throw new InvalidOperationException("MinLength may not be more than MaxLength");
        }

        if (_alphabets.Count == 0)
        {
            _alphabets.Add(new LatinAlphabet());
        }
        
        return new PasswordOptions
        {
            Alphabets = _alphabets,
            Verifiers = _verifiers,
            Replacements = _replacements,
            Dictionary = _dictionary,
            Requirements = _requirements,
            MinLength = _minLength,
            MaxLength = _maxLength,
            ExcludeSimilar = _excludeSimilar,
            Interactive = _interactive,
            Uppercase = _uppercase,
            Lowercase = _lowercase,
            Digits = _digits,
            Symbols = _symbols,
        };
    }
}

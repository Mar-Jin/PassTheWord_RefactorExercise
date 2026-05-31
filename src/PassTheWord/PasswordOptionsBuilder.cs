using PassTheWord.Requirements;
using PassTheWord.Alphabets;
using PassTheWord.Verification;

namespace PassTheWord;

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

    public PasswordOptionsBuilder WithLength(int min, int max)
    {
        _minLength = min;
        _maxLength = max;
        _requirements.Add(new MinLengthRequirement(min));
        _requirements.Add(new MaxLengthRequirement(max));
        return this;
    }

    public PasswordOptionsBuilder WithReplacements(Dictionary<char, char> replacements)
    {
        _replacements = replacements;
        return this;
    }

    public PasswordOptionsBuilder WithDictionary(List<string> dictionary)
    {
        _dictionary = dictionary;
        return this;
    }

    public PasswordOptionsBuilder AsInteractive()
    {
        _interactive = true;
        return this;
    }

    public PasswordOptionsBuilder ExcludeSimilarCharacters()
    {
        _excludeSimilar = true;
        return this;
    }

    public PasswordOptionsBuilder AllowUppercase() { _uppercase = true; return this; }
    public PasswordOptionsBuilder AllowLowercase() { _lowercase = true; return this; }
    public PasswordOptionsBuilder AllowDigits()    { _digits = true; return this; }
    public PasswordOptionsBuilder AllowSymbols()   { _symbols = true; return this; }

    public PasswordOptionsBuilder RequireUppercase()
    {
        _requirements.Add(new UppercaseRequirement());
        return this;
    }

    public PasswordOptionsBuilder RequireDigit()
    {
        _requirements.Add(new DigitRequirement());
        return this;
    }

    public PasswordOptionsBuilder RequireSymbol()
    {
        _requirements.Add(new SymbolRequirement());
        return this;
    }

    public PasswordOptionsBuilder AddAlphabet(IAlphabet alphabet)
    {
        _alphabets.Add(alphabet);
        return this;
    }

    public PasswordOptionsBuilder AddVerifier(IPasswordVerifier verifier)
    {
        _verifiers.Add(verifier);
        return this;
    }

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
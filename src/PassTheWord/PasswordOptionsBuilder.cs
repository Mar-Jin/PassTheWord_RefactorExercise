namespace PassTheWord;

public class PasswordOptionsBuilder
{
    private Dictionary<char, char> _replacements = new();
    private List<string>? _dictionary;
    private int _minLength = 8;
    private int _maxLength = 20;
    private bool _excludeSimilar;
    private bool _interactive;
    private bool _uppercase;
    private bool _lowercase;
    private bool _digits;
    private bool _symbols;
    private bool _reqUpper;
    private bool _reqDigit;
    private bool _reqSymbol;

    public PasswordOptionsBuilder WithLength(int min, int max)
    {
        _minLength = min;
        _maxLength = max;
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

    public PasswordOptionsBuilder RequireUppercase() { _reqUpper = true; return this; }
    public PasswordOptionsBuilder RequireDigit()     { _reqDigit = true; return this; }
    public PasswordOptionsBuilder RequireSymbol()    { _reqSymbol = true; return this; }

    public PasswordOptions Build()
    {
        if (_minLength > _maxLength)
        {
            throw new InvalidOperationException("MinLength may not be more than MaxLength");
        }
        
        return new PasswordOptions
        {
            Replacements = _replacements,
            Dictionary = _dictionary,
            MinLength = _minLength,
            MaxLength = _maxLength,
            ExcludeSimilar = _excludeSimilar,
            Interactive = _interactive,
            Uppercase = _uppercase,
            Lowercase = _lowercase,
            Digits = _digits,
            Symbols = _symbols,
            ReqUpper = _reqUpper,
            ReqDigit = _reqDigit,
            ReqSymbol = _reqSymbol
        };
    }
}
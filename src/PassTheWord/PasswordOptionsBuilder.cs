namespace PassTheWord;

public class PasswordOptionsBuilder
{
    private readonly PasswordOptions _options = new();

    public PasswordOptionsBuilder WithLength(int min, int max)
    {
        _options.MinLength = min;
        _options.MaxLength = max;
        return this;
    }

    public PasswordOptionsBuilder WithReplacements(Dictionary<char, char> replacements)
    {
        _options.Replacements = replacements;
        return this;
    }

    public PasswordOptionsBuilder WithDictionary(List<string> dictionary)
    {
        _options.Dictionary = dictionary;
        return this;
    }

    public PasswordOptionsBuilder AsInteractive()
    {
        _options.Interactive = true;
        return this;
    }

    public PasswordOptionsBuilder ExcludeSimilarCharacters()
    {
        _options.ExcludeSimilar = true;
        return this;
    }

    public PasswordOptionsBuilder AllowUppercase() { _options.Uppercase = true; return this; }
    public PasswordOptionsBuilder AllowLowercase() { _options.Lowercase = true; return this; }
    public PasswordOptionsBuilder AllowDigits()    { _options.Digits = true; return this; }
    public PasswordOptionsBuilder AllowSymbols()   { _options.Symbols = true; return this; }

    public PasswordOptionsBuilder RequireUppercase() { _options.ReqUpper = true; return this; }
    public PasswordOptionsBuilder RequireDigit()     { _options.ReqDigit = true; return this; }
    public PasswordOptionsBuilder RequireSymbol()    { _options.ReqSymbol = true; return this; }

    public PasswordOptions Build()
    {
        if (_options.MinLength > _options.MaxLength)
        {
            throw new InvalidOperationException("MinLength may not be more than MaxLength");
        }
        
        return _options;
    }
}
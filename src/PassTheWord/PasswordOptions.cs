namespace PassTheWord;

public class PasswordOptions
{
    public Dictionary<char, char> Replacements { get; init; } = new();
    public List<string>? Dictionary { get; init; }
    public int MinLength { get; init; } = 8;
    public int MaxLength { get; init; } = 20;
    public bool ExcludeSimilar { get; init; }
    public bool Interactive { get; init; }
    public bool Uppercase { get; init; }
    public bool Lowercase { get; init; }
    public bool Digits { get; init; }
    public bool Symbols { get; init; }
    public bool ReqUpper { get; init; }
    public bool ReqDigit { get; init; }
    public bool ReqSymbol { get; init; }
}
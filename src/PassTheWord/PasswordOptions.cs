using PassTheWord.Requirements;

namespace PassTheWord;

public class PasswordOptions
{
    public Dictionary<char, char> Replacements { get; init; } = new();
    public List<string>? Dictionary { get; init; }

    public RequirementCollection Requirements { get; init; } = new();
    
    public int MinLength { get; init; } = 8;
    public int MaxLength { get; init; } = 20;
    public bool ExcludeSimilar { get; init; }
    public bool Interactive { get; init; }
    public bool Uppercase { get; init; }
    public bool Lowercase { get; init; }
    public bool Digits { get; init; }
    public bool Symbols { get; init; }
}
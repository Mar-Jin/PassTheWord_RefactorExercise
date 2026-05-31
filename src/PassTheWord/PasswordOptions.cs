using PassTheWord.Requirements;
using PassTheWord.Alphabets;
using PassTheWord.Verification;

namespace PassTheWord;

public class PasswordOptions
{
    public List<IAlphabet> Alphabets { get; init; } = new() { new LatinAlphabet() };
    public List<IPasswordVerifier> Verifiers { get; init; } = new();
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
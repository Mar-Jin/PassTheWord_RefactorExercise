namespace PassTheWord;

public class PasswordOptions
{
    public Dictionary<char, char> Replacements { get; set; } = new();
    public List<string>? Dictionary { get; set; } = null;
    
    public int MinLength { get; set; } = 8;
    public int MaxLength { get; set; } = 20;
    
    public bool ExcludeSimilar { get; set; } = false;
    public bool Interactive { get; set; } = false;
    
    public bool Uppercase { get; set; } = false;
    public bool Lowercase { get; set; } = false;
    public bool Digits { get; set; } = false;
    public bool Symbols { get; set; } = false;
    
    public bool ReqUpper { get; set; } = false;
    public bool ReqDigit { get; set; } = false;
    public bool ReqSymbol { get; set; } = false;
}
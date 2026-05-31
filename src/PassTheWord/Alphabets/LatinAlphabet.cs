namespace PassTheWord.Alphabets;

public class LatinAlphabet : IAlphabet
{
    public string Name => "Latin";

    public string GetUppercase(bool excludeSimilar) => 
        excludeSimilar ? "ABCDEFGHJKLMNPQRSTUVWXYZ" : "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

    public string GetLowercase(bool excludeSimilar) => 
        excludeSimilar ? "abcdefghijkmnopqrstuvwxyz" : "abcdefghijklmnopqrstuvwxyz";

    public string GetDigits(bool excludeSimilar) => 
        excludeSimilar ? "23456789" : "0123456789";

    public string GetSymbols(bool excludeSimilar) => 
        "!@#$%^&*()_+-=,./?~";

    public string GetSymbols(bool excludeSimilar, string customSymbols) => 
        string.IsNullOrEmpty(customSymbols) ? "!@#$%^&*()_+-=,./?~" : customSymbols;
}

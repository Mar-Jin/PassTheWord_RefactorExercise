namespace PassTheWord.Alphabets;

public interface IAlphabet
{
    string Name { get; }
    string GetUppercase(bool excludeSimilar);
    string GetLowercase(bool excludeSimilar);
    string GetDigits(bool excludeSimilar);
    string GetSymbols(bool excludeSimilar);
}

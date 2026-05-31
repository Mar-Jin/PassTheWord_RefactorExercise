namespace PassTheWord.Strategies;

public interface IPasswordStrategy
{
    (int len, char[] buf) Generate(Span<char> buf, int minLength, int maxLength, Dictionary<char, char> replacements);
}
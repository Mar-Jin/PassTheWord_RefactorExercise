namespace PassTheWord.Strategies;

public interface IPasswordStrategy
{
    (int len, char[] buf) Generate();
}
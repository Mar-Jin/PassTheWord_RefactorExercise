namespace PassTheWord.Strategies;

public static class PasswordStrategyFactory
{
    public static IPasswordStrategy Create(PasswordOptions options)
    {
        if (options.Dictionary != null && options.Dictionary.Count > 0)
        {
            return new WordListStrategy(options.Dictionary);
        }

        return new RandomCharacterStrategy(options);
    }
}
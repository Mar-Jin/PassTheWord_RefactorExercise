namespace PassTheWord.Strategies;

public static class PasswordStrategyFactory
{
    public static IPasswordStrategy Create(PasswordOptions options)
    {
        if (options.Dictionary != null && options.Dictionary.Count > 0)
        {
            return new WordListStrategy(options);
        }

        if (options.Interactive)
        {
            return new InteractivePassphraseStrategy(options);
        }

        return new RandomCharacterStrategy(options);
    }
}
namespace PassTheWord.Strategies;

public class PasswordStrategyFactory
{
    private static PasswordStrategyFactory? _instance;
    public static PasswordStrategyFactory Instance => _instance ??= new();
    private PasswordStrategyFactory() { }
    
    public IPasswordStrategy Create(PasswordOptions options)
    {
        if (options.Dictionary != null)
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
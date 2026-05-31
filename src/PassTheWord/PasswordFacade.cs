using System.Text;
using RND = System.Security.Cryptography.RandomNumberGenerator;
using PassTheWord.Strategies;
using Microsoft.Extensions.Logging;

namespace PassTheWord;

public class PasswordFacade
{
    private readonly PasswordStrategyFactory _factory;
    public PasswordFacade() : this(PasswordStrategyFactory.Instance) { }
    public PasswordFacade(PasswordStrategyFactory factory)
    {
        _factory = factory;
    }

    public (int len, char[] buf) GeneratePassword(PasswordOptions options, Span<char> destination)
    {
        if (destination.Length < options.MinLength)
        {
            throw new ArgumentException("The length of the destination buffer is less than the minimum length requested", nameof(destination));
        }

        IPasswordStrategy strategy = _factory.Create(options);
        return strategy.Generate();
    }
}
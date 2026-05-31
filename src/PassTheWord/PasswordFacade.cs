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

    public (int, char[]) GeneratePassword(PasswordOptions options, Span<char> buf)
    {
        Random rnd = new();
        int len = 0;

        // check buf.Length >= minlength
        if (!(buf.Length >= options.MinLength))
            throw new ArgumentException("The length of the buffer is less than the minimum length requested");

        IPasswordStrategy strategy = _factory.Create(options);

        return strategy.Generate();
    }
}
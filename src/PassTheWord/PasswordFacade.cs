using System.Text;
using PassTheWord.Strategies;

namespace PassTheWord;

/// <summary>
/// The main entry point for the password generation system.
/// This facade simplifies access to various generation strategies and configurations.
/// </summary>
public class PasswordFacade
{
    private readonly PasswordStrategyFactory _factory;

    public PasswordFacade() : this(PasswordStrategyFactory.Instance) { }

    public PasswordFacade(PasswordStrategyFactory factory)
    {
        _factory = factory;
    }

    /// <summary>
    /// Generates a password based on the provided options and writes it to the destination span.
    /// </summary>
    /// <param name="options">The configuration options for password generation.</param>
    /// <param name="destination">The buffer where the generated password will be stored.</param>
    /// <returns>A tuple containing the length of the generated password and a reference to the buffer.</returns>
    /// <exception cref="ArgumentException">Thrown if the destination buffer is too small for the requested minimum length.</exception>
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

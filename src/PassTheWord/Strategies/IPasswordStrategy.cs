namespace PassTheWord.Strategies;

/// <summary>
/// Defines a contract for password generation strategies.
/// Different strategies (e.g., random character, word list) implement this interface.
/// </summary>
public interface IPasswordStrategy
{
    /// <summary>
    /// Executes the generation process and returns the resulting password.
    /// </summary>
    /// <returns>A tuple containing the length of the password and the character buffer.</returns>
    (int len, char[] buf) Generate();
}
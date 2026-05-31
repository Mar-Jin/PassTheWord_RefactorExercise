namespace PassTheWord.Alphabets;

/// <summary>
/// Defines a contract for character sets (alphabets) used in password generation.
/// Implement this interface to add support for different languages or custom character groups.
/// </summary>
public interface IAlphabet
{
    /// <summary>
    /// Gets the display name of the alphabet (e.g., "Latin", "Cyrillic").
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Returns a string containing all available uppercase letters for this alphabet.
    /// </summary>
    /// <param name="excludeSimilar">If true, characters that look similar (e.g., 'O' and '0') should be omitted.</param>
    /// <returns>A string of uppercase characters.</returns>
    string GetUppercase(bool excludeSimilar);

    /// <summary>
    /// Returns a string containing all available lowercase letters for this alphabet.
    /// </summary>
    /// <param name="excludeSimilar">If true, characters that look similar should be omitted.</param>
    /// <returns>A string of lowercase characters.</returns>
    string GetLowercase(bool excludeSimilar);

    /// <summary>
    /// Returns a string containing all available digits for this alphabet.
    /// </summary>
    /// <param name="excludeSimilar">If true, characters that look similar should be omitted.</param>
    /// <returns>A string of digit characters.</returns>
    string GetDigits(bool excludeSimilar);

    /// <summary>
    /// Returns a string containing all available symbols for this alphabet.
    /// </summary>
    /// <param name="excludeSimilar">If true, characters that look similar should be omitted.</param>
    /// <returns>A string of symbol characters.</returns>
    string GetSymbols(bool excludeSimilar);
}

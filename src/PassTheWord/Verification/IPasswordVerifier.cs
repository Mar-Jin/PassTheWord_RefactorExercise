namespace PassTheWord.Verification;

/// <summary>
/// Defines a contract for external password verification services.
/// This allows integrating with third-party lists of leaked passwords or custom security policies.
/// </summary>
public interface IPasswordVerifier
{
    /// <summary>
    /// Gets the name of the verifier or the service it connects to.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Gets the name of the hash algorithm required by this verifier (e.g., "SHA256", "SHA512").
    /// The password generator will provide a hash using this algorithm.
    /// </summary>
    string HashAlgorithmName { get; }

    /// <summary>
    /// Checks if the generated password (represented by its hash) is considered safe by the verifier.
    /// </summary>
    /// <param name="hash">The binary hash of the generated password.</param>
    /// <returns>True if the password is safe; otherwise, false.</returns>
    bool IsSafe(byte[] hash);
}

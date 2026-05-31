namespace PassTheWord.Verification;

public interface IPasswordVerifier
{
    string Name { get; }
    string HashAlgorithmName { get; }
    bool IsSafe(byte[] hash);
}

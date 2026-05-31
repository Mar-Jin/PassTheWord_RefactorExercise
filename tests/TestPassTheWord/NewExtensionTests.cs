using NUnit.Framework;
using PassTheWord;
using System.Linq;
using PassTheWord.Alphabets;
using PassTheWord.Verification;

namespace TestPassTheWord;

[TestFixture]
public class NewExtensionTests : TestBase
{
    [Test]
    public void Test_CyrillicAlphabet_Generation()
    {
        Log("Starting Test_CyrillicAlphabet_Generation");
        // ARRANGE
        var options = new PasswordOptionsBuilder()
            .AddAlphabet(new CyrillicAlphabet())
            .AllowLowercase()
            .WithLength(10, 10)
            .Build();

        // ACT
        var (len, resultBuf) = Facade.GeneratePassword(options, Buffer);
        string generatedPassword = GetString(resultBuf, len);
        Log($"Generated Cyrillic password: {generatedPassword}");

        // ASSERT
        // Check if characters are in the Russian Cyrillic range (approx \u0410-\u044F)
        bool hasCyrillic = generatedPassword.Any(c => c >= 0x0400 && c <= 0x04FF);
        Assert.That(hasCyrillic, Is.True, "Password should contain Cyrillic characters");
    }

    [Test]
    public void Test_ExternalVerifier_RejectingPassword()
    {
        Log("Starting Test_ExternalVerifier_RejectingPassword");
        // ARRANGE
        var verifier = new AlwaysRejectVerifier();
        var options = new PasswordOptionsBuilder()
            .AllowLowercase()
            .WithLength(5, 5)
            .AddVerifier(verifier)
            .Build();

        // ACT & ASSERT
        // This might loop a few times, but BasePasswordStrategy has a loop.
        // If it never finds a safe one, it will loop. 
        // For testing purposes, we want to see it call the verifier.
        
        Log("This test demonstrates the verifier connection.");
        // We'll use a mock-like approach by checking the verifier's call count if we had one.
        // For now, we just ensure it can be added and the process runs.
        var (len, resultBuf) = Facade.GeneratePassword(options, Buffer);
        Log($"Generated password with verifier: {GetString(resultBuf, len)}");
        Assert.That(verifier.CallCount, Is.GreaterThan(0), "Verifier should have been called");
    }

    private class AlwaysRejectVerifier : IPasswordVerifier
    {
        public string Name => "Rejector";
        public string HashAlgorithmName => "SHA256";
        public int CallCount { get; private set; }

        public bool IsSafe(byte[] hash)
        {
            CallCount++;
            return CallCount > 5; // Allow after 5 tries to avoid infinite loop in test
        }
    }
}

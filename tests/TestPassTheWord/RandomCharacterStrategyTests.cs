using NUnit.Framework;
using PassTheWord;
using System.Linq;

namespace TestPassTheWord;

[TestFixture]
public class RandomCharacterStrategyTests : TestBase
{
    [Test]
    public void Test_Random_With_All_Requirements()
    {
        Log("Starting Test_Random_With_All_Requirements");
        // ARRANGE
        PasswordOptions options = new PasswordOptionsBuilder()
            .WithLength(10, 20)
            .AllowUppercase()
            .AllowLowercase()
            .AllowDigits()
            .AllowSymbols()
            .RequireUppercase()
            .RequireDigit()
            .RequireSymbol()
            .Build();

        // ACT
        var (len, resultBuf) = Facade.GeneratePassword(options, Buffer);
        string generatedPassword = GetString(resultBuf, len);
        Log($"Generated password: {generatedPassword}");

        // ASSERT
        Assert.That(generatedPassword.Any(char.IsUpper), Is.True, "Missing uppercase");
        Assert.That(generatedPassword.Any(char.IsLower), Is.True, "Missing lowercase");
        Assert.That(generatedPassword.Any(char.IsDigit), Is.True, "Missing digit");
        Assert.That(generatedPassword.Any(c => !char.IsLetterOrDigit(c)), Is.True, "Missing symbol");
    }

    [Test]
    public void Test_Random_Exclude_Similar()
    {
        Log("Starting Test_Random_Exclude_Similar");
        // ARRANGE
        PasswordOptions options = new PasswordOptionsBuilder()
            .WithLength(50, 50)
            .AllowUppercase()
            .AllowLowercase()
            .AllowDigits()
            .ExcludeSimilarCharacters()
            .Build();

        // ACT
        var (len, resultBuf) = Facade.GeneratePassword(options, Buffer);
        string generatedPassword = GetString(resultBuf, len);
        Log($"Generated password: {generatedPassword}");

        // ASSERT
        string similar = "IOl01";
        foreach (char c in similar)
        {
            Assert.That(generatedPassword, Does.Not.Contain(c), $"Password should not contain similar character '{c}'");
        }
    }

    [Test]
    public void Test_Random_Statistical_Replacements()
    {
        Log("Starting Test_Random_Statistical_Replacements");
        // ARRANGE
        var subs = new Dictionary<char, char> { { 'o', '0' }, { 'i', '1' } };
        PasswordOptions options = new PasswordOptionsBuilder()
            .WithReplacements(subs)
            .AllowLowercase()
            .WithLength(25, 40)
            .Build();

        bool replacementFound = false;

        // ACT
        for (int i = 0; i < 20; i++)
        {
            var (len, resultBuf) = Facade.GeneratePassword(options, Buffer);
            string password = GetString(resultBuf, len);

            if (password.Contains("0") || password.Contains("1"))
            {
                replacementFound = true;
                Log($"Replacement detected in run {i + 1}: {password}");
                break;
            }
        }

        // ASSERT
        Assert.That(replacementFound, Is.True, "No replacements were made in 20 runs.");
    }
}

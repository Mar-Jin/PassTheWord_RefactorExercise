using NUnit.Framework;
using PassTheWord;
using System.Collections.Generic;

namespace TestPassTheWord;

[TestFixture]
public class WordListStrategyTests : TestBase
{
    private List<string> _words;

    [SetUp]
    public override void Setup()
    {
        base.Setup();
        _words = new List<string> { "apple", "banana", "cherry", "date" };
    }

    [Test]
    public void Test_WordList_Length_Requirement()
    {
        Log("Starting Test_WordList_Length_Requirement");
        // ARRANGE
        PasswordOptions options = new PasswordOptionsBuilder()
            .WithDictionary(_words)
            .WithLength(20, 40)
            .Build();

        // ACT
        var (len, resultBuf) = Facade.GeneratePassword(options, Buffer);
        string generatedPassword = GetString(resultBuf, len);
        Log($"Generated password: {generatedPassword}");

        // ASSERT
        Assert.That(len, Is.GreaterThanOrEqualTo(20), "Password too short");
    }

    [Test]
    public void Test_WordList_With_Replacements()
    {
        Log("Starting Test_WordList_With_Replacements");
        // ARRANGE
        var subs = new Dictionary<char, char> { { 'a', '@' }, { 'e', '3' } };
        
        PasswordOptions options = new PasswordOptionsBuilder()
            .WithDictionary(_words)
            .WithReplacements(subs)
            .WithLength(20, 40)
            .Build();

        bool replacementFound = false;

        // ACT
        for (int i = 0; i < 20; i++)
        {
            var (len, resultBuf) = Facade.GeneratePassword(options, Buffer);
            string generatedPassword = GetString(resultBuf, len);

            if (generatedPassword.Contains('@') || generatedPassword.Contains('3'))
            {
                replacementFound = true;
                Log($"Replacement detected in run {i + 1}: {generatedPassword}");
                break;
            }
        }

        // ASSERT
        Assert.That(replacementFound, Is.True, "No replacements were made in 20 runs.");
    }

    [Test]
    public void Test_WordList_SurrogateCharacters_ThrowsArgumentException()
    {
        Log("Starting Test_WordList_SurrogateCharacters_ThrowsArgumentException");
        // ARRANGE
        var surrogateWords = new List<string> { "𠜎" };
        PasswordOptions options = new PasswordOptionsBuilder()
            .WithDictionary(surrogateWords)
            .Build();

        // ACT & ASSERT
        var ex = Assert.Throws<ArgumentException>(() => Facade.GeneratePassword(options, Buffer));
        Log($"Caught expected exception: {ex.Message}");
    }

    [Test]
    public void Test_WordList_WordTooLongForBuffer_ThrowsArgumentException()
    {
        Log("Starting Test_WordList_WordTooLongForBuffer_ThrowsArgumentException");
        char[] tightBuf = new char[5];

        PasswordOptions options = new PasswordOptionsBuilder()
            .WithDictionary(_words)
            .WithLength(10, 15)
            .Build();

        // ACT & ASSERT
        var ex = Assert.Throws<ArgumentException>(() => Facade.GeneratePassword(options, tightBuf));
        Log($"Caught expected exception: {ex.Message}");
    }
}

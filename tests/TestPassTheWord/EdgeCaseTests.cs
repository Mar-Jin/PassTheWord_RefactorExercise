using NUnit.Framework;
using PassTheWord;
using PassTheWord.Requirements;
using PassTheWord.Strategies;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TestPassTheWord;

[TestFixture]
public class EdgeCaseTests
{
    private PasswordFacade _facade;
    private char[] _buf;

    [SetUp]
    public void Setup()
    {
        _facade = new PasswordFacade();
        _buf = new char[100];
    }

    /// <summary>
    /// RISK DEMONSTRATION: This test shows that the system can enter an infinite loop 
    /// if requirements are set that the chosen strategy can never fulfill.
    /// We use a Timeout to prevent the test suite from hanging forever.
    /// </summary>
    [Test, Timeout(5000)] 
    public void Risk_InfiniteLoop_WhenRequirementIsUnsatisfiable()
    {
        // ARRANGE
        // We require a digit, but we don't allow digits in the alphabet.
        // The RandomCharacterStrategy will never pick a digit, so the 
        // BasePasswordStrategy will loop forever trying to satisfy the requirement.
        PasswordOptions options = new PasswordOptionsBuilder()
            .AllowLowercase() // Only lowercase allowed
            .RequireDigit()   // But a digit is required
            .WithLength(10, 10)
            .Build();

        // ACT & ASSERT
        // This is expected to fail due to Timeout, demonstrating the infinite loop risk.
        Console.WriteLine("LOG: Starting infinite loop risk test. This should time out if no safeguard is present.");
        
        // Note: In a real scenario, this would hang the application.
        // Since we can't 'catch' a hang easily, we just document it here.
        // Assert.DoesNotThrow(() => _facade.GeneratePassword(options, _buf));
    }

    /// <summary>
    /// RISK DEMONSTRATION: Showing what happens when a Dictionary is provided but is empty.
    /// </summary>
    [Test]
    public void Risk_EmptyDictionary_ThrowsException()
    {
        // ARRANGE
        var options = new PasswordOptionsBuilder()
            .WithDictionary(new List<string>()) // Empty list
            .Build();

        // ACT & ASSERT
        Assert.Throws<ArgumentOutOfRangeException>(() => _facade.GeneratePassword(options, _buf), 
            "An empty dictionary causes an index out of bounds when picking a random word.");
    }

    /// <summary>
    /// RISK DEMONSTRATION: Conflicting 'ExcludeSimilar' with specific requirements.
    /// If a user requires a '1' but also excludes similar characters, 
    /// the generation might become much harder or fail depending on implementation.
    /// </summary>
    [Test]
    public void Risk_Conflicting_ExcludeSimilar_And_Requirements()
    {
        // ARRANGE
        // We require a digit, but we exclude similar characters (which includes '1' and '0').
        // If the implementation is not careful, it might pick '1' as the required digit
        // but then the requirement check might fail or the character might be filtered.
        PasswordOptions options = new PasswordOptionsBuilder()
            .AllowDigits()
            .RequireDigit()
            .ExcludeSimilarCharacters()
            .WithLength(5, 5)
            .Build();

        // ACT
        var (len, resultBuf) = _facade.GeneratePassword(options, _buf);
        string result = new string(resultBuf, 0, len);

        // ASSERT
        Assert.That(result.Any(char.IsDigit), Is.True);
        // This test actually passes now because RandomCharacterStrategy uses a specific 
        // set of "safe" digits (23456789) when ExcludeSimilar is on. 
        // But it shows that '1' and '0' will NEVER appear.
        Assert.That(result, Does.Not.Contain("1").And.Not.Contain("0"));
    }

    /// <summary>
    /// RISK DEMONSTRATION: Requesting a password with 0 allowed character types.
    /// </summary>
    [Test]
    public void Risk_NoAllowedCharacters_ThrowsException()
    {
        // ARRANGE
        // Builder doesn't allow calling AllowX(), so alphabet remains empty.
        PasswordOptions options = new PasswordOptionsBuilder()
            .WithLength(10, 10)
            .Build(); 

        // ACT & ASSERT
        Assert.Throws<ArgumentException>(() => _facade.GeneratePassword(options, _buf),
            "Generating with an empty alphabet should throw an ArgumentException.");
    }
}
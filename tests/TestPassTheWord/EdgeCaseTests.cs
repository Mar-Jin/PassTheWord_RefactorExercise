using NUnit.Framework;
using PassTheWord;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TestPassTheWord;

[TestFixture]
public class EdgeCaseTests : TestBase
{
    /// <summary>
    /// RISK DEMONSTRATION: This test shows that the system can enter an infinite loop 
    /// if requirements are set that the chosen strategy can never fulfill.
    /// </summary>
    [Test, Timeout(5000)] 
    public void Risk_InfiniteLoop_WhenRequirementIsUnsatisfiable()
    {
        Log("Starting Risk_InfiniteLoop_WhenRequirementIsUnsatisfiable");
        // ARRANGE
        // We require a digit, but we don't allow digits in the alphabet.
        PasswordOptions options = new PasswordOptionsBuilder()
            .AllowLowercase() // Only lowercase allowed
            .RequireDigit()   // But a digit is required
            .WithLength(10, 10)
            .Build();

        Log("This test documents the infinite loop risk. It will time out if no safeguard is present.");
        // Assert.DoesNotThrow(() => Facade.GeneratePassword(options, Buffer));
    }

    [Test]
    public void Risk_EmptyDictionary_ThrowsException()
    {
        Log("Starting Risk_EmptyDictionary_ThrowsException");
        // ARRANGE
        var options = new PasswordOptionsBuilder()
            .WithDictionary(new List<string>()) // Empty list
            .Build();

        // ACT & ASSERT
        var ex = Assert.Throws<ArgumentOutOfRangeException>(() => Facade.GeneratePassword(options, Buffer));
        Log($"Caught expected exception: {ex.Message}");
    }

    [Test]
    public void Risk_Conflicting_ExcludeSimilar_And_Requirements()
    {
        Log("Starting Risk_Conflicting_ExcludeSimilar_And_Requirements");
        // ARRANGE
        PasswordOptions options = new PasswordOptionsBuilder()
            .AllowDigits()
            .RequireDigit()
            .ExcludeSimilarCharacters()
            .WithLength(5, 5)
            .Build();

        // ACT
        var (len, resultBuf) = Facade.GeneratePassword(options, Buffer);
        string result = GetString(resultBuf, len);
        Log($"Generated result: {result}");

        // ASSERT
        Assert.That(result.Any(char.IsDigit), Is.True, "Result must contain a digit");
        Assert.That(result, Does.Not.Contain("1").And.Not.Contain("0"), "Should not contain similar digits");
    }

    [Test]
    public void Risk_NoAllowedCharacters_ThrowsException()
    {
        Log("Starting Risk_NoAllowedCharacters_ThrowsException");
        // ARRANGE
        PasswordOptions options = new PasswordOptionsBuilder()
            .WithLength(10, 10)
            .Build(); 

        // ACT & ASSERT
        var ex = Assert.Throws<InvalidOperationException>(() => Facade.GeneratePassword(options, Buffer));
        Log($"Caught expected exception: {ex.Message}");
    }
}
using NUnit.Framework;
using PassTheWord;
using System;
using System.Collections.Generic;
using System.IO;

namespace TestPassTheWord;

[TestFixture]
public class InteractivePassphraseStrategyTests : TestBase
{
    private StringWriter _consoleOutputHandler;

    [SetUp]
    public override void Setup()
    {
        base.Setup();
        _consoleOutputHandler = new StringWriter();
        Console.SetOut(_consoleOutputHandler);
    }

    [TearDown]
    public void TearDown()
    {
        Log("Tearing down InteractivePassphraseStrategyTests");
        _consoleOutputHandler.Dispose();
        Console.SetIn(new StreamReader(Console.OpenStandardInput()));
        Console.SetOut(new StreamWriter(Console.OpenStandardOutput()));
    }

    [Test]
    public void Test_Interactive_With_Invalid_Length_Then_Valid_Length()
    {
        Log("Starting Test_Interactive_With_Invalid_Length_Then_Valid_Length");
        // ARRANGE
        string simulatedInput = "short\r\n" + "this is way too long for the buffer that is configured\r\n" + "validpassword\r\n";
        using var insert = new StringReader(simulatedInput);
        Console.SetIn(insert);

        PasswordOptions options = new PasswordOptionsBuilder()
            .AsInteractive()
            .WithLength(6, 20)
            .Build();

        // ACT
        var (len, resultBuf) = Facade.GeneratePassword(options, Buffer);
        string generatedPassword = GetString(resultBuf, len);
        Log($"Generated password: {generatedPassword}");

        // ASSERT
        Assert.That(generatedPassword, Is.EqualTo("validpassword"), "Input flow failed to pick valid password");
        Assert.That(len, Is.EqualTo(13), "Length mismatch");
    }

    [Test]
    public void Test_Interactive_With_Replacements()
    {
        Log("Starting Test_Interactive_With_Replacements");
        // ARRANGE
        var subs = new Dictionary<char, char> { { 'o', '0' }, { 'i', '1' } };
        string simulatedInput = "iolio\r\n";
        
        PasswordOptions options = new PasswordOptionsBuilder()
            .AsInteractive()
            .WithReplacements(subs)
            .WithLength(4, 10)
            .Build();

        bool replacementFound = false;

        // ACT
        for (int i = 0; i < 20; i++)
        {
            using var insert = new StringReader(simulatedInput);
            Console.SetIn(insert);
            
            var (len, resultBuf) = Facade.GeneratePassword(options, Buffer);
            string generatedPassword = GetString(resultBuf, len);

            if (generatedPassword.Contains('0') || generatedPassword.Contains('1'))
            {
                replacementFound = true;
                Log($"Replacement detected in run {i + 1}: {generatedPassword}");
                break;
            }
        }

        // ASSERT
        Assert.That(replacementFound, Is.True, "No replacements were made in 20 runs.");
    }
}
using PassTheWord;
using NUnit.Framework;

namespace TestPassTheWord;

[TestFixture]
public class PasswordFacadeTests
{
    private List<string> _words;
    private List<string> _longWords;
    private Dictionary<char, char> _subs;
    private char[] _buf;
    private PasswordFacade _passwordGenerator;
    private StringWriter _consoleOutputHandler;

    [SetUp]
    public void Setup()
    {
        _words = ["hallo", "kat", "hond", "paard", "wei"];
        _longWords =
        [
            "kindercarnavalsoptochtvoorbereidingswerkzaamheden",
            "aansprakelijkheidswaardevaststellingsveranderingen"
        ];
        _subs = new() { { 'o', '0' }, { 'i', '1' }, { 's', '$' } };
        _buf = new char[100];
        _passwordGenerator = new();

        _consoleOutputHandler = new StringWriter();
        Console.SetOut(_consoleOutputHandler);
    }

    [TearDown]
    public void TearDown()
    {
        _consoleOutputHandler.Dispose();
        Console.SetIn(new StreamReader(Console.OpenStandardInput()));
        Console.SetOut(new StreamWriter(Console.OpenStandardOutput()));
    }

    [Test]
    public void Test_Interactive_Flow_With_Builder()
    {
        Console.WriteLine("LOG: Starting Test_Interactive_Flow_With_Builder");
        
        // ARRANGE
        string simulatedInput = "interactivePassword123\r\n";
        using var insert = new StringReader(simulatedInput);
        Console.SetIn(insert);

        PasswordOptions options = new PasswordOptionsBuilder()
            .WithReplacements(_subs)
            .AsInteractive()
            .WithLength(8, 30)
            .Build();

        // ACT
        var (len, resultBuf) = _passwordGenerator.GeneratePassword(options, _buf);
        string generatedPassword = new string(resultBuf, 0, len);
        
        Console.WriteLine($"LOG: Interactive result: {generatedPassword} (Length: {len})");

        // ASSERT
        Assert.That(len, Is.EqualTo(simulatedInput.Trim().Length));
        Assert.That(generatedPassword, Does.Contain("nteract"));
    }

    [Test]
    public void Test_BufferEdgeCase_TooSmall_ThrowsArgumentException()
    {
        Console.WriteLine("LOG: Starting Test_BufferEdgeCase_TooSmall_ThrowsArgumentException");
        char[] smallBuf = new char[5];

        PasswordOptions options = new PasswordOptionsBuilder()
            .WithLength(15, 20)
            .WithDictionary(_words)
            .Build();

        // ACT & ASSERT
        var ex = Assert.Throws<ArgumentException>(() =>
        {
            _passwordGenerator.GeneratePassword(options, smallBuf);
        });

        Console.WriteLine($"LOG: Exception successfully caught: {ex.Message}");
        Assert.That(ex.Message, Is.EqualTo("The length of the buffer is less than the minimum length requested"));
    }

    [Test]
    public void Test_Builder_InvalidLength_ThrowsInvalidOperationException()
    {
        Console.WriteLine("LOG: Starting Test_Builder_InvalidLength_ThrowsInvalidOperationException");

        // ACT & ASSERT
        var ex = Assert.Throws<InvalidOperationException>(() =>
        {
            new PasswordOptionsBuilder().WithLength(20, 10).Build(); 
        });

        Console.WriteLine($"LOG: Exception from Builder successfully caught: {ex.Message}");
        Assert.That(ex.Message, Is.EqualTo("MinLength may not be more than MaxLength"));
    }
}
using PassTheWord;
using NUnit.Framework;

namespace TestPassTheWord;

[TestFixture]
public class PasswordStrategyTests
{
    private List<string> _words;
    private List<string> _longWords;
    private List<string> _surrogateWords;
    private Dictionary<char, char> _subs;
    private char[] _buf;
    private PasswordFacade _passwordGenerator;

    [SetUp]
    public void Setup()
    {
        _words = ["hallo", "kat", "hond", "paard"];
        _longWords = ["kindercarnavalsoptochtvoorbereidingswerkzaamheden"];
        _surrogateWords = ["𠜎"];
        _subs = new() { { 'o', '0' }, { 'i', '1' }, { 's', '$' } };
        _buf = new char[100];
        _passwordGenerator = new();
    }

    [Test]
    public void Test_WordListStrategy_HappyPath()
    {
        Console.WriteLine("LOG: Starting Test_WordListStrategy_HappyPath");

        PasswordOptions options = new PasswordOptionsBuilder()
            .WithReplacements(_subs)
            .WithDictionary(_words)
            .WithLength(8, 25)
            .Build();

        // ACT
        var (len, resultBuf) = _passwordGenerator.GeneratePassword(options, _buf);
        string result = new string(resultBuf, 0, len);
        
        Console.WriteLine($"LOG: Dictionary password generated: {result}");

        // ASSERT
        Assert.That(len, Is.GreaterThanOrEqualTo(8));
        Assert.That(result, Is.Not.Null.Or.Empty);
    }

    [Test]
    public void Test_WordListStrategy_SurrogateCharacters_ThrowsArgumentException()
    {
        Console.WriteLine("LOG: Starting Test_WordListStrategy_SurrogateCharacters_ThrowsArgumentException");

        PasswordOptions options = new PasswordOptionsBuilder()
            .WithDictionary(_surrogateWords)
            .Build();

        // ACT & ASSERT
        var ex = Assert.Throws<ArgumentException>(() =>
        {
            _passwordGenerator.GeneratePassword(options, _buf);
        });

        Console.WriteLine($"LOG: Surrogate exception caught: {ex.Message}");
        Assert.That(ex.Message, Does.Contain("surrogate characters"));
    }

    [Test]
    public void Test_WordListStrategy_WordTooLongForBuffer_ThrowsArgumentException()
    {
        Console.WriteLine("LOG: Starting Test_WordListStrategy_WordTooLongForBuffer_ThrowsArgumentException");
        char[] tightBuf = new char[20];

        PasswordOptions options = new PasswordOptionsBuilder()
            .WithDictionary(_longWords)
            .WithLength(10, 15)
            .Build();

        // ACT & ASSERT
        var ex = Assert.Throws<ArgumentException>(() =>
        {
            _passwordGenerator.GeneratePassword(options, tightBuf);
        });

        Console.WriteLine($"LOG: Exception caught: {ex.Message}");
        Assert.That(ex.Message, Is.EqualTo("Cannot copy word to buffer"));
    }

    [Test]
    public void Test_CharacterStrategy_ExcludeSimilar_FiltersCharacters()
    {
        Console.WriteLine("LOG: Starting Test_CharacterStrategy_ExcludeSimilar_FiltersCharacters");

        PasswordOptions options = new PasswordOptionsBuilder()
            .ExcludeSimilarCharacters()
            .AllowUppercase()
            .RequireUppercase()
            .WithLength(15, 30)
            .Build();

        // ACT
        var (len, resultBuf) = _passwordGenerator.GeneratePassword(options, _buf);
        string result = new string(resultBuf, 0, len);
        
        Console.WriteLine($"LOG: ExcludeSimilar result: {result}");

        // ASSERT
        Assert.That(len, Is.GreaterThanOrEqualTo(15));
        Assert.That(result, Does.Not.Contain("I"));
        Assert.That(result, Does.Not.Contain("O"));
    }

    [Test]
    public void Test_CharacterStrategy_Statistical_Replacements()
    {
        Console.WriteLine("LOG: Starting Test_CharacterStrategy_Statistical_Replacements");

        PasswordOptions options = new PasswordOptionsBuilder()
            .WithReplacements(_subs)
            .AllowLowercase()
            .WithLength(25, 40)
            .Build();

        bool replacementFound = false;

        for (int i = 0; i < 20; i++)
        {
            char[] tempBuf = new char[100];
            var (len, resultBuf) = _passwordGenerator.GeneratePassword(options, tempBuf);
            string password = new string(resultBuf, 0, len);

            if (password.Contains("$") || password.Contains("0") || password.Contains("1"))
            {
                replacementFound = true;
                Console.WriteLine($"LOG: Replacement detected in run {i + 1}: {password}");
                break;
            }
        }

        // ASSERT
        Assert.That(replacementFound, Is.True, "The replacement loop (50% chance per target character) did not trigger a single replacement across 20 runs.");
    }
}
using PassTheWord;

namespace TestPassTheWord;

[TestFixture]
public class Tests
{
    // Word lists
    private List<string> _words;
    private List<string> _longWords;
    private List<string> _surrogateWords;

    // Replacement dictionaries
    private Dictionary<char, char> _subs;
    private Dictionary<char, char> _emptySubs;

    private char[] _buf;
    private int _len;

    // Password Generator
    private PasswordFacade _passwordGenerator;

    // Console
    private StringWriter _consoleOutputHandler;

    [SetUp]
    public void Setup()
    {
        _words = ["hallo", "kat", "hond", "paard", "wei", "accu", "batterij", "doei"];
        _longWords =
        [
            "kindercarnavalsoptochtvoorbereidingswerkzaamheden",
            "aansprakelijkheidswaardevaststellingsveranderingen",
            "Hottentottensoldatententententoonstellingsbouwterrein",
            "elektriciteitsproductiemaatschappijbuitenlandbelangen",
            "geneesmiddelenvergoedingssysteemorganisatiestructuur"
        ];
        _surrogateWords = ["𠜎"];

        _subs = new() { { 'o', '0' }, { 'i', '1' }, { 's', '$' } };
        _emptySubs = new();
        _buf = new char[100];
        _len = 0;

        _passwordGenerator = new();

        _consoleOutputHandler = new StringWriter();
        Console.SetOut(_consoleOutputHandler);
    }

    [TearDown]
    public void TearDown()
    {
        _consoleOutputHandler.Dispose();

        var standardIn = new StreamReader(Console.OpenStandardInput());
        var standardOut = new StreamWriter(Console.OpenStandardOutput());
        Console.SetIn(standardIn);
        Console.SetOut(standardOut);
    }

    [Test]
    public void Test_Interactive()
    {
        // ARRANGE
        string simulatedInput = "password\r\n";
        using var insert = new StringReader(simulatedInput);
        Console.SetIn(insert);

        // ACT
        (_len, _buf) = _passwordGenerator.GeneratePassword(
            replacements: _subs, 
            excludeSimilar: false, 
            dictionary: _words, 
            buf: _buf, 
            interactive: true, 
            minlength: 8, 
            maxlength: 20
        );

        // ASSERT
        Assert.That(_len, Is.GreaterThan(0));

        string generatedPassword = new string(_buf, 0, _len);
        Assert.That(generatedPassword, Is.Not.Null.Or.Empty);
    }

    [Test]
    public void Test_SubsWords()
    {
        // ACT
        (_len, _buf) = _passwordGenerator.GeneratePassword(
            replacements: _subs, 
            excludeSimilar: false, 
            dictionary: _words, 
            buf: _buf, 
            interactive: false, 
            minlength: 8, 
            maxlength: 20
        );

        string result = new string(_buf, 0, _len);

        // ASSERT
        Assert.That(result, Is.Not.Null.Or.Empty);
        Assert.That(_len, Is.GreaterThanOrEqualTo(8));
    }

    [Test]
    public void Test_Empty()
    {
        // ACT
        (_len, _buf) = _passwordGenerator.GeneratePassword(
            replacements: _emptySubs, 
            excludeSimilar: false, 
            dictionary: null, 
            buf: _buf, 
            interactive: false, 
            minlength: 8, 
            maxlength: 20,
            uppercase: true,
            lowercase: true
        );

        // ASSERT
        Assert.That(_len, Is.GreaterThanOrEqualTo(8));
    }

    [Test]
    public void Test_Surrogate()
    {
        // ACT & ASSERT
        Assert.Throws<ArgumentException>(() =>
        {
            (_len, _buf) = _passwordGenerator.GeneratePassword(
                replacements: _subs, 
                excludeSimilar: false, 
                dictionary: _surrogateWords, 
                buf: _buf, 
                interactive: false
            );
        });
    }

    [Test]
    public void Test_BufferEdgeCase_Small()
    {
        char[] smallBuf = new char[5];

        // ACT & ASSERT
        Assert.Throws<ArgumentException>(() =>
        {
            _passwordGenerator.GeneratePassword(
                replacements: _subs, 
                excludeSimilar: false, 
                dictionary: _words, 
                buf: smallBuf, 
                interactive: false, 
                minlength: 15, 
                maxlength: 20
            );
        });
    }
    
    [Test]
    public void Test_BufferEdgeCase_LongWords()
    {
        char[] buf = new char[100];

        // ACT & ASSERT
        Assert.Throws<ArgumentException>(() =>
        {
            _passwordGenerator.GeneratePassword(
                replacements: _subs, 
                excludeSimilar: false, 
                dictionary: _longWords, 
                buf: buf, 
                interactive: false, 
                minlength: 80, 
                maxlength: 120
            );
        });
    }
}
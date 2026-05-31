using NUnit.Framework;
using PassTheWord;

namespace TestPassTheWord;

public abstract class TestBase
{
    protected PasswordFacade Facade;
    protected char[] Buffer;
    protected const int DefaultBufferLength = 100;

    [SetUp]
    public virtual void Setup()
    {
        Log($"Setting up {GetType().Name}");
        Facade = new PasswordFacade();
        Buffer = new char[DefaultBufferLength];
    }

    protected void Log(string message)
    {
        TestContext.WriteLine($"[LOG] {DateTime.Now:HH:mm:ss.fff} - {message}");
    }

    protected string GetString(char[] buf, int len)
    {
        return new string(buf, 0, len);
    }
}

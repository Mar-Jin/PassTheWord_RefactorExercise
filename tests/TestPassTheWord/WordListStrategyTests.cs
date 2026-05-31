using NUnit.Framework;
using PassTheWord;
using System.Collections.Generic;

namespace TestPassTheWord
{
    [TestFixture]
    public class WordListStrategyTests
    {
        private PasswordFacade _passwordGenerator;
        private char[] _buf;
        private List<string> _words;

        [SetUp]
        public void Setup()
        {
            _passwordGenerator = new PasswordFacade();
            _buf = new char[100];
            _words = new List<string> { "apple", "banana", "cherry", "date" };
        }

        [Test]
        public void Test_WordList_Length_Requirement()
        {
            // ARRANGE
            PasswordOptions options = new PasswordOptionsBuilder()
                .WithDictionary(_words)
                .WithLength(20, 40)
                .Build();

            // ACT
            var (len, resultBuf) = _passwordGenerator.GeneratePassword(options, _buf);
            string generatedPassword = new string(resultBuf, 0, len);

            // ASSERT
            Assert.That(len, Is.GreaterThanOrEqualTo(20));
        }

        [Test]
        public void Test_WordList_With_Replacements()
        {
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
                var (len, resultBuf) = _passwordGenerator.GeneratePassword(options, _buf);
                string generatedPassword = new string(resultBuf, 0, len);

                if (generatedPassword.Contains('@') || generatedPassword.Contains('3'))
                {
                    replacementFound = true;
                    System.Console.WriteLine($"LOG: Replacement detected in run {i + 1}: {generatedPassword}");
                    break;
                }
            }

            // ASSERT
            Assert.That(replacementFound, Is.True, "No replacements were made in 20 runs.");
        }
    }
}

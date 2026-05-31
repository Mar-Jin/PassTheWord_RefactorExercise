using NUnit.Framework;
using PassTheWord;
using System;
using System.Linq;

namespace TestPassTheWord
{
    [TestFixture]
    public class RandomCharacterStrategyTests
    {
        private PasswordFacade _passwordGenerator;
        private char[] _buf;

        [SetUp]
        public void Setup()
        {
            _passwordGenerator = new PasswordFacade();
            _buf = new char[100];
        }

        [Test]
        public void Test_Random_With_All_Requirements()
        {
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
            var (len, resultBuf) = _passwordGenerator.GeneratePassword(options, _buf);
            string generatedPassword = new string(resultBuf, 0, len);

            // ASSERT
            Assert.That(generatedPassword.Any(char.IsUpper), Is.True);
            Assert.That(generatedPassword.Any(char.IsLower), Is.True);
            Assert.That(generatedPassword.Any(char.IsDigit), Is.True);
            Assert.That(generatedPassword.Any(c => !char.IsLetterOrDigit(c)), Is.True);
        }

        [Test]
        public void Test_Random_Exclude_Similar()
        {
            // ARRANGE
            PasswordOptions options = new PasswordOptionsBuilder()
                .WithLength(50, 50)
                .AllowUppercase()
                .AllowLowercase()
                .AllowDigits()
                .ExcludeSimilarCharacters()
                .Build();

            // ACT
            var (len, resultBuf) = _passwordGenerator.GeneratePassword(options, _buf);
            string generatedPassword = new string(resultBuf, 0, len);

            // ASSERT
            Assert.That(generatedPassword, Does.Not.Contain('I'));
            Assert.That(generatedPassword, Does.Not.Contain('O'));
            Assert.That(generatedPassword, Does.Not.Contain('l'));
            Assert.That(generatedPassword, Does.Not.Contain('0'));
            Assert.That(generatedPassword, Does.Not.Contain('1'));
        }
    }
}

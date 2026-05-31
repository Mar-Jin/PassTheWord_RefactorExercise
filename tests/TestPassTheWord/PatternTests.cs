using NUnit.Framework;
using PassTheWord;
using PassTheWord.Requirements;
using PassTheWord.Strategies;
using System.Collections.Generic;
using System.Linq;

namespace TestPassTheWord;

[TestFixture]
public class PatternTests
{
    [Test]
    public void Composite_RequirementCollection_ShouldVerifyAllUnderlyingRequirements()
    {
        // ARRANGE
        var collection = new RequirementCollection();
        collection.Add(new MinLengthRequirement(8));
        collection.Add(new DigitRequirement());

        // ACT & ASSERT
        Assert.That(collection.IsSatisfiedBy("abc"), Is.False, "Should fail: too short");
        Assert.That(collection.IsSatisfiedBy("abcdefgh"), Is.False, "Should fail: no digits");
        Assert.That(collection.IsSatisfiedBy("abcdefg1"), Is.True, "Should pass: both met");
    }

    [Test]
    public void Visitor_RequirementCharacterVisitor_ShouldCollectCorrectState()
    {
        // ARRANGE
        var collection = new RequirementCollection();
        collection.Add(new UppercaseRequirement());
        collection.Add(new DigitRequirement());
        collection.Add(new MinLengthRequirement(12));

        var visitor = new RequirementCharacterVisitor();

        // ACT
        collection.Accept(visitor);

        // ASSERT
        Assert.That(visitor.NeedsUpper, Is.True);
        Assert.That(visitor.NeedsDigit, Is.True);
        Assert.That(visitor.NeedsSymbol, Is.False);
        Assert.That(visitor.MinLength, Is.EqualTo(12));
    }

    [Test]
    public void Factory_Singleton_ShouldCreateCorrectStrategyTypes()
    {
        // ARRANGE
        var factory = PasswordStrategyFactory.Instance;

        var randomOptions = new PasswordOptionsBuilder().Build();
        var wordListOptions = new PasswordOptionsBuilder().WithDictionary(new List<string> { "test" }).Build();
        var interactiveOptions = new PasswordOptionsBuilder().AsInteractive().Build();

        // ACT
        var randomStrategy = factory.Create(randomOptions);
        var wordListStrategy = factory.Create(wordListOptions);
        var interactiveStrategy = factory.Create(interactiveOptions);

        // ASSERT
        Assert.That(PasswordStrategyFactory.Instance, Is.SameAs(factory), "Should be a Singleton");
        Assert.That(randomStrategy, Is.InstanceOf<RandomCharacterStrategy>());
        Assert.That(wordListStrategy, Is.InstanceOf<WordListStrategy>());
        Assert.That(interactiveStrategy, Is.InstanceOf<InteractivePassphraseStrategy>());
    }

    [Test]
    public void Builder_ShouldCorrectlyPopulateRequirementsCollection()
    {
        // ARRANGE
        var builder = new PasswordOptionsBuilder()
            .RequireUppercase()
            .RequireDigit()
            .WithLength(15, 25);

        // ACT
        var options = builder.Build();
        var visitor = new RequirementCharacterVisitor();
        options.Requirements.Accept(visitor);

        // ASSERT
        Assert.That(visitor.NeedsUpper, Is.True);
        Assert.That(visitor.NeedsDigit, Is.True);
        Assert.That(visitor.MinLength, Is.EqualTo(15));
        Assert.That(visitor.MaxLength, Is.EqualTo(25));
    }

    [Test]
    public void TemplateMethod_BasePasswordStrategy_ShouldExecuteCommonLogic()
    {
        // We verify this by checking if Replacements are applied regardless of the strategy
        // ARRANGE
        var subs = new Dictionary<char, char> { { 'a', '@' } };
        var options = new PasswordOptionsBuilder()
            .WithReplacements(subs)
            .AllowLowercase()
            .WithLength(100, 100) // Large enough to guarantee 'a' is generated
            .Build();
        
        var generator = new PasswordFacade();
        char[] buf = new char[200];

        // ACT
        var (len, resultBuf) = generator.GeneratePassword(options, buf);
        string result = new string(resultBuf, 0, len);

        // ASSERT
        // If the Template Method works, ApplyReplacements (in BaseStrategy) must have been called.
        Assert.That(result, Does.Contain("@"), "Replacements should be applied by the BaseStrategy");
    }
}
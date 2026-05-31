using NUnit.Framework;
using PassTheWord;
using System.Collections.Generic;
using PassTheWord.Requirements;
using PassTheWord.Strategies;

namespace TestPassTheWord;

[TestFixture]
public class PatternTests : TestBase
{
    [Test]
    public void Composite_RequirementCollection_ShouldVerifyAllUnderlyingRequirements()
    {
        Log("Starting Composite_RequirementCollection_ShouldVerifyAllUnderlyingRequirements");
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
        Log("Starting Visitor_RequirementCharacterVisitor_ShouldCollectCorrectState");
        // ARRANGE
        var collection = new RequirementCollection();
        collection.Add(new UppercaseRequirement());
        collection.Add(new DigitRequirement());
        collection.Add(new MinLengthRequirement(12));

        var visitor = new RequirementCharacterVisitor();

        // ACT
        collection.Accept(visitor);

        // ASSERT
        Assert.That(visitor.NeedsUpper, Is.True, "NeedsUpper should be true");
        Assert.That(visitor.NeedsDigit, Is.True, "NeedsDigit should be true");
        Assert.That(visitor.NeedsSymbol, Is.False, "NeedsSymbol should be false");
        Assert.That(visitor.MinLength, Is.EqualTo(12), "MinLength mismatch");
    }

    [Test]
    public void Factory_Singleton_ShouldCreateCorrectStrategyTypes()
    {
        Log("Starting Factory_Singleton_ShouldCreateCorrectStrategyTypes");
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
        Assert.That(randomStrategy, Is.InstanceOf<RandomCharacterStrategy>(), "Should be RandomCharacterStrategy");
        Assert.That(wordListStrategy, Is.InstanceOf<WordListStrategy>(), "Should be WordListStrategy");
        Assert.That(interactiveStrategy, Is.InstanceOf<InteractivePassphraseStrategy>(), "Should be InteractivePassphraseStrategy");
    }

    [Test]
    public void Builder_ShouldCorrectlyPopulateRequirementsCollection()
    {
        Log("Starting Builder_ShouldCorrectlyPopulateRequirementsCollection");
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
        Assert.That(visitor.NeedsUpper, Is.True, "NeedsUpper mismatch");
        Assert.That(visitor.NeedsDigit, Is.True, "NeedsDigit mismatch");
        Assert.That(visitor.MinLength, Is.EqualTo(15), "MinLength mismatch");
        Assert.That(visitor.MaxLength, Is.EqualTo(25), "MaxLength mismatch");
    }

    [Test]
    public void TemplateMethod_BasePasswordStrategy_ShouldExecuteCommonLogic()
    {
        Log("Starting TemplateMethod_BasePasswordStrategy_ShouldExecuteCommonLogic");
        // We verify this by checking if Replacements are applied regardless of the strategy
        // ARRANGE
        var subs = new Dictionary<char, char> { { 'a', '@' } };
        var options = new PasswordOptionsBuilder()
            .WithReplacements(subs)
            .AllowLowercase()
            .WithLength(50, 50) 
            .Build();
        
        bool replacementFound = false;
        for (int i = 0; i < 10; i++) // Run multiple times to avoid statistical bad luck
        {
            var (len, resultBuf) = Facade.GeneratePassword(options, Buffer);
            string result = GetString(resultBuf, len);
            if (result.Contains("@"))
            {
                replacementFound = true;
                Log($"Replacement '@' found in run {i + 1}: {result}");
                break;
            }
        }

        // ASSERT
        Assert.That(replacementFound, Is.True, "Replacements should be applied by the BaseStrategy (did not trigger in 10 runs of length 50)");
    }
}
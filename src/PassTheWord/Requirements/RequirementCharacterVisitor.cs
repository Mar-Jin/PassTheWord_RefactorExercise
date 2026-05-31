namespace PassTheWord.Requirements;

public class RequirementCharacterVisitor: IRequirementVisitor
{
    public bool NeedsUpper { get; private set; }
    public bool NeedsDigit { get; private set; }
    public bool NeedsSymbol { get; private set; }
    public int MinLength { get; private set; }
    public int MaxLength { get; private set; }
    
    public void Visit(UppercaseRequirement requirement) => NeedsUpper = true;
    public void Visit(DigitRequirement requirement) => NeedsDigit = true;
    public void Visit(SymbolRequirement requirement) => NeedsSymbol = true;
    public void Visit(MinLengthRequirement requirement) { }
    public void Visit(MaxLengthRequirement requirement) { }
}
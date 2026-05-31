namespace PassTheWord.Requirements;

public interface IRequirementVisitor
{
    void Visit(UppercaseRequirement requirement);
    void Visit(DigitRequirement requirement);
    void Visit(SymbolRequirement requirement);
    void Visit(MinLengthRequirement requirement);
    void Visit(MaxLengthRequirement requirement);
}
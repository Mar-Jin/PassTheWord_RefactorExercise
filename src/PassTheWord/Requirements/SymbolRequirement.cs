namespace PassTheWord.Requirements;

public class SymbolRequirement: IPasswordRequirement
{
    private const string Symbols = "!@#$%^&*()_+-=,./?~";
    public bool IsSatisfiedBy(string password) => password.Any(c => Symbols.Contains(c));
    public void Accept(IRequirementVisitor visitor) => visitor.Visit(this);
}
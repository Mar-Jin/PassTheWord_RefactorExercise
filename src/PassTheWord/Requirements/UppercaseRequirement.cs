namespace PassTheWord.Requirements;

public class UppercaseRequirement: IPasswordRequirement
{
    public bool IsSatisfiedBy(string password) => password.Any(char.IsUpper);
    public void Accept(IRequirementVisitor visitor) => visitor.Visit(this);
}
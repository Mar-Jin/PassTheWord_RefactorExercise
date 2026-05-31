namespace PassTheWord.Requirements;

public class MinLengthRequirement(int min) : IPasswordRequirement
{
    public bool IsSatisfiedBy(string password) => password.Length >= min;
    public void Accept(IRequirementVisitor visitor) => visitor.Visit(this);
}
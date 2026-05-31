namespace PassTheWord.Requirements;

public class MinLengthRequirement(int min) : IPasswordRequirement
{
    public int Min => min;
    public bool IsSatisfiedBy(string password) => password.Length >= min;
    public void Accept(IRequirementVisitor visitor) => visitor.Visit(this);
}
namespace PassTheWord.Requirements;

public class MaxLengthRequirement(int max): IPasswordRequirement
{
    public bool IsSatisfiedBy(string password) => password.Length <= max;
    public void Accept(IRequirementVisitor visitor) => visitor.Visit(this);

}
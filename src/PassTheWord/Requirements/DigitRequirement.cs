namespace PassTheWord.Requirements;

public class DigitRequirement: IPasswordRequirement
{
    public bool IsSatisfiedBy(string password) => password.Any(char.IsDigit);
    public void Accept(IRequirementVisitor visitor) => visitor.Visit(this);
}
namespace PassTheWord.Requirements;

public interface IPasswordRequirement
{
    bool IsSatisfiedBy(string password);
    void Accept(IRequirementVisitor visitor);
}
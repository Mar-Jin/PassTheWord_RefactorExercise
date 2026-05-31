namespace PassTheWord.Requirements;

public class RequirementCollection: IPasswordRequirement
{
    private readonly List<IPasswordRequirement> _requirements = new();

    public void Add(IPasswordRequirement requirement)
    {
        _requirements.Add(requirement);
    }

    public bool IsSatisfiedBy(string password)
    {
        return _requirements.All(r => r.IsSatisfiedBy(password));
    }

    public void Accept(IRequirementVisitor visitor)
    {
        foreach (var r in _requirements)
        {
            r.Accept(visitor);
        }
    }
}
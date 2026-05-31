namespace PassTheWord.Strategies;

public class CharacterStrategy: IPasswordStrategy
{
    public CharacterStrategy()
    {
        Console.WriteLine("Succesfully created CharacterStrategy");
    }
    
    public (int len, char[] buf) Generate(Span<char> buf, int minLength, int maxLength, Dictionary<char, char> replacements)
    {
        return (1, new char[100]);
    }
}
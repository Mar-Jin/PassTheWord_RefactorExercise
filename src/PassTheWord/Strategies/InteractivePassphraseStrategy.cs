namespace PassTheWord.Strategies;

public class InteractivePassphraseStrategy(PasswordOptions options): BasePasswordStrategy(options)
{

    protected override int FillBuffer(Span<char> buf)
    {
        string? input;
        do
        {
            Console.Write("Enter passphrase: ");
            input = Console.ReadLine();
        } while (input == null || input.Length < Requirements.MinLength || input.Length > buf.Length);

        input.TryCopyTo(buf);
        return input.Length;
    }
}
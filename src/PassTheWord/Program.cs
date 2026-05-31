namespace PassTheWord;

public class Program
{
    static void Main()
    {
        List<string> longWords =
        [
            "kindercarnavalsoptochtvoorbereidingswerkzaamheden",
            "aansprakelijkheidswaardevaststellingsveranderingen",
            "Hottentottensoldatententententoonstellingsbouwterrein",
            "elektriciteitsproductiemaatschappijbuitenlandbelangen",
            "geneesmiddelenvergoedingssysteemorganisatiestructuur"
        ];

        Dictionary<char, char> subs = new() { { 'o', '0' }, { 'i', '1' }, { 's', '$' } };
        char[] buf = new char[100];
        int len = 0;

        PasswordFacade passwordGenerator = new();

        PasswordOptions options = new PasswordOptionsBuilder()
            .WithReplacements(subs)
            .WithDictionary(longWords)
            .WithLength(15, 20)
            .Build();

        (len, buf) = passwordGenerator.GeneratePassword(options, buf);

        Console.WriteLine(buf[..len]);
    }
}
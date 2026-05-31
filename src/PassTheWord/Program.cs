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
        (len, buf) = passwordGenerator.GeneratePassword(
            replacements: subs,
            excludeSimilar: false,
            dictionary: longWords,
            buf: buf,
            interactive: false,
            minlength: 15,
            maxlength: 20
        );
        Console.WriteLine(buf[..len]);
    }
}
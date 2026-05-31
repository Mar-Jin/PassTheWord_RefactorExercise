namespace PassTheWord.Alphabets;

public class CyrillicAlphabet : IAlphabet
{
    public string Name => "Cyrillic";

    public string GetUppercase(bool excludeSimilar) => 
        "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ"; // Russian alphabet

    public string GetLowercase(bool excludeSimilar) => 
        "абвгдеёжзийклмнопрстуфхцчшщъыьэюя";

    public string GetDigits(bool excludeSimilar) => 
        excludeSimilar ? "23456789" : "0123456789";

    public string GetSymbols(bool excludeSimilar) => 
        "!@#$%^&*()_+-=,./?~";
}

using System;
using System.Collections.Generic;
using RND = System.Security.Cryptography.RandomNumberGenerator;

namespace PassTheWord.Strategies;

public abstract class BasePasswordStrategy : IPasswordStrategy
{
    protected readonly PasswordOptions Options;

    protected BasePasswordStrategy(PasswordOptions options)
    {
        Options = options;
    }

    public (int len, char[] buf) Generate()
    {
        Span<char> buf = new char[Options.MaxLength];

        int len = FillBuffer(buf);

        ApplyReplacements(buf, len);

        return (len, buf.ToArray());
    }

    protected abstract int FillBuffer(Span<char> buf);

    private void ApplyReplacements(Span<char> buf, int len)
    {
        for (int i = 0; i < len; i++)
        {
            char currentCharacter = buf[i];
            if (!Options.Replacements.ContainsKey(currentCharacter)) continue;
            
            if (RND.GetInt32(0, 100) < 50)
            {
                buf[i] = Options.Replacements[currentCharacter];
            }
        }
    }
    
    protected string BuildAlphabet()
    {
        string alphabet = "";
        if (Options.Uppercase) alphabet += Options.ExcludeSimilar ? "ABCDEFGHJKLMNPQRSTUVWXYZ" : "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        if (Options.Lowercase) alphabet += Options.ExcludeSimilar ? "abcdefghijkmnopqrstuvwxyz" : "abcdefghijklmnopqrstuvwxyz";
        if (Options.Digits)    alphabet += !Options.ExcludeSimilar ? "0123456789" : "23456789";
        if (Options.Symbols)   alphabet += "!@#$%^&*()_+-=,./?~";
        return alphabet;
    }
}
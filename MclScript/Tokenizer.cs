namespace MclScript;

public class Tokenizer
{
    public static Token[] Run(string src)
    {
        src = src + "$";
        List<Token> tokens = new List<Token>();
        string walk = "";
        bool comment = false;
        for (int i = 0; i < src.Length; i++)
        {
            char c = src[i];
            if (c == '#') comment = true;
            if ("\n$".Contains(c)) comment = false;
            if (" \n\t#".Contains(c) || comment) continue;
            if ("(){}[]+-*/=,;$".Contains(c))
            {
                if (walk.Length > 0)
                {
                    string type = "i";
                    if (walk == "if" || walk == "while") type = "c";
                    if ("0123456789".Contains(walk[0])) type = "n";
                    tokens.Add(new Token(type, walk));
                }
                tokens.Add(new Token(c.ToString()));
                walk = "";
                continue;
            }
            walk += c;
        }

        return tokens.ToArray();
    }
}
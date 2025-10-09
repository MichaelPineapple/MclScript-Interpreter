using MclScript;

namespace MclScript;

public class MclScript
{
    public static void Main(string[] args)
    {
        string src = File.ReadAllText(args[0]);
        Token[] tokens = Tokenize(src);
        Console.WriteLine(TokensToStr(tokens)+"\n");
        try
        {
            Evaluator.Evaluate(tokens);
            Console.WriteLine("\nACCEPT");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    private static Token[] Tokenize(string src)
    {
        src = src + "$";
        List<Token> tokens = new List<Token>();
        string walk = "";
        for (int i = 0; i < src.Length; i++)
        {
            char c = src[i];
            if (" \n\t".Contains(c)) continue;
            if ("(){}[]+-*/=,;$".Contains(c))
            {
                if (walk.Length > 0)
                {
                    string type = "i";
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
    
    public static string TokensToStr(Token[] array)
    {
        string str = "";
        foreach (Token t in array) str += (t.Type + " ");
        return str.Trim();
    }
}
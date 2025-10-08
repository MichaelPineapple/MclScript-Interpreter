namespace MclScript;

public class Program2
{
    public static void Main(string[] args)
    {
        string[] src = File.ReadAllLines(args[0]);
        string[] tokens = Tokenize(src[0]);
        int a = Parse(tokens);
        Console.WriteLine("=" + a);
    }

    private static string[] Tokenize(string src)
    {
        List<string> tokens = new List<string>();
        src = src.Replace("\t", "").Trim();
        string walk = "";
        for (int i = 0; i < src.Length; i++)
        {
            char c = src[i];
            if ("()+-* ".Contains(c))
            {
                if (walk.Length > 0) tokens.Add(walk);
                if (c != ' ') tokens.Add(c.ToString());
                walk = "";
            }

            walk += c;
        }
        return tokens.ToArray();
    }

    private static int Parse(string[] tokens)
    {
        if (tokens.Length == 1) return int.Parse(tokens[0]);
        
        int d = 0;
        List<string> lexp = new List<string>();
        for (int i = 0; i < tokens.Length; i++)
        {
            string t = tokens[i];
            if (t == "(")
            {
                if (d == 0) t = "";
                d++;
            }
            else if (t == ")")
            {
                if (d == 1) t = "";
                d--;
            }
            else if (i > 0 && d == 0) return HandleOperation(t, Parse(lexp.ToArray()), Parse(tokens[(i + 1)..]));
            if (t.Length > 0) lexp.Add(t);
        }

        throw new Exception("Error");
    }

    private static int HandleOperation(string op, int l, int r)
    {
        switch (op)
        {
            case "+":
                return l + r;
                break;
            case "-":
                return l - r;
                break;
            case "*":
                return l * r;
                break;
        }
        throw new Exception("Unknown Operator: " + op);
    }
}
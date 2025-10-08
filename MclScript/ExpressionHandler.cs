namespace MclScript;

public class ExpressionHandler
{
    public static void Main(string[] args)
    {
        string src = File.ReadAllText(args[0]);
        src = src.Replace(" ", "").Replace("\t", "").Replace("\n", "");
        string[] expressions = src.Split(";");
        expressions = expressions[..(expressions.Length - 1)];
        Context context = new Context();
        context.varMap["x"] = 0;
        for (int i = 0; i < expressions.Length; i++) EvaluateExpression(expressions[i], context);
        Console.WriteLine("****\nx = " + context.varMap["x"]);
    }

    private static int EvaluateExpression(string expStr, Context context)
    {
        return Evaluate(Tokenize(expStr), context);
    }

    private static string[] Tokenize(string exp)
    {
        List<string> tokens = new List<string>();
        string walk = "";
        for (int i = 0; i < exp.Length; i++)
        {
            char c = exp[i];
            if ("()+-*=".Contains(c))
            {
                if (walk.Length > 0) tokens.Add(walk);
                tokens.Add(c.ToString());
                walk = "";
            }
            else walk += c;
        }

        if (walk.Length > 0) tokens.Add(walk);
        return tokens.ToArray();
    }

    private static int Evaluate(string[] tokens, Context context)
    {
        if (tokens.Length == 0) throw new Exception("Empty Expression!");
        
        if (tokens.Length == 1)
        {
            string token0 = tokens[0];
            int parseVal;
            if (int.TryParse(token0, out parseVal)) return parseVal;
            else if (context.varMap.ContainsKey(token0)) return context.varMap[token0];
            else throw new Exception("Invalid expression term: " + token0);
        }

        if (tokens[1] == "=")
        {
            string token0 = tokens[0];
            context.varMap[token0] = Evaluate(tokens[2..], context);
            return 0;
        }

        int d = 0;
        List<string> rexp = new List<string>();
        int len = tokens.Length - 1;
        for (int i = len; i >= 0; i--)
        {
            string t = tokens[i];
            if (t == ")") d++;
            else if (t == "(") d--;
            else if (i < len && d == 0)
            {
                rexp.Reverse();
                int l = Evaluate(tokens[..i], context);
                int r = Evaluate(rexp.ToArray(), context);
                return HandleOperation(t, l, r);
            }

            rexp.Add(t);
        }

        if (d > 0) throw new Exception("Invalid Parens!");
        int lastTokenIndex = tokens.Length - 1;
        if (tokens[0] == "(" && tokens[lastTokenIndex] == ")") return Evaluate(tokens[1..lastTokenIndex], context);
        throw new Exception("Invalid Expression!");
    }

    private static string ArrayToStr(string[] array)
    {
        string str = "";
        foreach (string x in array) str += (x + " ");
        return str.Trim();
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
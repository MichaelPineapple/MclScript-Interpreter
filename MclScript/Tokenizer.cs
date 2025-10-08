namespace MclScript;

public class Tokenizer
{
    public static void Tokenize(string src, Context context)
    {
        src = src.Replace(" ", "").Replace("\t", "").Replace("\n", "");
        TokenizeFunctions(src, context);
    }

    private static void TokenizeFunctions(string src, Context context)
    {
        string walk = "";
        string name = "";
        string parameters = "";
        bool body = false;
        for (int i = 0; i < src.Length; i++)
        {
            char c = src[i];
            if (!body)
            {
                if (c == '(') { name = walk; walk = ""; c = '\0'; }
                if (c == ')') { parameters = walk; walk = ""; c = '\0'; }
            }
            if (c == '{') { body = true; c = '\0'; }
            if (c == '}')
            {
                AcceptNewFunction(name, parameters, walk, context);
                body = false; walk = ""; c = '\0';
            }
            if (c != '\0') walk += c;
        }
    }

    private static void AcceptNewFunction(string name, string parameters, string body, Context context)
    {
        string[] expressions = body.Split(";");
        string[] splitParameters = parameters.Split(',');
        List<string> cleanParams = new List<string>();
        foreach (string p in splitParameters) { if (p.Length > 0) cleanParams.Add(p); }
        splitParameters = splitParameters[..(splitParameters.Length - 1)];
        if (expressions.Length == 0 && body.Length > 0) throw new Exception("Missing Semicolon!");
        List<string[]> tokenizedExpressions = new List<string[]>();
        for (int i = 0; i < expressions.Length; i++) tokenizedExpressions.Add(Tokenize(expressions[i]));
        context.funcMap[name] = (cleanParams.ToArray(), tokenizedExpressions.ToArray());
    }
    
    private static string[] Tokenize(string exp)
    {
        List<string> tokens = new List<string>();
        string walk = "";
        for (int i = 0; i < exp.Length; i++)
        {
            char c = exp[i];
            if ("()+-*=,".Contains(c))
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
}
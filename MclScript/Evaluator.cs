namespace MclScript;

public class Evaluator
{
    private const string RETURN_KEY = "return";
    private const string PRINT_KEY = "print";
    
    public static void Evaluate(Context context)
    { 
        EvaluateFunction("main", new int[] { }, context);
    }
    
    private static int EvaluateFunction(string name, int[] paramValues, Context context)
    {
        (string[], string[][]) func = context.funcMap[name];
        string[] parameters = func.Item1;
        string[][] expressions = func.Item2;
        Context subContext = new Context(context);

        for (int i = 0; i < parameters.Length; i++)
        {
            if (i > paramValues.Length - 1) throw new Exception("Parameter Mismatch!");
            subContext.varMap[parameters[i]] = paramValues[i];
        }
        
        for (int i = 0; i < expressions.Length; i++)
        {
            EvaluateExpression(expressions[i], subContext);
            if (subContext.varMap.ContainsKey(RETURN_KEY)) return subContext.varMap[RETURN_KEY];
        }
        return 0;
    }

    private static int EvaluateExpression(string[] tokens, Context context)
    {
        if (tokens.Length == 0) return -1;
        
        if (tokens.Length == 1)
        {
            string token0 = tokens[0];
            int parseVal;
            if (int.TryParse(token0, out parseVal)) return parseVal;
            else if (context.varMap.ContainsKey(token0)) return context.varMap[token0];
            else throw new Exception("Invalid Expression Term: " + token0);
        }

        if (tokens[1] == "=")
        {
            string token0 = tokens[0];
            context.varMap[token0] = EvaluateExpression(tokens[2..], context);
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
                int l = EvaluateExpression(tokens[..i], context);
                int r = EvaluateExpression(rexp.ToArray(), context);
                int tmp;
                if (HandleBuiltInFunctions(t, new int[] { r }, out tmp, context)) return tmp;
                if (context.parent.funcMap.ContainsKey(t)) return EvaluateFunction(t, new int[] { r }, context.parent);
                return HandleOperation(t, l, r);
            }

            rexp.Add(t);
        }

        if (d > 0) throw new Exception("Invalid Parens!");
        int lastTokenIndex = tokens.Length - 1;
        if (tokens[0] == "(" && tokens[lastTokenIndex] == ")") return EvaluateExpression(tokens[1..lastTokenIndex], context);
        throw new Exception("Invalid Expression!");
    }

    private static bool HandleBuiltInFunctions(string name, int[] paramValues, out int result, Context context)
    {
        result = 0;
        switch (name)
        {
            case RETURN_KEY:
                context.varMap[RETURN_KEY] = paramValues[0];
                break;
            case PRINT_KEY:
                Console.WriteLine(paramValues[0]);
                break;
            default:
                return false;
        }
        return true;
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
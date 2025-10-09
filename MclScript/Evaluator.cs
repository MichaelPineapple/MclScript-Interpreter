namespace MclScript;

public class Evaluator
{
    private static Dictionary<string, int> Variables = new Dictionary<string, int>();
    private static Token[] Tokens;
    private static int Index = 0;
    
    public static void Evaluate(Token[] tokens)
    {
        Tokens = tokens;
        Index = 0;
        Global();
    }

    private static bool Peek(string type)
    {
        return Tokens[Index].Type == type;
    }
    
    private static string Get()
    {
        return Tokens[Index].Type;
    }

    private static void Accept(string type)
    {
        string tt = Tokens[Index].Type;
        if (tt != type)
        {
            Console.WriteLine("\n");
            throw new Exception("Unexpected Token: '" + type + "' -> '" + tt + "'");
        }
        Console.Write(tt + " ");
        Index++;
    }
    
    private static void Global()
    {
        FunctionDefinition();
        if (Peek("$")) Accept("$");
        else Global();
    }

    private static void FunctionDefinition()
    {
        Accept("i");
        Accept("(");
        FunctionSignature();
        Accept(")");
        Accept("{");
        Local();
        Accept("}");
    }

    private static void FunctionSignature()
    {
        // Params
    }

    private static void Local()
    {
        if (Peek("i")) Assignment();
        if (!Peek("}")) Local();
    }

    private static void Assignment()
    {
        Accept("i");
        Accept("=");
        Expression();
        Accept(";");
    }
    
    private static void Expression()
    {
        if (Peek("n")) Accept("n");
        else if (Peek("i")) Accept("i");
        else if (Peek("("))
        {
            Accept("(");
            Expression();
            Accept(")");
        }
        string op = Get();
        if (!"+-*/".Contains(op)) return;
        Accept(op);
        Expression();
    }
}
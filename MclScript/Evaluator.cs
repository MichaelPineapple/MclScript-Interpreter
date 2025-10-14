namespace MclScript;

public class Evaluator
{
    private const bool DEBUG = false;
    
    private static Dictionary<string, int> FunctionPointers = new Dictionary<string, int>();
    private static List<Instruction> InstructionsList = new List<Instruction>();
    private static Token[] Tokens;
    private static int Index = 0;
    
    public static Instruction[] Evaluate(Token[] tokens)
    {
        Tokens = tokens;
        Index = 0;
        Global();
        return InstructionsList.ToArray();
    }

    private static void Debug(string str)
    {
        if (DEBUG)
        {
            Console.WriteLine(str);
            Thread.Sleep(1000);
        }
    }
    
    private static int Asm(OpCode op, int x, int y = 0, bool addr = false)
    {
        InstructionsList.Add(new Instruction(op, x, y, addr));
        return InstructionsList.Count - 1;
    }

    private static bool Peek(string type)
    {
        return Tokens[Index].Type == type;
    }
    
    private static string Get()
    {
        return Tokens[Index].Type;
    }

    private static string Accept(string type)
    {
        Token t = Tokens[Index];
        string tt = t.Type;
        if (tt != type) throw new Exception("Expected '" + type + "' But Got '" + tt + "'");
        Debug(tt + " ");
        Index++;
        return t.Value;
    }
    
    private static void Global()
    {
        FunctionDefinition();
        if (Peek("$")) Accept("$");
        else Global();
    }

    private static void FunctionDefinition()
    {
        if (!Peek("i")) return;
        string lbl = Accept("i");
        FunctionPointers[lbl] = Asm(OpCode.NOP, 0);
        Accept("(");
        if (Peek("i")) ParametersDefinition();
        Accept(")");
        Block();
    }

    private static void ParametersDefinition()
    {
        Accept("i");
        if (!Peek(",")) return;
        Accept(",");
        ParametersDefinition();
    }

    private static void Local()
    {
        if (Peek("i")) Statement();
        else if (Peek("c")) Conditional();
        else return;
        Local();
    }
    
    private static void Statement()
    {
        Accept("i");
        if (Peek("="))
        {
            Accept("=");
            Expression();
        }
        else ParametersBlock();
        Accept(";");
    }

    private static void Conditional()
    {
        Accept("c");
        ExpressionBlock();
        Block();
    }
    
    private static void Expression()
    {
        if (Peek("n")) Accept("n");
        else if (Peek("i"))
        {
            Accept("i");
            if (Peek("(")) ParametersBlock();
        }
        else if (Peek("(")) ExpressionBlock();
        string op = Get();
        if (!"+-*/".Contains(op)) return;
        Accept(op);
        Expression();
    }

    private static void Parameters()
    {
        Expression();
        if (Peek(","))
        {
            Accept(",");
            Parameters();
        }
    }
    
    private static void Block()
    {
        Accept("{");
        Local();
        Accept("}");
    }

    private static void ExpressionBlock()
    {
        Accept("(");
        Expression();
        Accept(")");
    }

    private static void ParametersBlock()
    {
        Accept("(");
        Parameters();
        Accept(")");
    }
}
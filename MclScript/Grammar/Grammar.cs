namespace MclScript;

public abstract class Grammar
{
    protected Token[] Tokens;
    protected int Index = 0;
    
    protected bool Peek(string type)
    {
        return Tokens[Index].Type == type;
    }

    protected string Accept(string type)
    {
        Token t = Tokens[Index];
        string tt = t.Type;
        if (tt != type) Error("Expected '" + type + "' But Got '" + tt + "'");
        Debug(tt + " ");
        Index++;
        return t.Value;
    }
    
    private const bool DEBUG = false;
    private static void Debug(string str)
    {
        if (DEBUG)
        {
            Console.Write(str + " ");
            Thread.Sleep(500);
        }
    }

    protected void Error(string msg)
    {
        throw new Exception(msg);
    }
}
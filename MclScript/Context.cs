namespace MclScript;

public class Context
{
    public Context parent;
    public Dictionary<string, int> varMap = new Dictionary<string, int>();
    public Dictionary<string, (string[], string[][])> funcMap = new Dictionary<string, (string[], string[][])>();
    
    public Context(Context parentContext)
    {
        parent = parentContext;
    }
}
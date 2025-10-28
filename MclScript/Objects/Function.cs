namespace MclScript;

public struct Function
{
    public string[] Parameters { get; private set; }
    public Token[] Tokens { get; private set; }

    public Function(string[] parameters, Token[] tokens)
    {
        Parameters = parameters;
        Tokens = tokens;
    }
}
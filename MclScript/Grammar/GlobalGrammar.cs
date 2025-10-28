namespace MclScript;

public class GlobalGrammar : Grammar
{
    private Dictionary<string, Function> FunctionsMap = new Dictionary<string, Function>();
    
    public Dictionary<string, Function> Run(Token[] tokens)
    {
        Tokens = tokens;
        Global();
        return FunctionsMap;
    }
    
    private void Global()
    {
        FunctionDefinition();
        if (Peek("$")) Accept("$");
        else Global();
    }
    
    private void FunctionDefinition()
    {
        if (!Peek("i")) return;
        string i = Accept("i");
        Accept("(");
        List<string> parameters = new List<string>();
        if (Peek("i")) ParametersDefinition(parameters);
        Accept(")");
        Accept("{");
        Token[] tokens = GetFunctionTokens();
        Accept("}");
        FunctionsMap[i] = new Function(parameters.ToArray(), tokens);
    }

    private Token[] GetFunctionTokens()
    {
        int d = 1;
        List<Token> tokens = new List<Token>();
        while (d > 0)
        {
            Token t = Tokens[Index];
            string tt = t.Type;
            if (tt == "{") d++;
            if (tt == "}") d--;
            if (d > 0) tokens.Add(t); 
            Index++;
        }

        Index--;
        tokens.Add(new Token("$"));
        return tokens.ToArray();
    }
    
    private void ParametersDefinition(List<string> parameters)
    {
        string i = Accept("i");
        parameters.Add(i);
        if (!Peek(",")) return;
        Accept(",");
        ParametersDefinition(parameters);
    }
}
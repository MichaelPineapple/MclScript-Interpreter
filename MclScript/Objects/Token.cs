namespace MclScript;

public struct Token
{
    public string Type;
    public string Value;

    public Token(string type, string? value = null)
    {
        Type = type;
        Value = type;
        if (value != null) Value = value;
    }
}
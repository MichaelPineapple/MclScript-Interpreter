namespace MclScript;

public class MclScriptMain
{
    public static void Main(string[] args)
    {
        string src = File.ReadAllText(args[0]);
        Context context = new Context(null);
        
        try
        {
            Tokenizer.Tokenize(src, context);
            Evaluator.Evaluate(context);
        }
        catch (Exception e) { Console.WriteLine("ERROR - " + e.Message); }
    }

    public static string ArrayToStr(string[] array)
    {
        string str = "";
        foreach (string x in array) str += (x + " ");
        return str.Trim();
    }
}
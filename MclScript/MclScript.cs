using MclScript;

namespace MclScript;

public class MclScript
{
    public static int Main(string[] args)
    {
        string src = File.ReadAllText(args[0]);
        Token[] tokens = Tokenizer.Run(src);
        try
        {
            GlobalGrammar global = new GlobalGrammar();
            Dictionary<string, Function> funcMap = global.Run(tokens);
            LocalGrammar local = new LocalGrammar();
            return local.Run(funcMap, "main", new int[] {});
        }
        catch (Exception e)
        {
            Console.WriteLine("\n*** Interpretation Error ***");
            Console.WriteLine(e.Message);
            return -1;
        }
    }
}
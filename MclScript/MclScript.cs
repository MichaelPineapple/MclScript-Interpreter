using MclScript;

namespace MclScript;

public class MclScript
{
    public static int Main(string[] args)
    {
        string src = File.ReadAllText(args[0]);
        return Interpret(src, args[1..]);
    }

    public static int Interpret(string src, string[] args)
    {
        Token[] tokens = Tokenizer.Run(src);
        try
        {
            GlobalGrammar global = new GlobalGrammar();
            Dictionary<string, Function> funcMap = global.Run(tokens);
            LocalGrammar local = new LocalGrammar();
            List<int> argsList = new List<int>();
            for (int i = 0; i < args.Length; i++) argsList.Add(int.Parse(args[i]));
            return local.Run(funcMap, "main", argsList.ToArray());
        }
        catch (Exception e)
        {
            Console.WriteLine("\n*** Interpretation Error ***");
            Console.WriteLine(e.Message);
            return -1;
        }
    }
}
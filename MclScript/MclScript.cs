using MclScript;

namespace MclScript;

public class MclScript
{
    public static void Main(string[] args)
    {
        string src = File.ReadAllText(args[0]);
        Token[] tokens = Tokenizer.Run(src);
        Console.WriteLine(TokensToStr(tokens));
        Instruction[] instructions = new Instruction[0];
        try
        {
            instructions = Evaluator.Evaluate(tokens);
            Console.WriteLine("*** ACCEPT ***");
        }
        catch (Exception e)
        {
            Console.WriteLine("*** REJECT ***");
            Console.WriteLine(e.Message);
            return;
        }
        
        Assembler.Run(instructions);
    }

    
    
    public static string TokensToStr(Token[] array)
    {
        string str = "";
        foreach (Token t in array) str += (t.Type + " ");
        return str.Trim();
    }
}
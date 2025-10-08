class Program
{
    private static readonly Dictionary<string, int> cmdMap = new Dictionary<string, int>()
    {
        { "print", 0 },
        { "var", 1 }
    };

    private static readonly Dictionary<string, int> opMap = new Dictionary<string, int>()
    {
        { "+", 0 },
    };

    private static Dictionary<string, (int, string[])> funcMap = new Dictionary<string, (int, string[])>();

    private static string[] src;
    
    // public static void Main(string[] args)
    // {
    //     src = File.ReadAllLines(args[0]);
    //     DetectFunctions();
    //     InterpretFunction(funcMap["main"], new int[] { 0 });
    // }

    private static void DetectFunctions()
    {
        for (int i = 0; i < src.Length; i++)
        {
            string line = src[i].Replace(" ", "");
            if (line.StartsWith("$"))
            {
                string[] signature = ParseFunctionSignature(line[1..]);
                funcMap[signature[0]] = (i + 1, signature[1..]);
            }
        }
    }

    private static string[] ParseFunctionSignature(string str)
    {
        int paramStart = str.IndexOf('(');
        int paramEnd = str.IndexOf(')');
        string name = str[..paramStart];
        string param = str[(paramStart + 1)..paramEnd];
        return (name + "," + param).Split(',');
    }
    
    private static int InterpretFunction((int, string[]) function, int[] parameterValues)
    {
        int index = function.Item1;
        string[] parameterNames = function.Item2;
        
        Dictionary<string, int> varMap = new Dictionary<string, int>();

        try { for (int i = 0; i < parameterNames.Length; i++) varMap[parameterNames[i]] = parameterValues[i]; }
        catch (Exception e) { throw new Exception("Parameter Mismatch!"); }
                        
        if (src[index] != "{") throw new Exception("Expected '{' !");
        index += 1;
        int returnVal = 0;
        while (index < src.Length)
        {
            string[] args = ParseLine(src[index]);
            foreach (string s in args) Console.Write(s + " ");
            Console.WriteLine("");

            
            string arg0 = args[0];
            if (arg0 == "return") returnVal = InterpretExpression(varMap, args[1..]);
            else if (arg0 == "}") return returnVal;
            else if (arg0 == "print") Console.WriteLine(InterpretExpression(varMap, args[1..]));
            else if (args[1] == "=") varMap[arg0] = InterpretExpression(varMap, args[2..]);
            else throw new Exception("Invalid Syntax! Line:" + index);
            index++;
        }

        throw new Exception("Expected '}' !");
    }

    private static int[] InterpretParameters(Dictionary<string, int> varMap, string[] parameters)
    {
        int[] output = new int[parameters.Length];
        for (int i = 0; i < parameters.Length; i++)
        {
            output[i] = InterpretExpression(varMap, new string[] { parameters[i] });
        }
        return output;
    }

    private static int InterpretSymbol(Dictionary<string, int> varMap, string symbol)
    {
        int val;
        if (int.TryParse(symbol, out val)) return val;
        if (varMap.ContainsKey(symbol)) return varMap[symbol];
        if (funcMap.ContainsKey(symbol.Split('(')[0]))
        {
            string[] signature = ParseFunctionSignature(symbol);
            int[] paramVals = InterpretParameters(varMap, signature[1..]);
            return InterpretFunction(funcMap[signature[0]], paramVals);
        }
        if (symbol == "input") return int.Parse(Console.ReadLine());
        throw new Exception("Undeclared Variable! " + symbol);
    }

    private static string[] ParseLine(string line)
    {
        line = line.Replace("\t", " ").Trim();
        List<string> symbols = new List<string>();
        string walk = "";
        for (int i = 0; i < line.Length; i++)
        {
            char c = line[i];
            if ("()=+ ".Contains(c))
            {
                symbols.Add(walk);
                if (c != ' ') symbols.Add(c.ToString());
                walk = "";
            }
            else walk += c;
        }
        symbols.Add(walk);
        return symbols.ToArray();
    }
    
    private static int InterpretExpression(Dictionary<string, int> varMap, string[] symbols)
    {
        //Console.Write("\n***\n");
        //foreach (string s in symbols) Console.Write(s + " ");
        string sym0 = symbols[0];
        if (symbols.Length == 1) return InterpretSymbol(varMap, sym0);

        string sym1 = symbols[1];
        int op = opMap[sym1];
        int left = InterpretExpression(varMap, new string[]{ sym0 });
        int right = InterpretExpression(varMap, symbols[2..]);
        switch (op)
        {
            case 0:
                return left + right;
                break;
            default:
                throw new Exception("Unknown Operation! " + sym1);
        }
    }
}
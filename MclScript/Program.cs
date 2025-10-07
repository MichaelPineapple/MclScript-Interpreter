class Program
{
    private static readonly Dictionary<string, int> cmdMap = new Dictionary<string, int>()
    {
        { "print", 0 }
    };

    private static readonly Dictionary<string, int> opMap = new Dictionary<string, int>()
    {
        { "=", 0 },
        { "+", 1 }
    };

    private static Dictionary<string, object> varMap = new Dictionary<string, object>();
    
    public static void Main(string[] args)
    {
        string[] src = File.ReadAllLines(args[0]);
        executeSrc(src);
    }

    private static void executeSrc(string[] src)
    {
        for (int i = 0; i < src.Length; i++)
        {
            string line = src[i];
            executeLine(line);
        }
    }

    private static void executeLine(string line)
    {
        string[] args = line.Split(' ');
        string arg0 = args[0];
        string[] subArgs = args[1..];
        if (cmdMap.ContainsKey(args[0])) executeCmd(cmdMap[arg0], subArgs);
        else varMap[arg0] = InterpretArguments(subArgs);
    }

    private static void executeCmd(int cmd, string[] args)
    {
        switch (cmd)
        {
            case 0:
                Console.Write(interpretArg(args[0]).ToString());
                break;
            case 1:
                varMap[args[0]] = interpretArg(args[1]);
                break;
        }
    }

    private static object InterpretArguments(string[] args)
    {
        object output = null;
        int op = -1;
        for (int i = 0; i < args.Length; i++)
        {
            string arg = args[i];
            if (opMap.ContainsKey(arg)) op = opMap[arg];
            else
            {
                object argInterp = interpretArg(args[i]);
                switch (op)
                {
                    case 0:
                        if (output == null) output = argInterp;
                        else throw new Exception("Err");
                        break;
                        
                    case 1:
                        Type type = output.GetType();
                        if (type == typeof(int)) output = (int)output + (int)argInterp;
                        else if (type == typeof(string)) output = (string)output + (string)argInterp;
                        break;
                    
                    default:
                        throw new Exception("Err");
                }

                op = -1;
            }
        }

        return output;
    }

    private static object interpretArg(string str)
    {
        int val;
        if (int.TryParse(str, out val)) return val;
        if (str.StartsWith('"') && str.EndsWith('"'))
        {
            str = str[1..(str.Length - 1)];
            str = str.Replace("\\n", "\n");
            return str;
        }
        return varMap[str];
    }
}
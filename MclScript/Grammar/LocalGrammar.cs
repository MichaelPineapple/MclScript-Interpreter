using System.Diagnostics;

namespace MclScript;

public class LocalGrammar : Grammar
{
    private Dictionary<string, Function> FunctionsMap;
    private Dictionary<string, int> VariablesMap;
    private bool ExecuteMode = true;

    public int Run(Dictionary<string, Function> funcMap, string function, int[] paramVals)
    {
        VariablesMap = new Dictionary<string, int>();
        const string returnVarName = "return";
        VariablesMap[returnVarName] = 0;
        FunctionsMap = funcMap;
        Function func = FunctionsMap[function];
        Tokens = func.Tokens;
        if (func.Parameters.Length != paramVals.Length) Error("Parameters do not match!");
        for (int i = 0; i < func.Parameters.Length; i++) VariablesMap[func.Parameters[i]] = paramVals[i];
        Local();
        return VariablesMap[returnVarName];
    }
    
    private void Local()
    {
        if (Peek("i")) Statement();
        else if (Peek("if") || Peek("while")) Conditional();
        else return;
        Local();
    }
    
    private void Statement()
    {
        string i = Accept("i");
        if (Peek("=")) Assignment(i);
        else FunctionCall(i);
        Accept(";");
    }

    private void Conditional()
    {
        if (Peek("if")) IfElse();
        else if (Peek("while")) While();
    }
    
    private void Block()
    {
        Accept("{");
        Local();
        Accept("}");
    }

    private int ExpressionBlock()
    {
        Accept("(");
        int v = Expression();
        Accept(")");
        return v;
    }
    
    private int Expression()
    {
        int vl = 0;
        if (Peek("n")) vl = int.Parse(Accept("n"));
        else if (Peek("i"))
        {
            string i = Accept("i");
            if (Peek("(")) vl = FunctionCall(i);
            else
            {
                if (!VariablesMap.ContainsKey(i)) Error("Use of undeclared variable '" + i + "'!");
                vl = VariablesMap[i];
            }
        }
        else if (Peek("(")) vl = ExpressionBlock();
        string op = Tokens[Index].Type;
        if (!"+-*/%&|<:>".Contains(op)) return vl;
        Accept(op);
        int vr = Expression();
        return ExecuteOperation(vl, vr, op);
    }

    private int ExecuteOperation(int l, int r, string op)
    {
        if (!ExecuteMode) return 0;
        switch (op)
        {
            case "+":
                return l + r;
            case "-":
                return l - r;
            case "*":
                return l * r;
            case "/":
                return l / r;
            case "%":
                return l % r;
            case "&":
                return (l > 0) && (r > 0) ? 1 : 0;
            case "|":
                return (l > 0) || (r > 0) ? 1 : 0;
            case "<":
                return l < r ? 1 : 0;
            case ">":
                return l > r ? 1 : 0;
            case ":":
                return l == r ? 1 : 0;
            case "!":
                return l != r ? 1 : 0;
            default:
                Error("Unknown Operator: '" + op + "'");
                return 0;
        }
    }
    
    private void IfElse()
    {
        bool prevExeMode = ExecuteMode;
        Accept("if");
        int v0 = ExpressionBlock();
        ExecuteMode = v0 > 0;
        Block();
        ExecuteMode = prevExeMode;
        if (Peek("else"))
        {
            Accept("else");
            int v1 = ExpressionBlock();
            ExecuteMode = !(v0 > 0) && v1 > 0;
            Block();
            ExecuteMode = prevExeMode;
        }
    }

    private void While()
    {
        bool prevExeMode = ExecuteMode;
        Accept("while");
        int index = Index;
        int v2 = ExpressionBlock();
        while (v2 > 0)
        {
            ExecuteMode = true;
            Block();
            Index = index;
            v2 = ExpressionBlock();
        }
        ExecuteMode = false;
        Block();
        ExecuteMode = prevExeMode;
    }

    private void Assignment(string i)
    {
        Accept("=");
        int v = Expression();
        if (ExecuteMode) VariablesMap[i] = v;
    }
    
    private int FunctionCall(string i)
    {
        Accept("(");
        int[] paramVals = Parameters().ToArray();
        Accept(")");
        if (!ExecuteMode) return 0;
        int v = 0;
        if (HandleBuiltInFunctions(i, paramVals, out v)) return v;
        else
        {
            LocalGrammar localGrammar = new LocalGrammar();
            if (!FunctionsMap.ContainsKey(i)) Error("Use of undeclared function '" + i + "'!");
            return localGrammar.Run(FunctionsMap, i, paramVals);
        }
    }
    
    private List<int> Parameters()
    {
        List<int> vals = new List<int>();
        int v = Expression();
        vals.Add(v);
        if (Peek(","))
        {
            Accept(",");
            vals.AddRange(Parameters());
        }
        return vals;
    }

    private bool HandleBuiltInFunctions(string i, int[] v, out int o)
    {
        o = 0;
        switch (i)
        {
            case "print":
                Console.WriteLine(v[0]);
                break;
            case "input":
                o = int.Parse(Console.ReadLine());
                break;
            default:
                return false;
        }
        return true;
    }
}
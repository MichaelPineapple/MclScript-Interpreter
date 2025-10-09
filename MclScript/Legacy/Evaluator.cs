// namespace MclScript;
//
// public class Evaluator
// {
//     private const string RETURN_KEY = "return";
//     private const string PRINT_KEY = "print";
//     private const string INPUT_KEY = "input";
//     
//     public static void Evaluate(Context context)
//     { 
//         EvaluateFunction("main", new int[] { }, context);
//     }
//     
//     private static int EvaluateFunction(string name, int[] paramValues, Context context)
//     {
//         (string[], string[][]) func = context.funcMap[name];
//         string[] parameters = func.Item1;
//         string[][] expressions = func.Item2;
//         Context subContext = new Context(context);
//
//         if (paramValues.Length != parameters.Length) throw new Exception("Parameter Mismatch!");
//         for (int i = 0; i < parameters.Length; i++) subContext.varMap[parameters[i]] = paramValues[i];
//         
//         for (int i = 0; i < expressions.Length; i++)
//         {
//             EvaluateExpression(expressions[i], subContext);
//             if (subContext.varMap.ContainsKey(RETURN_KEY)) return subContext.varMap[RETURN_KEY];
//         }
//         return 0;
//     }
//
//     private static int EvaluateFunctionCall(string name, string[] exp, Context context)
//     {
//         Console.WriteLine("function call: " + name + " -> " + MclScriptMain.ArrayToStr(exp));
//
//         List<int> expResults = new List<int>();
//         List<string> walk = new List<string>();
//         for (int i = 1; i < exp.Length - 1; i++)
//         {
//             string token = exp[i];
//             if (token == ",")
//             {
//                 expResults.Add(EvaluateExpression(walk.ToArray(), context));
//                 walk = new List<string>();
//             }
//             else walk.Add(token);
//         }
//         if (walk.Count > 0) expResults.Add(EvaluateExpression(walk.ToArray(), context));
//         
//         int tmp;
//         int[] paramValues = expResults.ToArray();
//         if (HandleBuiltInFunctions(name, paramValues, out tmp, context)) return tmp;
//         if (context.parent.funcMap.ContainsKey(name)) return EvaluateFunction(name, paramValues, context.parent);
//         throw new Exception("Unknown Function: " + name);
//     }
//
//     private static int EvaluateExpression(string[] tokens, Context context)
//     {
//         Console.WriteLine("tokens: "+ MclScriptMain.ArrayToStr(tokens));
//         if (tokens.Length == 0) return -1;
//         
//         if (tokens.Length == 1)
//         {
//             string token0 = tokens[0];
//             int parseVal;
//             if (int.TryParse(token0, out parseVal)) return parseVal;
//             else if (context.varMap.ContainsKey(token0)) return context.varMap[token0];
//             else throw new Exception("Invalid Expression Term: " + token0);
//         }
//
//         if (tokens[1] == "=")
//         {
//             string token0 = tokens[0];
//             context.varMap[token0] = EvaluateExpression(tokens[2..], context);
//             return 0;
//         }
//         
//         int d = 0;
//         List<string> rexp = new List<string>();
//         int len = tokens.Length - 1;
//         for (int i = len; i >= 0; i--)
//         {
//             string t = tokens[i];
//             if (t == ")") d++;
//             else if (t == "(") d--;
//             else if (i < len && d == 0)
//             {
//                 rexp.Reverse();
//                 string[] rexpArray = rexp.ToArray();
//                 if ("+-*".Contains(t))
//                 {
//                     int l = EvaluateExpression(tokens[..i], context);
//                     int r = EvaluateExpression(rexpArray, context);
//                     return ExecuteOperation(t, l, r);
//                 }
//                 else return EvaluateFunctionCall(t, rexpArray, context);
//             }
//
//             rexp.Add(t);
//         }
//
//         if (d > 0) throw new Exception("Invalid Parens!");
//         int lastTokenIndex = tokens.Length - 1;
//         if (tokens[0] == "(" && tokens[lastTokenIndex] == ")") return EvaluateExpression(tokens[1..lastTokenIndex], context);
//         throw new Exception("Invalid Expression!");
//     }
//
//     private static bool HandleBuiltInFunctions(string name, int[] paramValues, out int result, Context context)
//     {
//         result = 0;
//         switch (name)
//         {
//             case RETURN_KEY:
//                 //if (paramValues.Length != 1) throw new Exception("Parameter Mismatch! " + paramValues.Length);
//                 context.varMap[RETURN_KEY] = paramValues[0];
//                 break;
//             case PRINT_KEY:
//                 //if (paramValues.Length != 1) throw new Exception("Parameter Mismatch! " + paramValues.Length);
//                 Console.WriteLine(paramValues[0]);
//                 break;
//             case INPUT_KEY:
//                 //if (paramValues.Length != 0) throw new Exception("Parameter Mismatch! " + paramValues.Length);
//                 result = int.Parse(Console.ReadLine());
//                 break;
//             default:
//                 return false;
//         }
//         return true;
//     }
//
//     private static int ExecuteOperation(string op, int l, int r)
//     {
//         switch (op)
//         {
//             case "+":
//                 return l + r;
//                 break;
//             case "-":
//                 return l - r;
//                 break;
//             case "*":
//                 return l * r;
//                 break;
//         }
//
//         throw new Exception("Invalid Operation: " + op);
//     }
// }
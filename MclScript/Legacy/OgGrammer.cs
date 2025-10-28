// namespace MclScript;
//
// public class OgGrammer : Grammer
// {
//     // private static Token[] Tokens;
//     // private static int Index = 0;
//     //
//     // public static void Run(Token[] tokens)
//     // {
//     //     Tokens = tokens;
//     //     Index = 0;
//     //     Global();
//     // }
//
//     // private static void Debug(string str)
//     // {
//     //     if (DEBUG)
//     //     {
//     //         Console.WriteLine(str);
//     //         Thread.Sleep(1000);
//     //     }
//     // }
//     //
//     // private static bool Peek(string type)
//     // {
//     //     return Tokens[Index].Type == type;
//     // }
//     //
//     // private static string Get()
//     // {
//     //     return Tokens[Index].Type;
//     // }
//     //
//     // private static string Accept(string type)
//     // {
//     //     Token t = Tokens[Index];
//     //     string tt = t.Type;
//     //     if (tt != type) throw new Exception("Expected '" + type + "' But Got '" + tt + "'");
//     //     Debug(tt + " ");
//     //     Index++;
//     //     return t.Value;
//     // }
//     
//     // private static void Global()
//     // {
//     //     FunctionDefinition();
//     //     if (Peek("$")) Accept("$");
//     //     else Global();
//     // }
//     //
//     // private static void FunctionDefinition()
//     // {
//     //     if (!Peek("i")) return;
//     //     string i = Accept("i");
//     //     Accept("(");
//     //     List<string> parameters = new List<string>();
//     //     if (Peek("i")) ParametersDefinition(parameters);
//     //     Accept(")");
//     //     Accept("{");
//     //     Token[] tokens = GetFunctionTokens();
//     //     Accept("}");
//     //     FunctionsMap[i] = new Function(parameters.ToArray(), tokens);
//     // }
//     //
//     // private static Token[] GetFunctionTokens()
//     // {
//     //     int d = 1;
//     //     List<Token> tokens = new List<Token>();
//     //     while (d > 0)
//     //     {
//     //         Token t = Tokens[Index];
//     //         tokens.Add(t);
//     //         string tt = t.Type;
//     //         if (tt == "{") d++;
//     //         if (tt == "}") d--;
//     //         Index++;
//     //     }
//     //     Index--;
//     //     Token[] array = tokens.ToArray();
//     //     return array[..(array.Length - 1)];
//     // }
//     //
//     //
//     // private static void ParametersDefinition(List<string> parameters)
//     // {
//     //     string i = Accept("i");
//     //     parameters.Add(i);
//     //     if (!Peek(",")) return;
//     //     Accept(",");
//     //     ParametersDefinition(parameters);
//     // }
//
//     private static void Local()
//     {
//         if (Peek("i")) Statement();
//         else if (Peek("if") || Peek("while")) Conditional();
//         else return;
//         Local();
//     }
//     
//     private static void Statement()
//     {
//         Accept("i");
//         if (Peek("="))
//         {
//             Accept("=");
//             Expression();
//         }
//         else ParametersBlock();
//         Accept(";");
//     }
//
//     private static void Conditional()
//     {
//         if (Peek("if"))
//         {
//             Accept("if");
//             ExpressionBlock();
//             Block();
//             if (Peek("else"))
//             {
//                 Accept("else");
//                 ExpressionBlock();
//                 Block();
//             }
//         }
//         else if (Peek("while"))
//         {
//             Accept("while");
//             ExpressionBlock();
//             Block();
//         }
//     }
//     
//     private static void Expression()
//     {
//         if (Peek("n")) Accept("n");
//         else if (Peek("i"))
//         {
//             Accept("i");
//             if (Peek("(")) ParametersBlock();
//         }
//         else if (Peek("(")) ExpressionBlock();
//         string op = Get();
//         if (!"+-*/".Contains(op)) return;
//         Accept(op);
//         Expression();
//     }
//
//     private static void Parameters()
//     {
//         Expression();
//         if (Peek(","))
//         {
//             Accept(",");
//             Parameters();
//         }
//     }
//     
//     private static void Block()
//     {
//         Accept("{");
//         Local();
//         Accept("}");
//     }
//
//     private static void ExpressionBlock()
//     {
//         Accept("(");
//         Expression();
//         Accept(")");
//     }
//
//     private static void ParametersBlock()
//     {
//         Accept("(");
//         Parameters();
//         Accept(")");
//     }
// }
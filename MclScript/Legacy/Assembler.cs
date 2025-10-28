// namespace MclScript;
//
// public class Assembler
// {
//     private static int[] Memory = new int[10000];
//     private static int Index = 0;
//     private static bool flag = false;
//     
//     public static void Run(Instruction[] instructions)
//     {
//         while (Index < instructions.Length)
//         {
//             Instruction instruction = instructions[Index];
//             Console.WriteLine(Index + " " + instruction.ToString());
//             ExecuteInstruction(instruction);
//             Index++;
//         }
//     }
//
//     private static void ExecuteInstruction(Instruction instruction)
//     {
//         int x = instruction.X;
//         int y = instruction.Y;
//         if (instruction.Ytype) y = Memory[y];
//         switch (instruction.Op)
//         {
//             case OpCode.NOP:
//                 break;
//             case OpCode.MOV:
//                 Memory[x] = y;
//                 break;
//             case OpCode.ADD:
//                 Memory[x] = Memory[x] - y;
//                 break;
//             case OpCode.OUT:
//                 Console.WriteLine(Memory[x]);
//                 break;
//             case OpCode.JMP:
//                 if (flag) Index = Memory[x];
//                 break;
//             case OpCode.CEQ:
//                 flag = (Memory[x] == y);
//                 break;
//         }
//     }
// }
//
// public struct Instruction
// {
//     public OpCode Op { get; private set; }
//     public int X { get; private set; }
//     public int Y { get; private set; }
//     public bool Ytype { get; private set; }
//     
//     public Instruction(OpCode op, int x, int y = 0, bool addr = false)
//     {
//         Op = op;
//         X = x;
//         Y = y;
//         Ytype = addr;
//     }
//
//     public override string ToString()
//     {
//         string yTypeStr = "";
//         if (Ytype) yTypeStr = "%";
//         return Op.ToString() + " %" + X + " " + yTypeStr + Y;
//     }
// }
//
// public enum OpCode
// {
//     NOP, // No Operation
//     MOV, // Set x = y
//     ADD, // x = x + y
//     SUB, // x = x - y
//     MUL, // x = x * y
//     DIV, // x = x / y
//     CEQ, // x == y
//     CLT, // x < y
//     CGT, // x > y
//     CGE, // x >= y
//     CLE, // x <= y
//     JMP, // If flag jump to x
//     UJP, // Jumpt to x
//     OUT, // Print x
// }
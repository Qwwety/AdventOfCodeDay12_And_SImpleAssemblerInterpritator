using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SimpleAssemblerInterpreter
{
    class Program
    {
        static string Variable1 = "0";
        static string Variable2 = "0";
        static int InstractionPointer = 0;
        static int Pointer = 0;

        static void Main(string[] args)
        {
            string[] Instraction = new string[]
            {
                "cpy 1 a",
                "cpy 1 b",
                "cpy 26 d",
                "jnz c 2",
                "jnz 1 5",
                "cpy 7 c",
                "inc d",
                "dec c",
                "jnz c -2",
                "cpy a c",
                "inc a",
                "dec b",
                "jnz b -2",
                "cpy c b",
                "dec d",
                "jnz d -6",
                "cpy 19 c",
                "cpy 11 d",
                "inc a",
                "dec d",
                "jnz d -2",
                "dec c",
                "jnz c -5",

            };

            Console.WriteLine(Interpret(Instraction));
            Console.ReadLine();
        }
        static string GetInstraction(string Input)
        {
            string PatternInstractionCpy = @"\bcpy\w*\b";
            string PatternInstractionInc = @"\binc\w*\b";
            string PatternInstractionDec = @"\bdec\w*\b";
            string PatternInstractionJnz = @"\bjnz\w*\b";

            Match Cpy = Regex.Match(Input, PatternInstractionCpy, RegexOptions.IgnoreCase);
            Match Inc = Regex.Match(Input, PatternInstractionInc, RegexOptions.IgnoreCase);
            Match Dec = Regex.Match(Input, PatternInstractionDec, RegexOptions.IgnoreCase);
            Match Jnz = Regex.Match(Input, PatternInstractionJnz, RegexOptions.IgnoreCase);
            if (Cpy.Success)
            {
                return Cpy.Value;
            }
            else if (Inc.Success)
            {
                return Inc.Value;
            }
            else if (Dec.Success)
            {
                return Dec.Value;
            }
            else
            {
                return Jnz.Value;
            }
        }
        static string GetVarible(string Input, Variables variables)
        {
            List<Match> Index = new List<Match>(2);
            string PatternA = @"\ba\b"; // создавать новые переменные по патерну, в цикл, проверять и т.д.
            string PatternB = @"\bb\b";
            string PatternC = @"\bc\b";
            string PatternD = @"\bd\b";
            string patterinInt = @"\-*\d+";
            Match A = Regex.Match(Input, PatternA, RegexOptions.IgnoreCase);
            Match B = Regex.Match(Input, PatternB, RegexOptions.IgnoreCase);
            Match C = Regex.Match(Input, PatternC, RegexOptions.IgnoreCase);
            Match D = Regex.Match(Input, PatternD, RegexOptions.IgnoreCase);
            Match Value = Regex.Match(Input, patterinInt, RegexOptions.IgnoreCase);


            if (A.Success && Variable1 != A.Value)
            {
                Index.Add(A);
            }
            if (B.Success && Variable1 != B.Value)
            {
                Index.Add(B);
            }
            if (C.Success && Variable1 != C.Value)
            {
                Index.Add(C);
            }
            if (D.Success && Variable1 != D.Value)
            {
                Index.Add(D);
            }
            if (Value.Success && Index.Count < 1)
            {
                return Value.Value;
            }



            if (Index.Count > 1)
            {
                if (Index[0].Index < Index[1].Index)
                {
                    return Index[0].Value;
                }
                else
                {
                    return Index[1].Value;
                }
            }
            else if (Index.Count == 1)
            {
                return Index[0].Value;
            }

            return "0";
        }
        static void DoInstraction(string CurrantInsctraction, string Variable1, string Variable2, Variables Variables)
        {
            int Value = 0;
            switch (CurrantInsctraction)
            {
                case ("inc"):
                    Value = Variables.GetVariable(Variable1);
                    Value++;
                    Variables.SetVariable(Variable1, Value);
                    break;
                case ("dec"):
                    Value = Variables.GetVariable(Variable1);
                    Value--;
                    Variables.SetVariable(Variable1, Value);
                    break;
                case ("cpy"):
                    Variables.SetVariable(Variable1, Variables.GetVariable(Variable2));
                    break;
                case ("jnz"):
                    if (Variables.GetVariable(Variable1) != 0)
                    {
                        Pointer = Variables.GetVariable(Variable2);
                    }
                    else
                    {
                        Pointer = 0;
                    }
                    break;
            }
        }
        static void PointerController()
        {
            if (Pointer != 0)
            {
                InstractionPointer += Pointer;
                Pointer = 0;
            }
            else
            {
                InstractionPointer++;
            }
        }

        public static Dictionary<string, int> Interpret(string[] program)
        {
            Variables Variables = new Variables();
            Variables.SetVariable("a", 0);
            Variables.SetVariable("b", 0);
            Variables.SetVariable("c", 0);
            Variables.SetVariable("d", 0);

            while (InstractionPointer < program.Length)
            {
                Console.WriteLine($"InstractionPointer::{InstractionPointer}");
                Variable1 = GetVarible(program[InstractionPointer], Variables);
                Variable2 = GetVarible(program[InstractionPointer], Variables);
                DoInstraction(GetInstraction(program[InstractionPointer]), Variable1, Variable2, Variables);
                PointerController();
                Console.WriteLine($"a::{ Variables.GetVariable("a")}");
                Console.WriteLine($"b::{ Variables.GetVariable("b")}");
                Console.WriteLine($"c::{ Variables.GetVariable("c")}");
                Console.WriteLine($"d::{ Variables.GetVariable("d")}");
                Variable1 = "0";
                Variable2 = "0";
            }
            Console.WriteLine($"Final a::{Variables.GetVariable("a")}");
            Console.WriteLine($"Final b::{Variables.GetVariable("b")}");
            Console.WriteLine($"Final c::{Variables.GetVariable("c")}");
            Console.WriteLine($"Final d::{Variables.GetVariable("d")}");
            return Variables.ReturnDictionary();
        }

    }
    class Variables
    {
        Dictionary<string, int> VariablesDictoinary;
        public Variables()
        {
            VariablesDictoinary = new Dictionary<string, int>();
        }
        public int GetVariable(string Key)
        {
            if (VariablesDictoinary.ContainsKey(Key))
            {
                return VariablesDictoinary[Key];
            }
            else
            {
                return Convert.ToInt32(Key);
            }

        }
        public void SetVariable(string Key, int Value)
        {
            if (VariablesDictoinary.ContainsKey(Key))
            {
                VariablesDictoinary[Key] = Value;
            }
            else
            {
                VariablesDictoinary.Add(Key, Value);
            }
        }
        public Dictionary<string, int> ReturnDictionary()
        {
            return VariablesDictoinary;
        }
    }
}

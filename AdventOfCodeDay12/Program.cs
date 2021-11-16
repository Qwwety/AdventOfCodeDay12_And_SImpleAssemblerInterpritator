using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AdventOfCodeDay12
{
    class Program
    {

        static void Main(string[] args)
        {
            int a = 0;
            int b = 0;
            int c = 0;
            int d = 0;
            int InsctractionPointer;

            int Read(string Varible)
            {
                switch (Varible)
                {
                    case "a":
                        return a;
                    case "b":
                        return b;
                    case "InsctractionPointer":
                        return InsctractionPointer;
                }
                return 777;
            }
            void Write(string Var, int Number)
            { 
                switch (Var)
                {
                    case "a":
                        a = Number;
                        break;
                    case "b":
                        b = Number;
                        break;
                    case "InsctractionPointer":
                         InsctractionPointer= Number;
                        break;
                }

            }
            String[] blat = new string[]
            {
               "cpy 10 a",
                "inc a",
                "inc a",
                "jnz a -1",
                "dec a",
            };

            void ReadInst(string[] Instraction)
            {
                string patterinInt = @"\-*\d+";
                string patternVarible = @"\ba\w*\b";
                int Value;
                for(InsctractionPointer=0; InsctractionPointer < Instraction.Length; InsctractionPointer++)
                {
                    string input = Instraction[InsctractionPointer].ToString();// сделать как в S.A.I.
                    Match Todo = Regex.Match(input, GetInstraction(Instraction, input), RegexOptions.IgnoreCase);// додеалать, ошибка тут 
                    Match Number = Regex.Match(input, patterinInt, RegexOptions.IgnoreCase);
                    Match Var = Regex.Match(input, patternVarible, RegexOptions.IgnoreCase);
                    if (Number.Success)
                    {
                        Value = Convert.ToInt32(Number.Value);
                    }
                    else
                    {
                        Value = 0;
                    }
                    Write(Var.Value, DDt(Todo.Value, Read(Var.Value), Value));
                    Console.WriteLine($"a::{a.ToString()}");
                }

            }
            string GetInstraction(string[] Instraction, string input)
            {
                string patternInstractionCpy = @"\bcpy\w*\b";
                string patternInstractionInc = @"\binc\w*\b";
                string patternInstractionDec = @"\bdec\w*\b";
                string patternInstractionJnz = @"\bjnz\w*\b";

                Match Cpy = Regex.Match(input, patternInstractionCpy, RegexOptions.IgnoreCase);
                Match Inc = Regex.Match(input, patternInstractionInc, RegexOptions.IgnoreCase);
                Match Dec = Regex.Match(input, patternInstractionDec, RegexOptions.IgnoreCase);
                Match Jnz = Regex.Match(input, patternInstractionJnz, RegexOptions.IgnoreCase);
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

            int DDt(string Action, int Varible1, int Varible2)
            {
                switch (Action)
                {
                    case ("inc"):
                        Varible1++;
                        return Varible1;
                    case ("dec"):
                        Varible1--;
                        return Varible1;
                    case ("cpy"):
                        Varible1 = Varible2;
                        return Varible1;
                    case ("jnz"):
                        if (Varible2 < 0)
                        {
                            Varible2--;
                        }
                        if (Varible1 != 0)
                        {
                            InsctractionPointer += Varible2;
                        }
                        break;
                }
                return Varible1;

            }

            ReadInst(blat);
          
            Console.ReadKey();
        }
        

    }
}

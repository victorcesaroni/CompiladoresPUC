using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompiladoresVM
{
    class VMCore
    {
        public class Instruction
        {
            public string opcode;
            public string arg1;
            public string arg2;
            public int numArgs;
            public bool hasComment;
            public string comment;
            public bool isLabel;
            public string label;
            public bool useLabel;

            public Instruction()
            {

            }

            public override string ToString()
            {
                if (numArgs == 0)
                    return opcode.ToString() + " #" + comment;
                if (numArgs == 1)
                    return opcode.ToString() + " " + arg1.ToString() + " #" + comment;
                if (numArgs == 2)
                    return opcode.ToString() + " " + arg1.ToString() + ", " + arg2.ToString() + " #" + comment;

                return "ERROR";
            }

            public Instruction(string text)
            {
                text = text.Replace("\t", " ");
                text = text.Replace("  ", " ");
                string[] s = text.Split(' ');

                int commentIdx = text.IndexOf("#");
                if (commentIdx != -1)
                {
                    comment = text.Substring(commentIdx + 1);
                    hasComment = true;
                }
                else
                {
                    comment = "";
                    hasComment = false;
                }

                if (IsLabel(text, ref label))
                {
                    text = text.Substring(text.IndexOf(' ') + 1);
                    s = text.Split(' ');
                    isLabel = true;
                }
                else
                {
                    label = "";
                    isLabel = false;
                }

                opcode = GetOpcode(s[0]);
                numArgs = NumberOfArguments(opcode);
                useLabel = UseLabel(opcode);

                if (numArgs == 0)
                {
                    arg1 = "";
                    arg2 = "";
                }
                else if (numArgs == 1)
                {
                    arg1 = s[1];
                    arg2 = "";
                }
                else if (numArgs == 2)
                {
                    if (hasComment)
                    {
                        int start = text.IndexOf(' ') + 1;
                        text = text.Substring(start, text.IndexOf(' ', start) - start);
                        s = text.Split(',');
                    }
                    else
                    {
                        text = text.Substring(text.IndexOf(' ') + 1);
                        s = text.Split(',');
                    }

                    arg1 = s[0];
                    arg2 = s[1];
                }
            }

            public string GetOpcode(string m)
            {
                string tmp = m.IndexOf(' ') != -1 ? m.Substring(0, m.IndexOf(' ')) : m;
                tmp = tmp.ToUpper();

                switch (tmp)
                {
                    case "LDC": return  tmp;
                    case "LDV": return  tmp;
                    case "ADD": return  tmp;
                    case "SUB": return  tmp;
                    case "MULT": return tmp;
                    case "DIVI": return tmp;
                    case "INV": return  tmp;
                    case "AND": return  tmp;
                    case "OR": return   tmp;
                    case "NEG": return  tmp;
                    case "CME": return  tmp;
                    case "CMA": return  tmp;
                    case "CEQ": return  tmp;
                    case "CDIF": return tmp;
                    case "CMEQ": return tmp;
                    case "CMAQ": return tmp;
                    case "START": return tmp;
                    case "HLT": return  tmp;
                    case "STR": return  tmp;
                    case "JMP": return  tmp;
                    case "JMPF": return tmp;
                    case "NULL": return tmp;
                    case "RD": return   tmp;
                    case "PRN": return  tmp;
                    case "ALLOC": return tmp;
                    case "DALLOC": return tmp;
                    case "CALL": return tmp;
                    case "RETURN": return tmp;
                };

                throw new Exception("Invalid opcode");
            }

            public bool IsLabel(string m, ref string label)
            {
                switch (m)
                {
                    case "LDC": return false;
                    case "LDV": return false;
                    case "ADD": return false;
                    case "SUB": return false;
                    case "MULT": return false;
                    case "DIVI": return false;
                    case "INV": return false;
                    case "AND": return false;
                    case "OR": return false;
                    case "NEG": return false;
                    case "CME": return false;
                    case "CMA": return false;
                    case "CEQ": return false;
                    case "CDIF": return false;
                    case "CMEQ": return false;
                    case "CMAQ": return false;
                    case "START": return false;
                    case "HLT": return false;
                    case "STR": return false;
                    case "JMP": return false;
                    case "JMPF": return false;
                    case "NULL": return false;
                    case "RD": return false;
                    case "PRN": return false;
                    case "ALLOC": return false;
                    case "DALLOC": return false;
                    case "CALL": return false;
                    case "RETURN": return false;
                };

                try
                {
                    int start = m.IndexOf(' ') + 1;
                    var opcode = GetOpcode(m.Substring(start));

                    label = m.Split(' ')[0];
                    return true;
                }
                catch { }

                return false;
            }

            public int NumberOfArguments(string opcode)
            {
                switch (opcode)
                {
                    case "LDC": return 1;
                    case "LDV": return 1;
                    case "ADD": return 0;
                    case "SUB": return 0;
                    case "MULT": return 0;
                    case "DIVI": return 0;
                    case "INV": return 0;
                    case "AND": return 0;
                    case "OR": return 0;
                    case "NEG": return 0;
                    case "CME": return 0;
                    case "CMA": return 0;
                    case "CEQ": return 0;
                    case "CDIF": return 0;
                    case "CMEQ": return 0;
                    case "CMAQ": return 0;
                    case "START": return 0;
                    case "HLT": return 0;
                    case "STR": return 1;
                    case "JMP": return 1;
                    case "JMPF": return 1;
                    case "NULL": return 0;
                    case "RD": return 0;
                    case "PRN": return 0;
                    case "ALLOC": return 2;
                    case "DALLOC": return 2;
                    case "CALL": return 1;
                    case "RETURN": return 0;
                };
                
                throw new Exception("Invalid opcode");
            }

            public bool UseLabel(string opcode)
            {
                switch (opcode)
                {
                    case "LDC": return false;
                    case "LDV": return false;
                    case "ADD": return false;
                    case "SUB": return false;
                    case "MULT": return false;
                    case "DIVI": return false;
                    case "INV": return false;
                    case "AND": return false;
                    case "OR": return false;
                    case "NEG": return false;
                    case "CME": return false;
                    case "CMA": return false;
                    case "CEQ": return false;
                    case "CDIF": return false;
                    case "CMEQ": return false;
                    case "CMAQ": return false;
                    case "START": return false;
                    case "HLT": return false;
                    case "STR": return false;
                    case "JMP": return true;
                    case "JMPF": return true;
                    case "NULL": return false;
                    case "RD": return false;
                    case "PRN": return false;
                    case "ALLOC": return false;
                    case "DALLOC": return false;
                    case "CALL": return true;
                    case "RETURN": return false;
                };

                throw new Exception("Invalid opcode");
            }
        }

        public struct Registers
        {
            public int S; // stack index
            public int I; // instruction index
        };

        public struct MemorySegments
        {
            public Instruction[] Instructions;
            public int maxInstructions;
            public int[] M;
        };

        public class Breakpoint
        {
            public int I;

            public Breakpoint(int I)
            {
                this.I = I;
            }
        }

        public struct IO
        {
            public bool waiting;
            public int input;
            public int output;
        }

        public Registers registers;
        public MemorySegments memory;
        public IO io;

        public List<Breakpoint> breakpoints;
        public Dictionary<string, int> labelTable;

        public VMCore(uint maxInstructions, uint maxMemory)
        {
            registers.S = 0;
            registers.I = 0;
            memory.Instructions = new Instruction[maxInstructions];
            memory.M = new int[maxMemory];
            memory.maxInstructions = 0;
            breakpoints = new List<Breakpoint>();
            labelTable = new Dictionary<string, int>();
            io.waiting = false;
        }

        public void ParseFromFile(string path)
        {
            string[] lines = File.ReadAllLines(path);
            memory.maxInstructions = 0;
            foreach (var l in lines)
            {
                memory.Instructions[memory.maxInstructions] = new Instruction(l);

                if (memory.Instructions[memory.maxInstructions].isLabel)
                {
                    labelTable[memory.Instructions[memory.maxInstructions].label] = memory.maxInstructions;
                }
                memory.maxInstructions++;
            }
        }

        public void Run(bool singleStep, bool stopOnBreakPoint)
        {
            while (true)
            {
                bool continueExec = SingleStep(stopOnBreakPoint);

                if (!continueExec || singleStep)
                    break;
            }
        }

        public bool SingleStep(bool stopOnBreakPoint)
        {
            bool continueExec = true;

            if (registers.I < memory.Instructions.Count())
            {
                var instruction = memory.Instructions[registers.I];
                var opcode = instruction.opcode;

                if (stopOnBreakPoint && breakpoints.IndexOf(new Breakpoint(registers.I)) != -1)
                {
                    continueExec = false;
                    return false;
                }

                switch (opcode)
                {
                    case "LDC":
                        registers.S += 1;
                        memory.M[registers.S] = int.Parse(instruction.arg1);
                        registers.I++;
                        break;
                    case "LDV":
                        registers.S += 1;
                        memory.M[registers.S] = memory.M[int.Parse(instruction.arg1)];
                        registers.I++;
                        break;
                    case "ADD":
                        memory.M[registers.S - 1] += memory.M[registers.S];
                        registers.S -= 1;
                        registers.I++;
                        break;
                    case "SUB":
                        memory.M[registers.S - 1] -= memory.M[registers.S];
                        registers.S -= 1;
                        registers.I++;
                        break;
                    case "MULT":
                        memory.M[registers.S - 1] *= memory.M[registers.S];
                        registers.S -= 1;
                        registers.I++;
                        break;
                    case "DIVI":
                        memory.M[registers.S - 1] /= memory.M[registers.S];
                        registers.S -= 1;
                        registers.I++;
                        break;
                    case "INV":
                        memory.M[registers.S] = -memory.M[registers.S];
                        registers.I++;
                        break;
                    case "AND":
                        registers.I++;
                        break;
                    case "OR":
                        registers.I++;
                        break;
                    case "NEG":
                        memory.M[registers.S] = 1 - memory.M[registers.S];
                        registers.I++;
                        break;
                    case "CME":
                        registers.I++;
                        break;
                    case "CMA":
                        registers.I++;
                        break;
                    case "CEQ":
                        registers.I++;
                        break;
                    case "CDIF":
                        registers.I++;
                        break;
                    case "CMEQ":
                        registers.I++;
                        break;
                    case "CMAQ":
                        registers.I++;
                        break;
                    case "START":
                        registers.I = 0;
                        registers.S = -1;
                        registers.I++;
                        break;
                    case "HLT":
                        continueExec = false;
                        break;
                    case "STR":
                        registers.I++;
                        break;
                    case "JMP":
                        registers.I = labelTable[instruction.arg1];
                        break;
                    case "JMPF":
                        registers.I = labelTable[instruction.arg1];
                        break;
                    case "NULL":
                        registers.I++;
                        break;
                    case "RD":
                        registers.I++;
                        break;
                    case "PRN":
                        registers.I++;
                        break;
                    case "ALLOC":
                        {
                            int m = int.Parse(instruction.arg1);
                            int n = int.Parse(instruction.arg2);
                            for (int k = 0; k < n; k++)
                            {
                                registers.S++;
                                memory.M[registers.S] = memory.M[m + k];
                            }
                        }
                        registers.I++;
                        break;
                    case "DALLOC":
                        {
                            int m = int.Parse(instruction.arg1);
                            int n = int.Parse(instruction.arg2);
                            for (int k = n - 1; k >= 0; k--)
                            {
                                registers.S--;
                                memory.M[m + k] = memory.M[registers.S];
                            }
                        }
                        registers.I++;
                        break;
                    case "CALL":
                        registers.S++;
                        memory.M[registers.S] = registers.I + 1;
                        registers.I = labelTable[instruction.arg1];
                        break;
                    case "RETURN":
                        registers.I = memory.M[registers.S];
                        registers.S--;
                        break;
                };
            }

            return continueExec;
        }
    }
}

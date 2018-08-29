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
                    case "LDC": return tmp;
                    case "LDV": return tmp;
                    case "ADD": return tmp;
                    case "SUB": return tmp;
                    case "MULT": return tmp;
                    case "DIVI": return tmp;
                    case "INV": return tmp;
                    case "AND": return tmp;
                    case "OR": return tmp;
                    case "NEG": return tmp;
                    case "CME": return tmp;
                    case "CMA": return tmp;
                    case "CEQ": return tmp;
                    case "CDIF": return tmp;
                    case "CMEQ": return tmp;
                    case "CMAQ": return tmp;
                    case "START": return tmp;
                    case "HLT": return tmp;
                    case "STR": return tmp;
                    case "JMP": return tmp;
                    case "JMPF": return tmp;
                    case "NULL": return tmp;
                    case "RD": return tmp;
                    case "PRN": return tmp;
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

        public Instruction[] instructions; // instructions memory
        public int instructionCount; // number of instructions
        public int[] M; // stack

        public struct IO
        {
            public bool waitingInput;
            public bool waitingOutput;
            public int inputMemoryIndex;
            public int outputMemoryIndex;
        }

        // registers
        public int S; // stack index
        public int I; // instruction index

        public IO io;

        public List<int> breakpoints;
        public Dictionary<string, int> labelTable;

        public bool lastBreakWasBreakpoint = false;

        public VMCore(uint maxInstructions, uint maxMemory)
        {
            S = 0;
            I = 0;
            instructions = new Instruction[maxInstructions];
            M = new int[maxMemory];
            instructionCount = 0;
            breakpoints = new List<int>();
            labelTable = new Dictionary<string, int>();
            io.waitingInput = false;
            io.waitingOutput = false;
            io.inputMemoryIndex = -1;
            io.outputMemoryIndex = -1;
        }

        public void ParseFromFile(string path)
        {
            string[] lines = File.ReadAllLines(path);
            instructionCount = 0;
            foreach (var l in lines)
            {
                instructions[instructionCount] = new Instruction(l);

                if (instructions[instructionCount].isLabel)
                {
                    labelTable[instructions[instructionCount].label] = instructionCount;
                }
                instructionCount++;
            }
        }

        public void Run(bool singleStep)
        {
            while (true)
            {
                bool continueExec = SingleStep();
                if (!continueExec || singleStep)
                    break;
            }
        }

        public int IORead()
        {
            if (!io.waitingOutput)
                throw new Exception("Invalid Read IO operation");

            int ret = M[io.outputMemoryIndex];
            io.waitingOutput = false;
            return ret;
        }

        public void IOWrite(int data)
        {
            if (!io.waitingInput)
                throw new Exception("Invalid Write IO operation");

            M[io.inputMemoryIndex] = data;
            io.waitingInput = false;
        }

        public bool SingleStep()
        {
            bool continueExec = true;

            if (io.waitingInput)
                return false;

            if (I < instructionCount)
            {
                var instruction = instructions[I];
                var opcode = instruction.opcode;

                int bpIdx = breakpoints.IndexOf(I);
                if (bpIdx != -1 && !lastBreakWasBreakpoint)
                {
                    lastBreakWasBreakpoint = true;
                    return false;
                }
                else
                {
                    lastBreakWasBreakpoint = false;
                }

                switch (opcode)
                {
                    case "LDC":
                        S += 1;
                        M[S] = int.Parse(instruction.arg1);
                        I++;
                        break;
                    case "LDV":
                        S += 1;
                        M[S] = M[int.Parse(instruction.arg1)];
                        I++;
                        break;
                    case "ADD":
                        M[S - 1] += M[S];
                        S -= 1;
                        I++;
                        break;
                    case "SUB":
                        M[S - 1] -= M[S];
                        S -= 1;
                        I++;
                        break;
                    case "MULT":
                        M[S - 1] *= M[S];
                        S -= 1;
                        I++;
                        break;
                    case "DIVI":
                        M[S - 1] /= M[S];
                        S -= 1;
                        I++;
                        break;
                    case "INV":
                        M[S] = -M[S];
                        I++;
                        break;
                    case "AND":
                        M[S - 1] = (M[S - 1] == 1 && M[S] == 1) ? 1 : 0;
                        S--;
                        I++;
                        break;
                    case "OR":
                        M[S - 1] = (M[S - 1] == 1 || M[S] == 1) ? 1 : 0;
                        S--;
                        I++;
                        break;
                    case "NEG":
                        M[S] = 1 - M[S];
                        I++;
                        break;
                    case "CME":
                        M[S - 1] = (M[S - 1] < M[S]) ? 1 : 0;
                        S--;
                        I++;
                        break;
                    case "CMA":
                        M[S - 1] = (M[S - 1] > M[S]) ? 1 : 0;
                        S--;
                        I++;
                        break;
                    case "CEQ":
                        M[S - 1] = (M[S - 1] == M[S]) ? 1 : 0;
                        S--;
                        I++;
                        break;
                    case "CDIF":
                        M[S - 1] = (M[S - 1] != M[S]) ? 1 : 0;
                        S--;
                        I++;
                        break;
                    case "CMEQ":
                        M[S - 1] = (M[S - 1] <= M[S]) ? 1 : 0;
                        S--;
                        I++;
                        break;
                    case "CMAQ":
                        M[S - 1] = (M[S - 1] >= M[S]) ? 1 : 0;
                        S--;
                        I++;
                        break;
                    case "START":
                        I = 0;
                        S = -1;
                        I++;
                        break;
                    case "HLT":
                        continueExec = false;
                        break;
                    case "STR":
                        {
                            int n = int.Parse(instruction.arg1);
                            M[n] = M[S];
                            S--;
                        }
                        I++;
                        break;
                    case "JMP":
                        I = labelTable[instruction.arg1];
                        break;
                    case "JMPF":
                        I = M[S] == 0 ? labelTable[instruction.arg1] : I + 1;
                        S--;
                        break;
                    case "NULL":
                        I++;
                        break;
                    case "RD":
                        continueExec = false;
                        S++;
                        io.waitingInput = true;
                        io.inputMemoryIndex = S;
                        I++;
                        break;
                    case "PRN":
                        io.waitingOutput = true;
                        io.outputMemoryIndex = S;
                        S--;
                        I++;
                        break;
                    case "ALLOC":
                        {
                            int m = int.Parse(instruction.arg1);
                            int n = int.Parse(instruction.arg2);
                            for (int k = 0; k < n; k++)
                            {
                                S++;
                                M[S] = M[m + k];
                            }
                        }
                        I++;
                        break;
                    case "DALLOC":
                        {
                            int m = int.Parse(instruction.arg1);
                            int n = int.Parse(instruction.arg2);
                            for (int k = n - 1; k >= 0; k--)
                            {
                                S--;
                                M[m + k] = M[S];
                            }
                        }
                        I++;
                        break;
                    case "CALL":
                        S++;
                        M[S] = I + 1;
                        I = labelTable[instruction.arg1];
                        break;
                    case "RETURN":
                        I = M[S];
                        S--;
                        break;
                };
            }

            return continueExec;
        }
    }
}

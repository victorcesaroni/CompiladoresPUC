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
        public enum InstructionOpcode : UInt16
        {
            LDC,
            LDV,
            ADD,
            SUB,
            MULT,
            DIVI,
            INV,
            AND,
            OR,
            NEG,
            CME,
            CMA,
            CEQ,
            CDIF,
            CMEQ,
            CMAQ,
            START,
            HLT,
            STR,
            JMP,
            JMPF,
            NULL,
            RD,
            PRN,
            ALLOC,
            DALLOC,
            CALL,
            RETURN,
        };

        public class Instruction
        {
            public InstructionOpcode opcode;
            public UInt32 arg1;
            public UInt32 arg2;
            public int numArgs;
            public string comment;

            public Instruction()
            {

            }

            public Instruction(string text)
            {
                text = text.Replace("\t", " ");
                text = text.Replace("  ", " ");
                string[] s = text.Split(' ');

                int commentIdx = text.IndexOf("#");
                if (commentIdx != -1)
                {
                    comment = text.Substring(commentIdx);
                }
                else
                {
                    comment = "";
                }

                opcode = GetOpcode(s[0]);
                numArgs = NumberOfArguments(opcode);

                if (numArgs == 0)
                {
                    arg1 = UInt32.MaxValue;
                    arg2 = UInt32.MaxValue;
                }
                else if (numArgs == 1)
                {
                    arg1 = UInt32.Parse(s[1]);
                    arg2 = UInt32.MaxValue;
                }
                else if (numArgs == 2)
                {
                    arg1 = UInt32.Parse(s[1]);
                    arg2 = UInt32.Parse(s[2]);
                }
            }

            public InstructionOpcode GetOpcode(string m)
            {
                switch (m)
                {
                    case "LDC": return InstructionOpcode.LDC;
                    case "LDV": return InstructionOpcode.LDV;
                    case "ADD": return InstructionOpcode.ADD;
                    case "SUB": return InstructionOpcode.SUB;
                    case "MULT": return InstructionOpcode.MULT;
                    case "DIVI": return InstructionOpcode.DIVI;
                    case "INV": return InstructionOpcode.INV;
                    case "AND": return InstructionOpcode.AND;
                    case "OR": return InstructionOpcode.OR;
                    case "NEG": return InstructionOpcode.NEG;
                    case "CME": return InstructionOpcode.CME;
                    case "CMA": return InstructionOpcode.CMA;
                    case "CEQ": return InstructionOpcode.CEQ;
                    case "CDIF": return InstructionOpcode.CDIF;
                    case "CMEQ": return InstructionOpcode.CMEQ;
                    case "CMAQ": return InstructionOpcode.CMAQ;
                    case "START": return InstructionOpcode.START;
                    case "HLT": return InstructionOpcode.HLT;
                    case "STR": return InstructionOpcode.STR;
                    case "JMP": return InstructionOpcode.JMP;
                    case "JMPF": return InstructionOpcode.JMPF;
                    case "NULL": return InstructionOpcode.NULL;
                    case "RD": return InstructionOpcode.RD;
                    case "PRN": return InstructionOpcode.PRN;
                    case "ALLOC": return InstructionOpcode.ALLOC;
                    case "DALLOC": return InstructionOpcode.DALLOC;
                    case "CALL": return InstructionOpcode.CALL;
                    case "RETURN": return InstructionOpcode.RETURN;
                };
                throw new Exception("Invalid opcode");
            }

            public int NumberOfArguments(InstructionOpcode opcode)
            {
                switch (opcode)
                {
                    case InstructionOpcode.LDC: return 1;
                    case InstructionOpcode.LDV: return 1;
                    case InstructionOpcode.ADD: return 0;
                    case InstructionOpcode.SUB: return 0;
                    case InstructionOpcode.MULT: return 0;
                    case InstructionOpcode.DIVI: return 0;
                    case InstructionOpcode.INV: return 0;
                    case InstructionOpcode.AND: return 0;
                    case InstructionOpcode.OR: return 0;
                    case InstructionOpcode.NEG: return 0;
                    case InstructionOpcode.CME: return 0;
                    case InstructionOpcode.CMA: return 0;
                    case InstructionOpcode.CEQ: return 0;
                    case InstructionOpcode.CDIF: return 0;
                    case InstructionOpcode.CMEQ: return 0;
                    case InstructionOpcode.CMAQ: return 0;
                    case InstructionOpcode.START: return 0;
                    case InstructionOpcode.HLT: return 0;
                    case InstructionOpcode.STR: return 1;
                    case InstructionOpcode.JMP: return 1;
                    case InstructionOpcode.JMPF: return 1;
                    case InstructionOpcode.NULL: return 0;
                    case InstructionOpcode.RD: return 0;
                    case InstructionOpcode.PRN: return 0;
                    case InstructionOpcode.ALLOC: return 2;
                    case InstructionOpcode.DALLOC: return 2;
                    case InstructionOpcode.CALL: return 1;
                    case InstructionOpcode.RETURN: return 0;
                };

                throw new Exception("Invalid opcode");
            }
        }

        public struct Registers
        {
            public uint S; // stack index
            public uint I; // instruction index
        };

        public struct MemorySegments
        {
            public Instruction[] M;
            public UInt32[] S;
        };

        public class Breakpoint
        {
            public uint I;

            public Breakpoint(uint I)
            {
                this.I = I;
            }
        }

        public class IO
        {
            public bool waiting;
            public int input;
            public int output;
        }

        public Registers registers;
        public MemorySegments memory;
        public IO io;

        public List<Breakpoint> breakpoints;        

        public VMCore(uint maxInstructions, uint maxMemory)
        {
            registers.S = 0;
            registers.I = 0;
            memory.M = new Instruction[maxInstructions];
            memory.S = new UInt32[maxMemory];
            breakpoints = new List<Breakpoint>();
            io.waiting = false;
        }

        public void ParseFromFile(string path)
        {
            string[] lines = File.ReadAllLines(path);
            int i = 0;
            foreach (var l in lines)
            {
                memory.M[i] = new Instruction(l);
                i++;
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

            if (registers.I < memory.M.Count())
            {
                var opcode = memory.M[registers.I].opcode;

                if (stopOnBreakPoint && breakpoints.IndexOf(new Breakpoint(registers.I)) != -1)
                {
                    continueExec = false;
                    return false;
                }

                switch (opcode)
                {
                    case InstructionOpcode.LDC:
                        registers.I++;
                        break;
                    case InstructionOpcode.LDV:
                        registers.I++;
                        break;
                    case InstructionOpcode.ADD:
                        registers.I++;
                        break;
                    case InstructionOpcode.SUB:
                        registers.I++;
                        break;
                    case InstructionOpcode.MULT:
                        registers.I++;
                        break;
                    case InstructionOpcode.DIVI:
                        registers.I++;
                        break;
                    case InstructionOpcode.INV:
                        registers.I++;
                        break;
                    case InstructionOpcode.AND:
                        registers.I++;
                        break;
                    case InstructionOpcode.OR:
                        registers.I++;
                        break;
                    case InstructionOpcode.NEG:
                        registers.I++;
                        break;
                    case InstructionOpcode.CME:
                        registers.I++;
                        break;
                    case InstructionOpcode.CMA:
                        registers.I++;
                        break;
                    case InstructionOpcode.CEQ:
                        registers.I++;
                        break;
                    case InstructionOpcode.CDIF:
                        registers.I++;
                        break;
                    case InstructionOpcode.CMEQ:
                        registers.I++;
                        break;
                    case InstructionOpcode.CMAQ:
                        registers.I++;
                        break;
                    case InstructionOpcode.START:
                        registers.I++;
                        break;
                    case InstructionOpcode.HLT:
                        registers.I++;
                        break;
                    case InstructionOpcode.STR:
                        registers.I++;
                        break;
                    case InstructionOpcode.JMP:
                        registers.I++;
                        break;
                    case InstructionOpcode.JMPF:
                        registers.I++;
                        break;
                    case InstructionOpcode.NULL:
                        registers.I++;
                        break;
                    case InstructionOpcode.RD:
                        registers.I++;
                        break;
                    case InstructionOpcode.PRN:
                        registers.I++;
                        break;
                    case InstructionOpcode.ALLOC:
                        registers.I++;
                        break;
                    case InstructionOpcode.DALLOC:
                        registers.I++;
                        break;
                    case InstructionOpcode.CALL:
                        registers.I++;
                        break;
                    case InstructionOpcode.RETURN:
                        registers.I++;
                        break;
                };
            }

            return continueExec;
        }
    }
}

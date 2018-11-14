using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Compilador
{
    public class GeradorCodigo
    {
        public StreamWriter arquivo;

        public GeradorCodigo(StreamWriter arquivo)
        {
            this.arquivo = arquivo;
        }

        public void Gera(string label, string asm)
        {
            if (label != "")
                arquivo.Write(label + "\t");
            arquivo.Write(asm + "\r\n");
        }

        public void LDC(string var, string label = "") { Gera(label, "LDC " + var); }
        public void LDV(string var, string label = "") { Gera(label, "LDV " + var); }
        public void ADD(string label = "") { Gera(label, "ADD"); }
        public void SUB(string label = "") { Gera(label, "SUB"); }
        public void MULT(string label = "") { Gera(label, "MULT"); }
        public void DIVI(string label = "") { Gera(label, "DIVI"); }
        public void INV(string label = "") { Gera(label, "INV"); }
        public void AND(string label = "") { Gera(label, "AND"); }
        public void OR(string label = "") { Gera(label, "OR"); }
        public void NEG(string label = "") { Gera(label, "NEG"); }
        public void CME(string label = "") { Gera(label, "CME"); }
        public void CMA(string label = "") { Gera(label, "CMA"); }
        public void CMQ(string label = "") { Gera(label, "CMQ"); }
        public void CDIF(string label = "") { Gera(label, "CDIF"); }
        public void CMEQ(string label = "") { Gera(label, "CMEQ"); }
        public void CMAQ(string label = "") { Gera(label, "CMAQ"); }
        public void STAT(string label = "") { Gera(label, "START"); }
        public void HLT(string label = "") { Gera(label, "HLT"); }
        public void STR(string label = "") { Gera(label, "STR"); }
        public void JMP(string to, string label = "") { Gera(label, "JMP " + to); }
        public void JMPF(string to, string label = "") { Gera(label, "JMPF " + to); }
        public void NULL(string label = "") { Gera(label, "NULL"); }
        public void RD(string label = "") { Gera(label, "RD"); }
        public void PRN(string label = "") { Gera(label, "PRN"); }
        public void ALLOC(string m, string n, string label = "") { Gera(label, "ALLOC " + m + "," + n); }
        public void DEALLOC(string m, string n, string label = "") { Gera(label, "DEALLOC " + m + "," + n); }
        public void CALL(string to, string label = "") { Gera(label, "CALL " + to); }
        public void RETURN(string label = "") { Gera(label, "RETURN"); }
        public void RETURNF(string label = "") { Gera(label, "RETURNF"); }
    }
}

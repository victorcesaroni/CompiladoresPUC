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

        public void Gera(string label, string asm, string comentario)
        {
            String line = "";

            if (comentario != "")
                line = String.Format("{0,-12} {1,-12} # {2}\r\n", label, asm, comentario);
            else
                line = String.Format("{0,-12} {1,-12}\r\n", label, asm);

            arquivo.Write(line);
        }

        public void LDC(string var, string label = "", string comentario = "") { Gera(label, "LDC " + var, comentario); }
        public void LDV(string var, string label = "", string comentario = "") { Gera(label, "LDV " + var, comentario); }
        public void ADD(string label = "", string comentario = "") { Gera(label, "ADD", comentario); }
        public void SUB(string label = "", string comentario = "") { Gera(label, "SUB", comentario); }
        public void MULT(string label = "", string comentario = "") { Gera(label, "MULT", comentario); }
        public void DIVI(string label = "", string comentario = "") { Gera(label, "DIVI", comentario); }
        public void INV(string label = "", string comentario = "") { Gera(label, "INV", comentario); }
        public void AND(string label = "", string comentario = "") { Gera(label, "AND", comentario); }
        public void OR(string label = "", string comentario = "") { Gera(label, "OR", comentario); }
        public void NEG(string label = "", string comentario = "") { Gera(label, "NEG", comentario); }
        public void CME(string label = "", string comentario = "") { Gera(label, "CME", comentario); }
        public void CMA(string label = "", string comentario = "") { Gera(label, "CMA", comentario); }
        public void CMQ(string label = "", string comentario = "") { Gera(label, "CMQ", comentario); }
        public void CDIF(string label = "", string comentario = "") { Gera(label, "CDIF", comentario); }
        public void CMEQ(string label = "", string comentario = "") { Gera(label, "CMEQ", comentario); }
        public void CMAQ(string label = "", string comentario = "") { Gera(label, "CMAQ", comentario); }
        public void START(string label = "", string comentario = "") { Gera(label, "START", comentario); }
        public void HLT(string label = "", string comentario = "") { Gera(label, "HLT", comentario); }
        public void STR(string var, string label = "", string comentario = "") { Gera(label, "STR " + var, comentario); }
        public void JMP(string to, string label = "", string comentario = "") { Gera(label, "JMP " + to, comentario); }
        public void JMPF(string to, string label = "", string comentario = "") { Gera(label, "JMPF " + to, comentario); }
        public void NULL(string label = "", string comentario = "") { Gera(label, "NULL", comentario); }
        public void RD(string label = "", string comentario = "") { Gera(label, "RD", comentario); }
        public void PRN(string label = "", string comentario = "") { Gera(label, "PRN", comentario); }
        public void ALLOC(string m, string n, string label = "", string comentario = "") { Gera(label, "ALLOC " + m + "," + n, comentario); }
        public void DALLOC(string m, string n, string label = "", string comentario = "") { Gera(label, "DALLOC " + m + "," + n, comentario); }
        public void CALL(string to, string label = "", string comentario = "") { Gera(label, "CALL " + to, comentario); }
        public void RETURN(string label = "", string comentario = "") { Gera(label, "RETURN", comentario); }
        public void RETURNF(string m, string n, string label = "", string comentario = "") { Gera(label, "RETURNF " + m + "," + n, comentario); }
    }
}

using System;
using System.Collections.Generic;
using System.IO;

namespace Compilador
{
    public enum SimboloTipo
    {
        INTEIRO,
        BOOLEANO,
        FUNCAO_INTEIRO,
        FUNCAO_BOOLEANO,
        PROCEDIMENTO,
    }

    public class SimboloInfo
    {
        public SimboloInfo(string lexema, SimboloTipo tipo, bool marca)
        {
            this.lexema = lexema;
            this.tipo = tipo;
            this.marca = marca;
        }

        public string lexema;
        public SimboloTipo tipo;
        public bool marca;
    }
    
    public class TabelaSimbolo
    {
        public List<SimboloInfo> simbolos = new List<SimboloInfo>();

        public void Insere(List<string> lexemas, SimboloTipo tipo, bool marca)
        {
            foreach (var item in lexemas)
            {
                simbolos.Add(new SimboloInfo(item, tipo, marca));
            }
        }

        public bool PesquisaDuplicidadeVariavel(string lexema)
        {
            for (int i = simbolos.Count - 1; i >= 0; i--)
            {
                if (simbolos[i].marca)
                    return false;
                if (simbolos[i].lexema == lexema)
                    return true;
            }
            return false;
        }

        public bool PesquisaDuplicidadeProcedimentoFuncao(string lexema)
        {
            for (int i = simbolos.Count - 1; i >= 0; i--)
            {
                if (simbolos[i].lexema == lexema)
                    return true;
            }
            return false;
        }

        public void VoltaNivel()
        {
            for (int i = simbolos.Count - 1; i >= 0; i--)
            {
                if (!simbolos[i].marca)
                {
                    simbolos.RemoveAt(i);
                }
                else
                {
                    simbolos[i].marca = false;
                    break;
                }
            }
        }        
    }

    public class AnalisadorSemantico
    {
        public TabelaSimbolo tabelaSimbolo = new TabelaSimbolo();
        public List<Token> posFixaPilhaAux = new List<Token>();
        public List<Token> posFixa = new List<Token>();

        public AnalisadorSemantico()
        {

        }

        public static Dictionary<Simbolo, int> prioridade = new Dictionary<Simbolo, int>() {
            /*{ "u+", 5 },
            { "u-", 5 },
            { "nao", 5 },
            { "*", 4 },
            { "div", 4 },
            { "+", 3 },
            { "-", 3 },
            { "<", 2 },
            { ">", 2 },
            { ">=", 2 },
            { "<=", 2 },
            { "=", 2 },
            { "!=", 2 },
            { "e", 1 },
            { "ou", 0 },*/
            { Simbolo.S_NAO, 5 },
            { Simbolo.S_MULT, 4 },
            { Simbolo.S_DIV, 4 },
            { Simbolo.S_MAIS, 3 },
            { Simbolo.S_MENOS, 3 },
            { Simbolo.S_MENOR, 2 },
            { Simbolo.S_MAIOR, 2 },
            { Simbolo.S_MAIOR_IG, 2 },
            { Simbolo.S_MENOR_IG, 2 },
            { Simbolo.S_IG, 2 },
            { Simbolo.S_DIF, 2 },
            { Simbolo.S_E, 1 },
            { Simbolo.S_OU, 0 },
        };

        public void ParaPosFixa(Token token)
        {
            if (token.simbolo == Simbolo.S_ABRE_PARENTESES)
            {
                posFixaPilhaAux.Insert(0, token);
            }
            else if (token.simbolo == Simbolo.S_FECHA_PARENTESES)
            {
                while (posFixaPilhaAux[0].simbolo != Simbolo.S_ABRE_PARENTESES)
                {
                    posFixa.Add(posFixaPilhaAux[0]);
                    posFixaPilhaAux.RemoveAt(0);
                }
                posFixaPilhaAux.RemoveAt(0);
            }
            else if (prioridade.ContainsKey(token.simbolo))
            {
                // é um operando
                while (posFixaPilhaAux.Count > 0)
                {
                    if (posFixaPilhaAux[0].simbolo != Simbolo.S_ABRE_PARENTESES && prioridade[token.simbolo] < prioridade[posFixaPilhaAux[0].simbolo])
                    {
                        posFixa.Add(posFixaPilhaAux[0]);
                        posFixaPilhaAux.RemoveAt(0);
                    }
                    else
                    {
                        break;
                    }
                }

                posFixaPilhaAux.Insert(0, token);
            }           
            else
            {
                posFixa.Add(token);
            }
        }

        public void ReinicializaPosFixa()
        {
            posFixaPilhaAux.Clear();
            posFixa.Clear();
        }

        public void FinalizaPosFixa()
        {
            //desempilha tudo
            while (posFixaPilhaAux.Count > 0)
            {
                posFixa.Add(posFixaPilhaAux[0]);
                posFixaPilhaAux.RemoveAt(0);
            }

            for (int i = 0; i < posFixa.Count; i++)
                System.Diagnostics.Debug.Write(posFixa[i].lexema);
            System.Diagnostics.Debug.WriteLine("");
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;

namespace Compilador
{
    public class AnalisadorSemantico
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

        TabelaSimbolo tabelaSimbolo;

        public class TabelaSimbolo
        {
            public List<SimboloInfo> simbolos;

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

            public static Dictionary<string, int> prioridade = new Dictionary<string, int>() {
                { "u+", 5 },
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
                { "ou", 0 },
            };

            public List<string> ParaPosFixa(List<string> expressao)
            {
                List<string> pilha = new List<string>();
                List<string> posfixa = new List<string>();
                List<string> expressaoProcessada = new List<string>();

                for (int i = 0; i < expressao.Count; i++)
                {
                    string x = expressao[i];

                    if (x == "+" || x == "-")
                    {
                        if (i > 0)
                        {
                            if (!(expressao[i - 1][0] >= '0' && expressao[i - 1][0] <= '9' &&
                                expressao[i - 1][0] >= 'A' && expressao[i - 1][0] <= 'Z' &&
                                expressao[i - 1][0] >= 'a' && expressao[i - 1][0] <= 'z'))
                            {
                                x = "u" + x;
                            }
                        }
                        else if (i == 0)
                        {
                            x = "u" + x;
                        }
                    }

                    expressaoProcessada.Add(x);
                }
               
                return posfixa;
            }
        }

        public AnalisadorSemantico()
        {

        }
    }
}

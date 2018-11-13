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

        public TabelaSimbolo tabelaSimbolo;

        public class TabelaSimbolo
        {
            public List<SimboloInfo> simbolos;
            public List<Token> posFixaPilha;
            public List<Token> posFixa;

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

                { Simbolo.S_MAIS, 5 },
            };

            public void ParaPosFixa(Token token, bool unario)
            {
                /*for (int i = 0; i < expressao.Count; i++)
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
                }*/

                if (prioridade.ContainsKey(token.simbolo))
                {
                    // é um operando
                    while (posFixaPilha.Count > 0)
                    {
                        int pt = prioridade[token.simbolo];
                        int pp = prioridade[posFixaPilha[0].simbolo];

                        if (unario)
                            pp = pt;

                        if (posFixaPilha[0].simbolo != Simbolo.S_ABRE_PARENTESES && pt < pp)
                        {
                            posFixa.Add(posFixaPilha[0]);
                            posFixaPilha.RemoveAt(0);
                        }
                        else
                        {
                            break;
                        }
                    }

                    posFixaPilha.Insert(0, token);
                }
                else if (token.simbolo == Simbolo.S_ABRE_PARENTESES)
                {
                    posFixaPilha.Insert(0, token);
                }
                else if (token.simbolo == Simbolo.S_FECHA_PARENTESES)
                {
                    while (posFixaPilha[0].simbolo != Simbolo.S_ABRE_PARENTESES)
                    {
                        posFixa.Add(posFixaPilha[0]);
                        posFixaPilha.RemoveAt(0);
                    }
                    posFixaPilha.RemoveAt(0);
                }
                else
                {
                    posFixa.Add(token);
                }
            }
        }

        public AnalisadorSemantico()
        {
            tabelaSimbolo = new TabelaSimbolo();
        }
    }
}

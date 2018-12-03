using System;
using System.Collections.Generic;
using System.IO;

namespace Compilador
{
    public class ExceptionVariavelDuplicada : Exception
    {
        public Token token;
        public string info;

        public ExceptionVariavelDuplicada(string info, Token token)
        {
            this.token = token;
            this.info = info;
        }

        public override string ToString()
        {
            return info + " " + "Variável/Procedimento '" + token.lexema + "' já declarada(o)";
        }
    }
    public class ExceptionVariavelNaoDeclarada : Exception
    {
        public Token token;
        public string info;

        public ExceptionVariavelNaoDeclarada(string info, Token token)
        {
            this.token = token;
            this.info = info;
        }

        public override string ToString()
        {
            return info + " " + "Variável '" + token.lexema + "' usada mas não declarada";
        }
    }
    public class ExceptionTipoInvalido : Exception
    {
        public Token token;
        public string info;
        public SimboloTipo esperado;
        public SimboloTipo recebido;

        public ExceptionTipoInvalido(string info, SimboloTipo esperado, SimboloTipo recebido, Token token)
        {
            this.token = token;
            this.info = info;
            this.recebido = recebido;
            this.esperado = esperado;
        }

        public override string ToString()
        {
            return info + " " + "Esperava o tipo '" + esperado.ToString() + "' mas recebeu '" + recebido.ToString() + "' no token '" + token.lexema + "' ";
        }
    }

    public enum SimboloTipo
    {
        INTEIRO,
        BOOLEANO,
        FUNCAO_INTEIRO,
        FUNCAO_BOOLEANO,
        PROCEDIMENTO,
        INVALIDO,
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

        public bool PesquisaDuplicidadeNoEscopo(string lexema)
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
                if (simbolos[i].lexema == lexema &&(
                    simbolos[i].tipo == SimboloTipo.PROCEDIMENTO ||
                    simbolos[i].tipo == SimboloTipo.FUNCAO_INTEIRO ||
                    simbolos[i].tipo == SimboloTipo.FUNCAO_BOOLEANO))
                    return true;
            }
            return false;
        }

        public bool PesquisaDuplicidade(string lexema)
        {
            for (int i = simbolos.Count - 1; i >= 0; i--)
            {
                if (simbolos[i].lexema == lexema)
                    return true;
            }
            return false;
        }


        public bool PesquisaVariavelInteiro(string lexema)
        {
            for (int i = simbolos.Count - 1; i >= 0; i--)
            {
                if (simbolos[i].lexema == lexema && 
                    simbolos[i].tipo == SimboloTipo.INTEIRO)
                    return true;
            }
            return false;
        }

        public SimboloInfo Pesquisa(string lexema)
        {
            for (int i = simbolos.Count - 1; i >= 0; i--)
            {
                if (simbolos[i].lexema == lexema)
                    return simbolos[i];
            }
            return null;
        }
        
        public bool PesquisaFuncaoInteiro(string lexema)
        {
            for (int i = simbolos.Count - 1; i >= 0; i--)
            {
                if (simbolos[i].lexema == lexema &&
                    simbolos[i].tipo == SimboloTipo.FUNCAO_INTEIRO)
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

/*
        public SimboloTipo VerificaExpressao()
        {
            SimboloTipo tipo = SimboloTipo.INVALIDO;


            TabelaSimbolo tmpTabela = tabelaSimbolo;
            List<Token> tmp = posFixa;

            while (tmp.Count > 1)
            {
                for (int i = 1; i < tmp.Count; i++)
                {
                    if (prioridade.ContainsKey(tmp[i].simbolo))
                    {
                        if (tmp[i].simbolo == Simbolo.S_NAO)
                        {
                            SimboloInfo simbolo= tmpTabela.Pesquisa(tmp[i-1].lexema);
                            if(simbolo != null)
                            {
                                if (simbolo.tipo == SimboloTipo.BOOLEANO)
                                {
                                    tmp.RemoveAt(i);
                                    break;
                                }
                            }
                        }
                       /* else if (tmp[i].simbolo == Simbolo.S_MULT || 
                            tmp[i].simbolo == Simbolo.S_DIV || 
                            tmp[i].simbolo == Simbolo.S_MAIS || 
                            tmp[i].simbolo == Simbolo.S_MENOS)
                        {

                        }* /


                       /* if (tipo == SimboloTipo.INVALIDO)
                        {
                            Token a = tmp[i - 2];
                            Token b = tmp[i - 1];

                            SimboloInfo simboloA = tabelaSimbolo.Pesquisa(a.lexema);
                            if (simboloA == null)
                                throw new ExceptionVariavelNaoDeclarada("", a);

                            SimboloInfo simboloB = tabelaSimbolo.Pesquisa(b.lexema);
                            if (simboloB == null)
                                throw new ExceptionVariavelNaoDeclarada("", b);

                            if(tmp[i].)

                            if (simboloA.tipo == simboloB.tipo)
                                tipo = simboloA.tipo;
                        }* /

                       

                        if (simbolo.tipo == SimboloTipo.PROCEDIMENTO)
                            throw new ExceptionTipoInvalido("", SimboloTipo.INVALIDO, simbolo.tipo, token);



                    }
                }
            }

            return tipo;
        }
*/

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

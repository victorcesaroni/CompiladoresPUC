using System;
using System.Collections.Generic;
using System.IO;

namespace Compilador
{
    public class ExceptionSimboloEsperado : Exception
    {
        public Token token;
        public string info;
        public Simbolo esperado;

        public ExceptionSimboloEsperado(string info, Simbolo esperado, Token token)
        {
            this.token = token;
            this.info = info;
            this.esperado = esperado;
        }

        public override string ToString()
        {
            string lexema = "DESCONHECIDO";
            foreach (var pair in AnalisadorLexico.mapaDeSimbolo)
            {
                if (pair.Value == esperado)
                {
                    lexema = pair.Key;
                    break;
                }
            }

            return info + " " + "Esperado '" + lexema + "' mas recebeu '" + token.lexema + "'";
        }
    }


    public class ExceptionSimboloInesperado : Exception
    {
        public Token token;
        public string info;

        public ExceptionSimboloInesperado(string info, Token token)
        {
            this.token = token;
            this.info = info;
        }

        public override string ToString()
        {
            return info + " " + "Token inesperado '" + token.lexema + "'";
        }
    }

    public class AnalisadorSintatico
    {
        public StreamReader arquivo;
        public StreamWriter arquivoObjeto;
        public AnalisadorLexico lexico;
        public AnalisadorSemantico semantico;
        public GeradorCodigo gerador;

        Token token = new Token();

        public AnalisadorSintatico(string caminhoArquivo)
        {
            arquivo = new StreamReader(new FileStream(caminhoArquivo, FileMode.Open));
            arquivoObjeto = new StreamWriter(new FileStream(caminhoArquivo + ".obj", FileMode.OpenOrCreate));
            lexico = new AnalisadorLexico(arquivo);
            semantico = new AnalisadorSemantico();
            gerador = new GeradorCodigo(arquivoObjeto);
        }

        ~AnalisadorSintatico()
        {
            Finalizar();
        }

        public void Iniciar()
        {
            /*try
            {
                while (!lexico.FimDeArquivo())
                {
                    var token = lexico.PegaToken();
                    Console.WriteLine("{0} {1} {2}:{3}", token.simbolo, token.lexema, token.linha, token.coluna);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }*/

            /*try
            {
                Analisa();
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }*/

            Analisa();

            Finalizar();
        }

        public void Finalizar()
        {
            arquivo.Close();
            arquivoObjeto.Close();
        }

        public void Analisa()
        {
            Lexico();
            ChecaSimboloEsperado(Simbolo.S_PROGRAMA);
            Lexico();
            ChecaSimboloEsperado(Simbolo.S_IDENTIFICADOR);
            semantico.tabelaSimbolo.Insere(new List<string> { token.lexema }, SimboloTipo.PROCEDIMENTO, true);
            Lexico();
            ChecaSimboloEsperado(Simbolo.S_PONTO_VIRGULA);
            AnalisaBloco();
            ChecaSimboloEsperado(Simbolo.S_PONTO);

            Lexico();
            if (token.simbolo != Simbolo.S_FINAL_DE_ARQUIVO)
                throw new ExceptionSimboloInesperado("Inesperado apos final de programa", token);
        }

        void AnalisaBloco()
        {
            Lexico();
            AnalisaEtapaDeclaracaoVariaveis();
            AnalisaSubRotinas();
            AnalisaComandos();
        }

        void AnalisaEtapaDeclaracaoVariaveis()
        {
            if (token.simbolo == Simbolo.S_VAR)
            {
                Lexico();
                ChecaSimboloEsperado(Simbolo.S_IDENTIFICADOR);

                while (token.simbolo == Simbolo.S_IDENTIFICADOR)
                {
                    AnalisaVariaveis();
                    ChecaSimboloEsperado(Simbolo.S_PONTO_VIRGULA);
                    Lexico();
                }
            }
        }

        void AnalisaSubRotinas()
        {
            /*if (token.simbolo == Simbolo.S_PROCEDIMENTO || token.simbolo == Simbolo.S_FUNCAO)
            {

            }*/

            while (token.simbolo == Simbolo.S_PROCEDIMENTO || token.simbolo == Simbolo.S_FUNCAO)
            {
                if (token.simbolo == Simbolo.S_PROCEDIMENTO)
                    AnalisaDeclaracaoProcedimento();
                else
                    AnalisaDeclaracaoFuncao();

                ChecaSimboloEsperado(Simbolo.S_PONTO_VIRGULA);
                Lexico();                
            }
        }

        void AnalisaDeclaracaoFuncao()
        {
            Lexico();
            ChecaSimboloEsperado(Simbolo.S_IDENTIFICADOR);

            if (semantico.tabelaSimbolo.PesquisaDuplicidade(token.lexema))
                throw new ExceptionVariavelDuplicada("", token);

            string tmp = token.lexema;
            
            Lexico();
            ChecaSimboloEsperado(Simbolo.S_DOIS_PONTOS);
            Lexico();

            if (token.simbolo != Simbolo.S_INTEIRO && token.simbolo != Simbolo.S_BOOLEANO)
                throw new ExceptionSimboloInesperado("", token);

            if (token.simbolo == Simbolo.S_INTEIRO)
                semantico.tabelaSimbolo.Insere(new List<string> { tmp }, SimboloTipo.FUNCAO_INTEIRO, true);
            else if (token.simbolo == Simbolo.S_BOOLEANO)
                semantico.tabelaSimbolo.Insere(new List<string> { tmp }, SimboloTipo.FUNCAO_BOOLEANO, true);

            Lexico();
            if (token.simbolo == Simbolo.S_PONTO_VIRGULA)
            {
                AnalisaBloco();
            }
            semantico.tabelaSimbolo.VoltaNivel();
        }

        void AnalisaDeclaracaoProcedimento()
        {
            Lexico();
            ChecaSimboloEsperado(Simbolo.S_IDENTIFICADOR);

            if (semantico.tabelaSimbolo.PesquisaDuplicidade(token.lexema))
                throw new ExceptionVariavelDuplicada("", token);

            semantico.tabelaSimbolo.Insere(new List<string> { token.lexema }, SimboloTipo.PROCEDIMENTO, true);

            Lexico();
            ChecaSimboloEsperado(Simbolo.S_PONTO_VIRGULA);
            AnalisaBloco();

            semantico.tabelaSimbolo.VoltaNivel();
        }

        void AnalisaComandos()
        {
            /*if (token.simbolo == Simbolo.S_INICIO)
            {
                Lexico();
                AnalisaComandoSimples();

                while (token.simbolo != Simbolo.S_FIM)
                {
                    if (token.simbolo == Simbolo.S_INICIO)
                        AnalisaComandos();
                    
                     ChecaSimboloEsperado(Simbolo.S_PONTO_VIRGULA);

                    Lexico();
                    if (token.simbolo != Simbolo.S_FIM)
                        AnalisaComandoSimples();
                }
            }*/

            ChecaSimboloEsperado(Simbolo.S_INICIO);

            Lexico();
            AnalisaComandoSimples();
            
            while (token.simbolo != Simbolo.S_FIM)
            {
                ChecaSimboloEsperado(Simbolo.S_PONTO_VIRGULA);
                Lexico();

                if (token.simbolo != Simbolo.S_FIM)
                    AnalisaComandoSimples();
            }

            ChecaSimboloEsperado(Simbolo.S_FIM);

            Lexico();
        }

        void AnalisaComandoSimples()
        {
            if (token.simbolo == Simbolo.S_IDENTIFICADOR)
                AnalisaAtribChProcedimento();
            else if (token.simbolo == Simbolo.S_SE)
                AnalisaSe();
            else if (token.simbolo == Simbolo.S_ENQUANTO)
                AnalisaEnquanto();
            else if (token.simbolo == Simbolo.S_LEIA)
                AnalisaLeia();
            else if (token.simbolo == Simbolo.S_ESCREVA)
                AnalisaEscreva();
            else if (token.simbolo == Simbolo.S_INICIO)
                AnalisaComandos();
            else
                throw new ExceptionSimboloInesperado("", token);
        }

        void AnalisaAtribChProcedimento()
        {
            Token old = token;

            Lexico();
            if (token.simbolo == Simbolo.S_ATRIBUICAO)
                AnalisaAtribuicao(old); // ja leu um identificador?
            else
                ChamadaProcedimento(old);
        }

        void ChamadaProcedimento(Token old)
        {
            SimboloInfo simbolo = semantico.tabelaSimbolo.Pesquisa(old.lexema);

            if (simbolo == null)
                throw new ExceptionVariavelNaoDeclarada("", token);

            //Lexico();
            //ChecaSimboloEsperado(Simbolo.S_IDENTIFICADOR);
           // Lexico();
        }

        void AnalisaAtribuicao(Token old)
        {
            SimboloInfo simbolo = semantico.tabelaSimbolo.Pesquisa(old.lexema);

            if (simbolo == null)
                throw new ExceptionVariavelNaoDeclarada("", token);


            Lexico();
            /*ChecaSimboloEsperado(Simbolo.S_IDENTIFICADOR);
            Lexico();
            ChecaSimboloInesperado(Simbolo.S_ATRIBUICAO);
            Lexico();*/
            semantico.ReinicializaPosFixa();
            SimboloTipo tipo2 = AnalisaExpressao();
            if(!MesmoTipo(simbolo.tipo,tipo2))
                throw new ExceptionTipoInvalido("", simbolo.tipo, tipo2, old); 

            semantico.FinalizaPosFixa();
        }

        void AnalisaSe()
        {
            Token old = token;
            Lexico();
            semantico.ReinicializaPosFixa();
            SimboloTipo tipo = AnalisaExpressao();
            if (!MesmoTipo(tipo, SimboloTipo.BOOLEANO))
                throw new ExceptionTipoInvalido("", SimboloTipo.BOOLEANO, tipo, old); 
            semantico.FinalizaPosFixa();
            ChecaSimboloEsperado(Simbolo.S_ENTAO);
            Lexico();
            AnalisaComandoSimples();
            if (token.simbolo == Simbolo.S_SENAO)
            {
                Lexico();
                AnalisaComandoSimples();
            }
        }

        void AnalisaEnquanto()
        {
            Lexico();
            semantico.ReinicializaPosFixa();
            AnalisaExpressao();
            semantico.FinalizaPosFixa();

            ChecaSimboloEsperado(Simbolo.S_FACA);

            Lexico();
            AnalisaComandoSimples();
        }

        SimboloTipo AnalisaExpressao()
        {
            SimboloTipo tipo1 = AnalisaExpressaoSimples();
          
            if (token.simbolo == Simbolo.S_MAIOR || 
                token.simbolo == Simbolo.S_MENOR || 
                token.simbolo == Simbolo.S_MAIOR_IG || 
                token.simbolo == Simbolo.S_MENOR_IG || 
                token.simbolo == Simbolo.S_DIF)
            {
                semantico.ParaPosFixa(token);
                Lexico();
                SimboloTipo tipo2 = AnalisaExpressaoSimples();

                if (!MesmoTipo(tipo1, tipo2))
                    throw new ExceptionTipoInvalido("", tipo1, tipo2, token);              
                return SimboloTipo.BOOLEANO;
            }

            return tipo1;

        }

        bool MesmoTipo(SimboloTipo tipo1, SimboloTipo tipo2)
        {
            if (tipo1 == SimboloTipo.INTEIRO || tipo1 == SimboloTipo.FUNCAO_INTEIRO)
                return tipo2 == SimboloTipo.INTEIRO || tipo2 == SimboloTipo.FUNCAO_INTEIRO;
            else if (tipo1 == SimboloTipo.BOOLEANO || tipo1 == SimboloTipo.FUNCAO_BOOLEANO)
                return tipo2 == SimboloTipo.BOOLEANO || tipo2 == SimboloTipo.FUNCAO_BOOLEANO;
            return false;
        }

        SimboloTipo AnalisaExpressaoSimples()
        {
            if (token.simbolo == Simbolo.S_MAIS)
            {
                Lexico();
            }

            if (token.simbolo == Simbolo.S_MENOS)
            {
                semantico.ParaPosFixa(new Token(Simbolo.S_NUMERO, "0", 0, 0));
                semantico.ParaPosFixa(token);
                Lexico();
            }

            SimboloTipo tipo1 = AnalisaTermo();

            while (token.simbolo == Simbolo.S_MAIS || token.simbolo == Simbolo.S_MENOS || token.simbolo == Simbolo.S_OU)
            {
                semantico.ParaPosFixa(token);
                Token old = token;      
                Lexico();
                SimboloTipo tipo2 = AnalisaTermo();

                if (old.simbolo == Simbolo.S_MAIS || old.simbolo == Simbolo.S_MENOS)
                {
                    if (!MesmoTipo(tipo1, SimboloTipo.INTEIRO))
                        throw new ExceptionTipoInvalido("", SimboloTipo.INTEIRO, tipo1, old);
                    if (!MesmoTipo(tipo2, SimboloTipo.INTEIRO))
                        throw new ExceptionTipoInvalido("", SimboloTipo.INTEIRO, tipo2, old);
                    return SimboloTipo.INTEIRO;
                }
                else if (old.simbolo == Simbolo.S_OU)
                {
                    if (!MesmoTipo(tipo1, SimboloTipo.BOOLEANO))
                        throw new ExceptionTipoInvalido("", SimboloTipo.BOOLEANO, tipo1, old);
                    if (!MesmoTipo(tipo2, SimboloTipo.BOOLEANO))
                        throw new ExceptionTipoInvalido("", SimboloTipo.BOOLEANO, tipo2, old);
                    return SimboloTipo.BOOLEANO;
                }
            }

            return tipo1;
        }

        SimboloTipo AnalisaFator()
        {
            semantico.ParaPosFixa(token);

            if (token.simbolo == Simbolo.S_IDENTIFICADOR)
            {
                return AnalisaChamadaFuncao();
            }
            else if (token.simbolo == Simbolo.S_NUMERO)
            {
                Lexico();
                return SimboloTipo.INTEIRO;
            }
            else if (token.simbolo == Simbolo.S_NAO)
            {
                Lexico();
                return AnalisaFator();
            }
            else if (token.simbolo == Simbolo.S_ABRE_PARENTESES)
            {
                Lexico();
                SimboloTipo tipo = AnalisaExpressao();
                ChecaSimboloEsperado(Simbolo.S_FECHA_PARENTESES);
                semantico.ParaPosFixa(token);
                Lexico();
                return tipo;
            }
            else if (token.lexema == "verdadeiro" || token.lexema == "falso")
            {
                Lexico();
                return SimboloTipo.BOOLEANO;
            }
            else throw new ExceptionSimboloInesperado("", token);
        }

        SimboloTipo AnalisaChamadaFuncao()
        {
            ChecaSimboloEsperado(Simbolo.S_IDENTIFICADOR);

            SimboloInfo simbolo = semantico.tabelaSimbolo.Pesquisa(token.lexema);

            if (simbolo == null)
                throw new ExceptionVariavelNaoDeclarada("", token);

            Lexico();
            return simbolo.tipo;
        }

        SimboloTipo AnalisaTermo()
        {
            SimboloTipo tipo1 = AnalisaFator();

            while (token.simbolo == Simbolo.S_MULT || token.simbolo == Simbolo.S_DIV || token.simbolo == Simbolo.S_E)
            {
                semantico.ParaPosFixa(token);
                Token old = token;      
                Lexico();
                SimboloTipo tipo2 = AnalisaFator();

                if (old.simbolo == Simbolo.S_MULT || old.simbolo == Simbolo.S_DIV)
                {
                    if (!MesmoTipo(tipo1, SimboloTipo.INTEIRO))
                        throw new ExceptionTipoInvalido("", SimboloTipo.INTEIRO, tipo1, old);
                    if (!MesmoTipo(tipo2, SimboloTipo.INTEIRO))
                        throw new ExceptionTipoInvalido("", SimboloTipo.INTEIRO, tipo2, old);
                    return SimboloTipo.INTEIRO;
                }
                else if (old.simbolo == Simbolo.S_E)
                {
                    if (!MesmoTipo(tipo1, SimboloTipo.BOOLEANO))
                        throw new ExceptionTipoInvalido("", SimboloTipo.BOOLEANO, tipo1, old);
                    if (!MesmoTipo(tipo2, SimboloTipo.BOOLEANO))
                        throw new ExceptionTipoInvalido("", SimboloTipo.BOOLEANO, tipo2, old);
                    return SimboloTipo.BOOLEANO;
                }
            }

            return tipo1;
        }

        void AnalisaLeia()
        {
            Lexico();
            ChecaSimboloEsperado(Simbolo.S_ABRE_PARENTESES);
            Lexico();
            ChecaSimboloEsperado(Simbolo.S_IDENTIFICADOR);

            if (!semantico.tabelaSimbolo.PesquisaVariavelInteiro(token.lexema))
                throw new ExceptionVariavelNaoDeclarada("", token);

            Lexico();
            ChecaSimboloEsperado(Simbolo.S_FECHA_PARENTESES);
            Lexico();
        }

        void AnalisaEscreva()
        {
            Lexico();
            ChecaSimboloEsperado(Simbolo.S_ABRE_PARENTESES);
            Lexico();
            ChecaSimboloEsperado(Simbolo.S_IDENTIFICADOR);

            if (!semantico.tabelaSimbolo.PesquisaVariavelInteiro(token.lexema) &&
                !semantico.tabelaSimbolo.PesquisaFuncaoInteiro(token.lexema))
            {
                SimboloInfo s = semantico.tabelaSimbolo.Pesquisa(token.lexema);

                if (s != null)
                    throw new ExceptionTipoInvalido("", SimboloTipo.INTEIRO, s.tipo, token);
                else
                    throw new ExceptionVariavelNaoDeclarada("", token);
            }

            Lexico();
            ChecaSimboloEsperado(Simbolo.S_FECHA_PARENTESES);
            Lexico();
        }

        void AnalisaVariaveis()
        {
            List<string> tmp = new List<string>();

            do
            {
                ChecaSimboloEsperado(Simbolo.S_IDENTIFICADOR);

                if (semantico.tabelaSimbolo.PesquisaDuplicidadeNoEscopo(token.lexema) ||
                    semantico.tabelaSimbolo.PesquisaDuplicidadeProcedimentoFuncao(token.lexema))
                    throw new ExceptionVariavelDuplicada("", token);

                tmp.Add(token.lexema);

                Lexico();
                if (token.simbolo == Simbolo.S_VIRGULA || token.simbolo == Simbolo.S_DOIS_PONTOS)
                {
                    if (token.simbolo == Simbolo.S_VIRGULA)
                    {
                        Lexico();
                        ChecaSimboloInesperado(Simbolo.S_DOIS_PONTOS);
                    }
                }

                if (lexico.FimDeArquivo() && token.simbolo != Simbolo.S_DOIS_PONTOS)
                    throw new ExceptionSimboloEsperado("Final de arquivo inesperado", Simbolo.S_DOIS_PONTOS, token);
            } while (token.simbolo != Simbolo.S_DOIS_PONTOS);

            Lexico();
            AnalisaTipo(tmp);
        }

        void AnalisaTipo(List<string> listaDeVar)
        {
            if (token.simbolo != Simbolo.S_INTEIRO && token.simbolo != Simbolo.S_BOOLEANO)
                throw new ExceptionSimboloInesperado("", token);

            if (token.simbolo == Simbolo.S_INTEIRO)
                semantico.tabelaSimbolo.Insere(listaDeVar, SimboloTipo.INTEIRO, false);
            else if (token.simbolo == Simbolo.S_BOOLEANO)
                semantico.tabelaSimbolo.Insere(listaDeVar, SimboloTipo.BOOLEANO, false);

            Lexico();
        }

        void ChecaSimboloEsperado(Simbolo s)
        {
            if (token.simbolo != s)
                throw new ExceptionSimboloEsperado("", s, token);
        }

        void ChecaSimboloInesperado(Simbolo s)
        {
            if (token.simbolo == s)
                throw new ExceptionSimboloInesperado("", token);
        }

        public void Lexico()
        {
            token = lexico.PegaToken();
        }
    }
}

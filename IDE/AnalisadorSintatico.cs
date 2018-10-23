using System;
using System.Collections.Generic;
using System.IO;

namespace Compilador
{
    class ExceptionEsperado : Exception
    {
        Token token;
        string message;
        Simbolo esperado;

        public ExceptionEsperado(string message, Simbolo esperado, Token token)
        {
            this.token = token;
            this.message = message;
            this.esperado = esperado;
            this.message = this.ToString();
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

            return String.Format("{2} {0}:{1} (Esperado '{3}' mas recebeu '{4}')", token.linha, token.coluna, message, lexema, token.lexema);
        }
    }

    class ExceptionInesperado : Exception
    {
        Token token;
        string message;

        public ExceptionInesperado(string message, Token token)
        {
            this.token = token;
            this.message = message;
            this.message = this.ToString();
        }

        public override string ToString()
        {
            return String.Format("{2} {0}:{1} ('{3}' inesperado')", token.linha, token.coluna, message, token.lexema);
        }
    }

    class AnalisadorSintatico
    {
        public StreamReader arquivo;
        public AnalisadorLexico lexico;
        public string caminhoArquivo;

        Token token = new Token();

        public AnalisadorSintatico(string caminhoArquivo)
        {
            this.caminhoArquivo = caminhoArquivo;
            arquivo = new StreamReader(new FileStream(caminhoArquivo, FileMode.Open));
            lexico = new AnalisadorLexico(arquivo);
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

            arquivo.Close();
        }

        public void Analisa()
        {
            Lexico();
            ChecaSimboloEsperado(Simbolo.S_PROGRAMA);
            Lexico();
            ChecaSimboloEsperado(Simbolo.S_IDENTIFICADOR);
            Lexico();
            ChecaSimboloEsperado(Simbolo.S_PONTO_VIRGULA);
            AnalisaBloco();
            ChecaSimboloEsperado(Simbolo.S_PONTO);

            try
            {
                Lexico();
                throw new ExceptionEsperado("Token inesperado apos final de programa", Simbolo.S_PONTO, token);
            }
            catch
            {
                Console.WriteLine("SUCESSO");
            }
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
            Lexico();
            ChecaSimboloEsperado(Simbolo.S_DOIS_PONTOS);
            Lexico();

            if (token.simbolo != Simbolo.S_INTEIRO && token.simbolo != Simbolo.S_BOOLEANO)
                throw new ExceptionInesperado("", token);

            Lexico();
            if (token.simbolo == Simbolo.S_PONTO_VIRGULA)
                AnalisaBloco();
        }

        void AnalisaDeclaracaoProcedimento()
        {
            Lexico();
            ChecaSimboloEsperado(Simbolo.S_IDENTIFICADOR);
            Lexico();
            ChecaSimboloEsperado(Simbolo.S_PONTO_VIRGULA);
            AnalisaBloco();
        }

        void AnalisaComandos()
        {
            /*if (token.simbolo == Simbolo.S_INICIO)
            {
                Lexico();
                AnalisaComandoSimples();

                while (token.simbolo != Simbolo.S_FIM)
                {
                    //if (token.simbolo == Simbolo.S_INICIO)
                    //  AnalisaComandos();
                    
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
                AnalisaComandoSimples();
            }

            ChecaSimboloEsperado(Simbolo.S_FIM);
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
        }

        void AnalisaAtribChProcedimento()
        {
            Lexico();
            if (token.simbolo == Simbolo.S_ATRIBUICAO)
                AnalisaAtribuicao(); // ja leu um identificador?
            else
                ChamadaProcedimento();
        }

        void ChamadaProcedimento()
        {
            Lexico();
            ChecaSimboloEsperado(Simbolo.S_IDENTIFICADOR);
            Lexico();
        }

        void AnalisaAtribuicao()
        {
            Lexico();
            /*ChecaSimboloEsperado(Simbolo.S_IDENTIFICADOR);
            Lexico();
            ChecaSimboloInesperado(Simbolo.S_ATRIBUICAO);
            Lexico();*/
            AnalisaExpressao();
        }

        void AnalisaSe()
        {
            Lexico();
            AnalisaExpressao();
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
            AnalisaExpressao();

            ChecaSimboloEsperado(Simbolo.S_FACA);

            Lexico();
            AnalisaComandoSimples();
        }

        void AnalisaExpressao()
        {
            AnalisaExpressaoSimples();

            if (token.simbolo == Simbolo.S_MAIOR || 
                token.simbolo == Simbolo.S_MENOR || 
                token.simbolo == Simbolo.S_MAIOR_IG || 
                token.simbolo == Simbolo.S_MENOR_IG || 
                token.simbolo == Simbolo.S_DIF)
            {
                Lexico();
                AnalisaExpressaoSimples();
            }
        }

        void AnalisaExpressaoSimples()
        {
            if (token.simbolo == Simbolo.S_MAIS || token.simbolo == Simbolo.S_MENOS)
                Lexico();                
            
            AnalisaTermo();

            while (token.simbolo == Simbolo.S_MAIS || token.simbolo == Simbolo.S_MENOS || token.simbolo == Simbolo.S_OU)
            {
                Lexico();
                AnalisaTermo();
            }
        }

        void AnalisaFator()
        {
            if (token.simbolo == Simbolo.S_IDENTIFICADOR)
            {
                AnalisaChamadaFuncao();
            }
            else if (token.simbolo == Simbolo.S_NUMERO)
            {
                Lexico();
            }
            else if (token.simbolo == Simbolo.S_NAO)
            {
                Lexico();
                AnalisaFator();
            }
            else if (token.simbolo == Simbolo.S_ABRE_PARENTESES)
            {
                Lexico();
                AnalisaExpressao();
                ChecaSimboloEsperado(Simbolo.S_FECHA_PARENTESES);
                Lexico();
            }
            else if (token.lexema == "verdadeiro" || token.lexema == "falso")
            {
                Lexico();
            }
            else throw new ExceptionInesperado("", token);
        }

        void AnalisaChamadaFuncao()
        {
            ChecaSimboloEsperado(Simbolo.S_IDENTIFICADOR);
            Lexico();
        }

        void AnalisaTermo()
        {
            AnalisaFator();

            while (token.simbolo == Simbolo.S_MULT || token.simbolo == Simbolo.S_DIV || token.simbolo == Simbolo.S_E)
            {
                Lexico();
                AnalisaFator();
            }
        }

        void AnalisaLeia()
        {
            Lexico();
            ChecaSimboloEsperado(Simbolo.S_ABRE_PARENTESES);
            Lexico();
            ChecaSimboloEsperado(Simbolo.S_IDENTIFICADOR);
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
            Lexico();
            ChecaSimboloEsperado(Simbolo.S_FECHA_PARENTESES);
            Lexico();
        }

        void AnalisaVariaveis()
        {
            do
            {
                ChecaSimboloEsperado(Simbolo.S_IDENTIFICADOR);

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
                    throw new ExceptionEsperado("Final de arquivo inesperado", Simbolo.S_DOIS_PONTOS, token);
            } while (token.simbolo != Simbolo.S_DOIS_PONTOS);

            Lexico();
            AnalisaTipo();
        }

        void AnalisaTipo()
        {
            if (token.simbolo != Simbolo.S_INTEIRO && token.simbolo != Simbolo.S_BOOLEANO)
                throw new ExceptionInesperado("", token);

            Lexico();
        }

        void ChecaSimboloEsperado(Simbolo s)
        {
            if (token.simbolo != s)
                throw new ExceptionEsperado("", s, token);
        }

        void ChecaSimboloInesperado(Simbolo s)
        {
            if (token.simbolo == s)
                throw new ExceptionInesperado("", token);
        }

        public void Lexico()
        {
            token = lexico.PegaToken();
        }
    }
}

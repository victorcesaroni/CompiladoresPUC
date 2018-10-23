using System;
using System.Collections.Generic;
using System.IO;

namespace Compilador
{
    enum Simbolo
    {
        S_PROGRAMA,
        S_INICIO,
        S_FIM,
        S_PROCEDIMENTO,
        S_FUNCAO,
        S_SE,
        S_ENTAO,
        S_SENAO,
        S_ENQUANTO,
        S_FACA,
        S_ATRIBUICAO,
        S_ESCREVA,
        S_LEIA,
        S_VAR,
        S_INTEIRO,
        S_BOOLEANO,
        S_IDENTIFICADOR,
        S_NUMERO,
        S_PONTO,
        S_PONTO_VIRGULA,
        S_VIRGULA,
        S_ABRE_PARENTESES,
        S_FECHA_PARENTESES,
        S_MAIOR,
        S_MAIOR_IG,
        S_IG,
        S_MENOR,
        S_MENOR_IG,
        S_DIF,
        S_MAIS,
        S_MENOS,
        S_MULT,
        S_DIV,
        S_E,
        S_OU,
        S_NAO,
        S_DOIS_PONTOS,
    };

    class Token
    {
        public Token()
        {

        }

        public Token(Simbolo simbolo, string lexema, ulong linha, ulong coluna)
        {
            this.linha = linha;
            this.coluna = coluna;
            this.lexema = lexema;
            this.simbolo = simbolo;
        }

        public Simbolo simbolo;
        public string lexema;
        public ulong linha, coluna;
    }

    class ExceptionErroLexical : Exception
    {
        public Token token;
        public ExceptionErroLexical(Token token)
        {
            this.token = token;
        }
    }

    class AnalisadorLexico
    {
        public static Dictionary<string, Simbolo> mapaDeSimbolo = new Dictionary<string, Simbolo>() {
            { "programa", Simbolo.S_PROGRAMA },
            { "inicio", Simbolo.S_INICIO },
            { "fim", Simbolo.S_FIM },
            { "procedimento", Simbolo.S_PROCEDIMENTO },
            { "funcao", Simbolo.S_FUNCAO },
            { "se", Simbolo.S_SE },
            { "entao", Simbolo.S_ENTAO },
            { "senao", Simbolo.S_SENAO },
            { "enquanto", Simbolo.S_ENQUANTO },
            { "faca", Simbolo.S_FACA },
            { ":=", Simbolo.S_ATRIBUICAO },
            { "escreva", Simbolo.S_ESCREVA },
            { "leia", Simbolo.S_LEIA },
            { "var", Simbolo.S_VAR },
            { "inteiro", Simbolo.S_INTEIRO },
            { "booleano", Simbolo.S_BOOLEANO },
            { "identificador", Simbolo.S_IDENTIFICADOR },
            { "numero", Simbolo.S_NUMERO },
            { ".", Simbolo.S_PONTO },
            { ";", Simbolo.S_PONTO_VIRGULA },
            { ",", Simbolo.S_VIRGULA },
            { "(", Simbolo.S_ABRE_PARENTESES },
            { ")", Simbolo.S_FECHA_PARENTESES },
            { ">", Simbolo.S_MAIOR },
            { ">=", Simbolo.S_MAIOR_IG },
            { "=", Simbolo.S_IG },
            { "<", Simbolo.S_MENOR },
            { "<=", Simbolo.S_MENOR_IG },
            { "!=", Simbolo.S_DIF },
            { "+", Simbolo.S_MAIS },
            { "-", Simbolo.S_MENOS },
            { "*", Simbolo.S_MULT },
            { "div", Simbolo.S_DIV },
            { "e", Simbolo.S_E },
            { "ou", Simbolo.S_OU },
            { "nao", Simbolo.S_NAO },
            { ":", Simbolo.S_DOIS_PONTOS },
        };

        public StreamReader arquivo;
        ulong linha, coluna;
        public char c;
        bool firstTime;

        public AnalisadorLexico(StreamReader arquivo)
        {
            this.arquivo = arquivo;

            linha = 1;
            coluna = 0;

            firstTime = true;
        }
        
        public Token PegaToken()
        {
            if (firstTime)
            {
                c = Ler();
                firstTime = false;
            }

            if (!FimDeArquivo())
            {
                while ((c == '{' || VerificaEspaco(c)) && !FimDeArquivo())
                {
                    if (c == '{')
                    {
                        while (c != '}')
                        {
                            c = Ler();

                            if (FimDeArquivo() && c != '}')
                            {
                                throw new Exception(String.Format("Erro léxico L:{0} C:{1}", linha, coluna));
                            }
                        }

                        c = Ler();
                    }

                    while (VerificaEspaco(c) && !FimDeArquivo())
                    {
                        c = Ler();
                    }
                }

                if (!FimDeArquivo())
                {
                    var t = CriaToken();
                    return t;
                }
            }

            throw new Exception("Nao foi possivel pegar proximo token");
        }
        public Token CriaToken()
        {
            if (VerificaDigito(c))
                return TrataDigito();
            else if (VerificaLetra(c))
                return TrataIdentificadorPalavraReservada();
            else if (VerificaAtribuicao(c))
                return TrataAtribuicao();
            else if (VerificaOperadorAritmetico(c))
                return TrataOperadorAritimetico();
            else if (VerificaOperadorRelacional(c))
                return TrataOperadorRelacional();
            else if (VerificaPontuacao(c))
                return TrataPontuacao();

            throw new Exception(String.Format("Erro léxico L:{0} C:{1}", linha, coluna));
        }
        public Token TrataPontuacao()
        {
            Token token = new Token(mapaDeSimbolo[c.ToString()], c.ToString(), linha, coluna);
            c = Ler();
            return token;
        }

        public Token TrataOperadorRelacional()
        {
            Token token = new Token();
            token.linha = linha;
            token.coluna = coluna;
            token.lexema = c.ToString();

            if (c == '>')
            {
                c = Ler();
                if (c == '=')
                {
                    token.lexema += c.ToString();
                    token.simbolo = Simbolo.S_MAIOR_IG;
                    c = Ler();
                }
                else
                {
                    token.simbolo = Simbolo.S_MAIOR;
                }
            }
            else if (c == '<')
            {
                c = Ler();
                if (c == '=')
                {
                    token.lexema += c.ToString();
                    token.simbolo = Simbolo.S_MENOR_IG;
                    c = Ler();
                }
                else
                {
                    token.simbolo = Simbolo.S_MENOR;
                }
            }
            else if (c == '!')
            {
                c = Ler();
                if (c == '=')
                {
                    token.lexema += c.ToString();
                    token.simbolo = Simbolo.S_DIF;
                    c = Ler();
                }
                else
                {
                    throw new Exception(String.Format("Erro léxico L:{0} C:{1}", linha, coluna));
                }
            }
            else if (c == '=')
            {
                token.lexema += c.ToString();
                token.simbolo = Simbolo.S_IG;
                c = Ler();
            }

            return token;
        }

        public Token TrataOperadorAritimetico()
        {
            Token token = new Token(mapaDeSimbolo[c.ToString()], c.ToString(), linha, coluna);
            c = Ler();
            return token;
        }

        public Token TrataAtribuicao()
        {
            string atrib = c.ToString();

            c = Ler();

            if (c == '=')
            {
                atrib += c.ToString();
                c = Ler();

                return new Token(Simbolo.S_ATRIBUICAO, atrib, linha, coluna);
            }

            return new Token(Simbolo.S_DOIS_PONTOS, atrib, linha, coluna);
        }

        public Token TrataIdentificadorPalavraReservada()
        {
            string id = c.ToString();

            c = Ler();

            while (VerificaLetra(c) || VerificaDigito(c) || c == '_')
            {
                id += c.ToString();
                c = Ler();
            }

            Simbolo s = Simbolo.S_IDENTIFICADOR;

            try
            {
                s = mapaDeSimbolo[id];
            }
            catch { }

            return new Token(s, id, linha, coluna);
        }

        public Token TrataDigito()
        {
            string num = c.ToString();
            c = Ler();

            while (VerificaDigito(c))
            {
                num += c.ToString();
                c = Ler();
            }

            return new Token(Simbolo.S_NUMERO, num, linha, coluna); ;
        }

        public bool VerificaAtribuicao(char c)
        {
            return c == ':';
        }

        public bool VerificaPontuacao(char c)
        {
            return c == ';' || c == ',' || c == '(' || c == ')' || c == '.';
        }

        public bool VerificaOperadorRelacional(char c)
        {
            return c == '>' || c == '<' || c == '!' ||  c == '=';
        }

        public bool VerificaOperadorAritmetico(char c)
        {
            return c == '+' || c == '-' || c == '*';
        }

        public bool VerificaDigito(char c)
        {
            return c >= '0' && c <= '9';
        }

        public bool VerificaLetra(char c)
        {
            return c >= 'A' && c <= 'Z' || c >= 'a' && c <= 'z';
        }

        public bool VerificaEspaco(char c)
        {
            switch (c)
            {
                case ' ': return true;
                case '\r': return true;
                case '\n': return true;
                case '\t': return true;
            }
            return false;
        }

        private char Ler()
        {
            char c = (char)arquivo.Read();

            coluna++;
            if (c == '\n')
            {
                coluna = 0;
                linha++;
            }

            return c;
        }

        public bool FimDeArquivo()
        {
            return arquivo.EndOfStream;
        }
    }
}
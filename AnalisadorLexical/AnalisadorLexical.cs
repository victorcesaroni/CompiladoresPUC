﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AnalisadorLexical
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
        Simbolo simbolo;
        string lexema;
        ulong linha, coluna;
    }

    class ExceptionErroLexical : Exception
    {
        Token token;
        public ExceptionErroLexical(Token token)
        {
            this.token = token;
        }
    }

    class AnalisadorLexical
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
        int linha, coluna;
        public List<Token> tokens;

        public AnalisadorLexical(string caminhoArquivo)
        {
            arquivo = new StreamReader(new FileStream(caminhoArquivo, FileMode.Open));

            char c = Ler();

            while (!FimDeArquivo())
            {
                switch (c)
                {
                    case ' ': c = Ler(); break;
                    case '\r': c = Ler(); break;
                    case '\n': c = Ler(); break;
                    case '\t': c = Ler(); break;
                    case '{':
                        c = Ler();
                        while (c != '}' && !FimDeArquivo())
                            c = Ler();
                        if (c != '}' || FimDeArquivo())
                            throw new Exception("nao achou fecha chave");
                        break;
                }

                if (!FimDeArquivo())
                {
                    tokens.Add(PegaToken(c));
                }
            }
        }

        public Token PegaToken(char c)
        {
            if (VerificaDigito(c))
                return TrataDigito(c);
            else if (VerificaLetra(c))
                return TrataIdentificadorPalavraReservada(c);
            else if (VerificaAtribuicao(c))
                return TrataAtribuicao(c);
            else if (VerificaOperadorAritmetico(c))
                return TrataOperadorAritimetico(c);
            else if (VerificaOperadorRelacional(c))
                return TrataOperadorRelacional(c);
            else if (VerificaPontuacao(c))
                return TrataPontuacao(c);
            
            throw new Exception(String.Format("Erro léxico L:{0} C:{1}", linha, coluna));
        }
        public Token TrataPontuacao(char c)
        {
            throw new Exception("NAO IMPLEMENTADO");
        }

        public Token TrataOperadorRelacional(char c)
        {
            throw new Exception("NAO IMPLEMENTADO");
        }

        public Token TrataOperadorAritimetico(char c)
        {
            throw new Exception("NAO IMPLEMENTADO");
        }

        public Token TrataAtribuicao(char c)
        {
            throw new Exception("NAO IMPLEMENTADO");
        }

        public Token TrataIdentificadorPalavraReservada(char c)
        {
            throw new Exception("NAO IMPLEMENTADO");
        }

        public Token TrataDigito(char c)
        {
            throw new Exception("NAO IMPLEMENTADO");
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
            return c == '>' || c == '<' || c == '=';
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

        private char Ler()
        {
            char c = (char)arquivo.Read();

            coluna++;
            if (c == '\n')
                linha++;

            return c;
        }

        private bool FimDeArquivo()
        {
            return arquivo.EndOfStream;
        }
    }
}
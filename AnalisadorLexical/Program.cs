using System;

namespace Compilador
{
    class Program
    {
        static void Main(string[] args)
        {
            AnalisadorSintatico a = new AnalisadorSintatico(@"C:\Users\15593866\Documents\compiladores2018\AnalisadorLexical\test.txt");

            Console.WriteLine("Pressione qualquer tecla para continuar");
            Console.ReadLine();
        }
    }
}

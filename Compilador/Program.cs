using System;

namespace Compilador
{
    class Program
    {
        static void Main(string[] args)
        {
            AnalisadorSintatico a = new AnalisadorSintatico("test.txt");

            Console.WriteLine("Pressione qualquer tecla para continuar");
            Console.ReadLine();
        }
    }
}

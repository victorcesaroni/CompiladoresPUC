using System;

namespace AnalisadorLexical
{
    class Program
    {
        static void Main(string[] args)
        {
            AnalisadorLexical a = new AnalisadorLexical(@"C:\Users\15593866\Documents\compiladores2018\AnalisadorLexical\test.txt");

            foreach(var t in a.tokens)
            {
                Console.WriteLine(String.Format("{2} #{3} [{0}:{1}] ", t.linha, t.coluna, t.lexema, t.simbolo.ToString()));
            }

            Console.ReadLine();
        }
    }
}

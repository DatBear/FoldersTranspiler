using System;
using System.Collections;
using System.Linq;
using System.Text;

namespace HelloWorld
{
    class Program
    {
        static void Main(string[] args)
        {
            
            Console.Write("Hello, world!");
            string a = "Hello, world!";
            a = Console.ReadLine();
            string c = a + 4;
            string b = a;
            int d = 4;
            int e = d - 5 + 6;
            d = e + 5;

            if (d == e+5)
            {
                Console.Write("if");
            }

            Console.Write(e);

            while (1 == 1)
            {
                Console.Write("while ");
            }
        }
    }
}
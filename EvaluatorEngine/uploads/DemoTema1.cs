using System;
using System.IO;

namespace DemoTema1
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] linie1Split;
            string[] linie2Split;

            int suma = 0;

            using (StreamReader sr = File.OpenText(args[0]))
            {
                linie1Split = sr.ReadLine().Split(',');

                linie2Split = sr.ReadLine().Split(',');
            }

            for (int i = 0; i < linie1Split.Length; i++)
            {
                suma += int.Parse(linie1Split[i]) * int.Parse(linie2Split[i]);
            }

            Console.WriteLine(suma);
        }
    }
}
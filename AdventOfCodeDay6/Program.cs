using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Linq;

namespace AdventOfCode
{
    class Program
    {

        static void Main(string[] args)
        {
            List<string> columns = new List<string>() { "", "", "", "", "", "", "", "" };
            string text = System.IO.File.ReadAllText(@"..\..\..\Input\Day6.txt");
            var input = text.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var item in input)
            {
                for (int i = 0; i < item.Length; i++)
                {
                    columns[i] += item[i];
                }
            }

            foreach (var item in columns)
            {
                Console.Write(item.GroupBy(x => x).OrderBy(x => x.Count()).First().Key);
            }

            Console.WriteLine();
            Console.ReadKey();
        }
        
    }
}

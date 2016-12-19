using System;
using System.Collections.Generic;

namespace AdventOfCodeDay19
{
    class Program
    {
        static void Main(string[] args)
        {
            var elvesAmount = 3017957;
            var elves = new List<int>();

            for (int i = 1; i <= elvesAmount; i++)
            {
                elves.Add(i);
            }

            while (elves.Count > 1)
            {
                for (int i = 0; i < elves.Count; i++)
                {
                    var nextElfOffset = elves.Count /2;
                    var nextElf = i + nextElfOffset;

                    if (i + nextElfOffset >= elves.Count)
                    {
                        nextElf = nextElf - elves.Count;
                        i--;
                    }

                    elves.RemoveAt(nextElf);
                }
            }

            Console.WriteLine(elves[0]);
            Console.ReadKey();
        }
    }
}

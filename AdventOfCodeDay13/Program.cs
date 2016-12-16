namespace AdventOfCodeDay13
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Cryptography.X509Certificates;

    class Program
    {
        private static int locations = 0;
        private static int areaWidth = 45;
        private static int areaHeight = 45;
        private static List<int> stepSolutions = new List<int>();
        private static List<KeyValuePair<int, int>> location = new List<KeyValuePair<int, int>>();

        static void Main(string[] args)
        {
            
            var area = new byte[areaHeight, areaWidth];

            for (int i = 0; i < areaHeight; i++)
            {
                for (int j = 0; j < areaWidth; j++)
                {
                    if (IsOpenSpace(j, i))
                    {
                        area[j, i] = 0;
                    }
                    else
                    {
                        area[j, i] = 1;
                    }
                }
            }

            //for (int i = 0; i < 50; i++)
            //{
            //    for (int j = 0; j < 50; j++)
            //    {
            //        if (j == 31 && i == 39)
            //        {
            //            Console.Write("0");
            //        }
            //        else if (area[j, i] == 0)
            //        {
            //            Console.Write("-");
            //        }
            //        else
            //        {
            //            Console.Write("█");
            //        }
            //    }

            //    Console.WriteLine();
            //}
            //Console.WriteLine();

            FindPath(area, 1, 1, 0, new List<KeyValuePair<int, int>>());

            foreach (var solution in stepSolutions)
            {
                Console.WriteLine(solution);
            }

            Console.WriteLine("===============");

            location = location.Distinct().ToList();
            foreach (var l in location)
            {
                Console.WriteLine(l);
            }
            Console.WriteLine(location.Count);

            Console.ReadKey();
        }

        private static void FindPath(byte[,] area, int currentJ, int currentI, int steps, List<KeyValuePair<int, int>> previous)
        {
            //Console.Clear();

            //for (int i = 0; i < areaHeight; i++)
            //{
            //    for (int j = 0; j < areaWidth; j++)
            //    {
            //        if (j == 31 && i == 39)
            //        {
            //            Console.Write("0");
            //        }
            //        else if (j == currentJ && i == currentI)
            //        {
            //            Console.Write("0");
            //        }
            //        else if (area[j, i] == 0)
            //        {
            //            Console.Write("-");
            //        }
            //        else
            //        {
            //            Console.Write("█");
            //        }
            //    }

            //    Console.WriteLine();

            //}

            //Console.WriteLine(steps);

            //Console.ReadKey();

            if (currentJ == 31 && currentI == 39)
            {
                //for (int i = 0; i < 50; i++)
                //{
                //    for (int j = 0; j < 50; j++)
                //    {
                //        if (j == currentJ && i == currentI)
                //        {
                //            Console.Write("0");
                //        }
                //        else if (area[j, i] == 0)
                //        {
                //            Console.Write("-");
                //        }
                //        else
                //        {
                //            Console.Write("█");
                //        }
                //    }

                //    Console.WriteLine();
                //}

                //Console.ReadKey();

                stepSolutions.Add(steps);
            }

            if (steps <= 50)
            {
                location.Add(new KeyValuePair<int, int>(currentJ, currentI));
            }

            previous.Add(new KeyValuePair<int, int>(currentJ, currentI));

            if (currentJ + 1 < areaWidth &&
                area[currentJ + 1, currentI] == 0 && 
                !previous.Contains(new KeyValuePair<int, int>(currentJ + 1, currentI)))
            {
                FindPath(area, currentJ + 1, currentI, steps + 1, new List<KeyValuePair<int, int>>(previous));
            }

            if (currentI + 1 < areaHeight &&
                area[currentJ, currentI + 1] == 0 &&
                !previous.Contains(new KeyValuePair<int, int>(currentJ, currentI + 1)))
            {
                FindPath(area, currentJ, currentI + 1, steps + 1, new List<KeyValuePair<int, int>>(previous));
            }

            if (currentJ - 1 >= 0 && 
                area[currentJ - 1, currentI] == 0 &&
                !previous.Contains(new KeyValuePair<int, int>(currentJ - 1, currentI)))
            {
                FindPath(area, currentJ - 1, currentI, steps + 1, new List<KeyValuePair<int, int>>(previous));
            }
            
            if (currentI - 1 >= 0 &&
                area[currentJ, currentI - 1] == 0 &&
                !previous.Contains(new KeyValuePair<int, int>(currentJ, currentI - 1)))
            {
                FindPath(area, currentJ, currentI - 1, steps + 1, new List<KeyValuePair<int, int>>(previous));
            }
        }

        private static bool IsOpenSpace(int x, int y)
        {
            var favouriteNumber = 1352;
            var finding = x*x + 3*x + 2*x*y + y + y*y + favouriteNumber;
            var bits = GetIntBinaryString(finding).Count(f => f == '1');
            return !IsOdd(bits);
        }

        static string GetIntBinaryString(int n)
        {
            char[] b = new char[32];
            int pos = 31;
            int i = 0;

            while (i < 32)
            {
                if ((n & (1 << i)) != 0)
                {
                    b[pos] = '1';
                }
                else
                {
                    b[pos] = '0';
                }
                pos--;
                i++;
            }
            return new string(b);
        }

        public static bool IsOdd(int value)
        {
            return value % 2 != 0;
        }
    }
}

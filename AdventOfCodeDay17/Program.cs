using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCodeDay17
{
    class Program
    {
        public class Step
        {
            public Position CurrenPosition;
            public string Passcode;

            public Step(Position currentPosition, string passcode)
            {
                CurrenPosition = currentPosition;
                Passcode = passcode;
            }
        }

        public class Position
        {
            public int CurrentI;
            public int CurrentJ;

            public Position(int currentI, int currentJ)
            {
                CurrentI = currentI;
                CurrentJ = currentJ;
            }
        }

        public static List<char> _validHashChars = new List<char> { 'b', 'c', 'd', 'e', 'f' };
        public static List<char> _walls = new List<char> { '|', '-' };

        public static int _rows;
        public static int _columns;

        public static string _initialPasscode = "vkjiggvb";

        static void Main(string[] args)
        {
            string text = System.IO.File.ReadAllText(@"..\..\..\Input\Day17.txt");
            var input = text.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            _rows = input.Length;
            _columns = input[0].Length;

            var map = new char[_rows, _columns];

            var currentPosition = new Position(0, 0);

            for (int i = 0; i < _rows; i++)
            {
                for (int j = 0; j < _columns; j++)
                {
                    if (input[i][j] == 'S')
                    {
                        currentPosition.CurrentI = i;
                        currentPosition.CurrentJ = j;
                    }
                    else
                    {
                        map[i, j] = input[i][j];
                    }
                }
            }

            var stepAgenda = new Queue<Step>();
            stepAgenda.Enqueue(new Step(currentPosition, _initialPasscode));

            var allPossiblePaths = new List<string>();

            while (stepAgenda.Count != 0)
            {
                var step = stepAgenda.Dequeue();

                //OutputMap(map, step.CurrenPosition, step.Passcode);

                if (map[step.CurrenPosition.CurrentI + 1, step.CurrenPosition.CurrentJ + 1] == 'V')
                {
                    var shortestPath = step.Passcode.Replace(_initialPasscode, "");
                    //Console.WriteLine(shortestPath);
                    //Console.WriteLine(shortestPath.Length);
                    allPossiblePaths.Add(shortestPath);
                }
                else
                {
                    var newSteps = GetPossibleSteps(step.CurrenPosition, map, step.Passcode);
                    foreach (var newStep in newSteps)
                    {
                        stepAgenda.Enqueue(newStep);
                    }
                }
            }

            Console.WriteLine("Longest path: " + allPossiblePaths.Max(p => p.Length));
            Console.ReadKey();
        }
        
        private static List<Step> GetPossibleSteps(Position currentPosition, char[,] map, string passcode)
        {
            var nextSteps = new List<Step>();
            var hash = CreateMD5(passcode);
            
            var newPosition = new Position(currentPosition.CurrentI + 2, currentPosition.CurrentJ);
            if (newPosition.CurrentI <= _rows - 1 &&
                _validHashChars.Contains(hash[1]) && _walls.Contains(map[currentPosition.CurrentI + 1, currentPosition.CurrentJ]))
            {
                nextSteps.Add(new Step(newPosition, string.Concat(passcode, "D")));
            }

            newPosition = new Position(currentPosition.CurrentI, currentPosition.CurrentJ + 2);
            if (newPosition.CurrentJ <= _columns - 1 &&
                _validHashChars.Contains(hash[3]) && _walls.Contains(map[currentPosition.CurrentI, currentPosition.CurrentJ + 1]))
            {
                nextSteps.Add(new Step(newPosition, string.Concat(passcode, "R")));
            }

            newPosition = new Position(currentPosition.CurrentI - 2, currentPosition.CurrentJ);
            if (newPosition.CurrentI >= 0 &&
                _validHashChars.Contains(hash[0]) && _walls.Contains(map[currentPosition.CurrentI - 1, currentPosition.CurrentJ]))
            {
                nextSteps.Add(new Step(newPosition, string.Concat(passcode, "U")));
            }

            newPosition = new Position(currentPosition.CurrentI, currentPosition.CurrentJ - 2);
            if (newPosition.CurrentJ >= 0 &&
                _validHashChars.Contains(hash[2]) && _walls.Contains(map[currentPosition.CurrentI, currentPosition.CurrentJ - 1]))
            {
                nextSteps.Add(new Step(newPosition, string.Concat(passcode, "L")));
            }

            return nextSteps;
        }

        private static void OutputMap(char[,] map, Position currentPosition, string passcode)
        {
            for (int i = 0; i < _rows; i++)
            {
                for (int j = 0; j < _columns; j++)
                {
                    if (i == currentPosition.CurrentI && j == currentPosition.CurrentJ)
                    {
                        Console.Write('S');
                    }
                    else
                    {
                        Console.Write(map[i, j]);
                    }
                }

                Console.WriteLine();
            }

            Console.WriteLine();
            Console.WriteLine(passcode);
            Console.WriteLine();
            Console.ReadKey();
        }

        public static string CreateMD5(string input)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString().ToLower();
            }
        }
    }
}

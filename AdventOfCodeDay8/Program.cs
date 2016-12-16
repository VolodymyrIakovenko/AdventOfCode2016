using System;
using System.Linq;

namespace AdventOfCode
{
    using System.Threading;

    class Program
    {
        static void Main(string[] args)
        {
            var rows = 6;
            var columns = 50;

            var screen = new byte[rows, columns];

            var text = System.IO.File.ReadAllText(@"..\..\..\Input\Day8.txt");
            var inputArray = text.Split(new char[] {'\r', '\n'}, StringSplitOptions.RemoveEmptyEntries);

            foreach (var input in inputArray)
            {
                Console.WriteLine(input);

              //  Console.ReadKey();

                if (input.StartsWith("rect"))
                {
                    var dimensions = input.Replace("rect ", "").Split('x');

                    for (var y = 0; y < int.Parse(dimensions[1]); y++)
                    {
                        for (var x = 0; x < int.Parse(dimensions[0]); x++)
                        {
                            screen[y, x] = 1;
                        }
                    }
                }
                else if (input.StartsWith("rotate"))
                {
                    var rotation = input.Replace("rotate ", "");
                    if (rotation.StartsWith("row"))
                    {
                        var rotationValues = rotation.Replace("row y=", "").Replace("by ", "").Split(' ');
                        var newRow = new byte[columns];

                        for (var x = columns - 1; x >= 0; x--)
                        {
                            if (screen[int.Parse(rotationValues[0]), x] == 1)
                            {
                                if (x + int.Parse(rotationValues[1]) > columns - 1)
                                {
                                    //Console.WriteLine(" => " + (x + int.Parse(rotationValues[1])) % columns);
                                    newRow[(x + int.Parse(rotationValues[1])) % columns] = 1;
                                }
                                else
                                {
                                    newRow[x + int.Parse(rotationValues[1])] = 1;
                                }
                            }
                        }

                        for (int i = 0; i < columns; i++)
                        {
                            screen[int.Parse(rotationValues[0]), i] = newRow[i];
                        }

                    }
                    else if (rotation.StartsWith("column"))
                    {
                        var rotationValues = rotation.Replace("column x=", "").Replace("by ", "").Split(' ');
                        var newColumn = new byte[6];

                        for (var y = rows - 1; y >= 0; y--)
                        {
                            if (screen[y, int.Parse(rotationValues[0])] == 1)
                            {

                                if (y + int.Parse(rotationValues[1]) > rows - 1)
                                {
                                    newColumn[(y + int.Parse(rotationValues[1])) % rows] = 1;
                                }
                                else
                                {
                                    newColumn[y + int.Parse(rotationValues[1])] = 1;
                                }
                            }
                        }

                        for (int i = 0; i < rows; i++)
                        {
                            screen[i, int.Parse(rotationValues[0])] = newColumn[i];
                        }
                    }
                }

                // Thread.Sleep(1000);


                Console.Clear();

                for (int y = 0; y < rows; y++)
                {
                    for (int x = 0; x < columns; x++)
                    {
                        if (screen[y, x] == 1)
                        {
                            Console.Write("█");
                        }
                        else
                        {
                            Console.Write(".");
                        }
                    }
                    Console.WriteLine();
                }



            }
            Console.Clear();
            var counter = 0;
            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < columns; x++)
                {
                    if (screen[y, x] == 1)
                    {
                        counter++;
                        Console.Write("█");
                    }
                    else
                    {
                        Console.Write(" ");
                    }

                    //Console.Write(screen[y, x]);

                }
                Console.WriteLine();
            }

            Console.WriteLine(counter);

            Console.ReadKey();
        }
    }
}

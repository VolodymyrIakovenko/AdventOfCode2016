using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCodeDay22
{
    using System.Runtime.InteropServices;

    class Program
    {
        public class Position
        {
            public int X;
            public int Y;

            public Position(int x, int y)
            {
                X = x;
                Y = y;
            }

            public override string ToString()
            {
                return "[" + X + "," + Y + "]";
            }
        }

        public class Node
        {
            public Position Position;
            public int Size;
            public int Used;
            public int Available => Size - Used;
            public int Percentage;

            public Node(Position position, int size, int used, int percentage)
            {
                Position = position;
                Size = size;
                Used = used;
                Percentage = percentage;
            }

            public override string ToString()
            {
                return Position + " " + Used + "/" + Size;
            }
        }

        public class State
        {
            public Node[,] Grid;
            public Position CurrentPosition;
            public Position ClosestViablePosition;
            public int Steps;

            public State(Node[,] grid, Position currentPosition, Position closestViablePosition, int steps)
            {
                Grid = grid;
                CurrentPosition = currentPosition;
                ClosestViablePosition = closestViablePosition;
                Steps = steps;
            }
            
            public bool IsSuccess()
            {
                return CurrentPosition.X == 0 && CurrentPosition.Y == 0;
            }

            public override string ToString()
            {
                return CurrentPosition + "; " + ClosestViablePosition;
            }
        }

        static void Main(string[] args)
        {
            var text = System.IO.File.ReadAllText(@"..\..\..\Input\Day22.txt");
            var input = text.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).Skip(2);

            var nodes = new List<Node>();

            foreach (var line in input)
            {
                var items = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                var name = items[0].Replace("/dev/grid/node-", "");
                var parsedPosition = name.Split('-');
                var position = new Position(int.Parse(parsedPosition[0].Replace("x", " ")), int.Parse(parsedPosition[1].Replace("y", " ")));

                var size = int.Parse(items[1].Replace("T", ""));
                var used = int.Parse(items[2].Replace("T", ""));
                //var available = int.Parse(items[3].Replace("T", ""));
                var percentage = int.Parse(items[4].Replace("%", ""));

                nodes.Add(new Node(position, size, used, percentage));
            }
            
            var maxY = nodes.Max(n => n.Position.Y) + 1;
            var maxX = nodes.Max(n => n.Position.X) + 1;

            var grid = new Node[maxX, maxY];
            foreach (var node in nodes)
            {
                grid[node.Position.X, node.Position.Y] = node;
            }

            var currentPosition = new Position(maxX - 1, 0);
            //Output(grid, maxY, maxX, currentPosition);

            var stateAgenda = new Queue<State>();

            var closestEmptyPosition = GetClosestViablePosition(grid, maxY, maxX, currentPosition);
            stateAgenda.Enqueue(new State(grid, currentPosition, closestEmptyPosition, 0));

            var visitedStates = new List<string>();

            var currentStep = 0;
            while (true)
            {
                var state = stateAgenda.Dequeue(); 

                if (visitedStates.Contains(state.CurrentPosition + " " + state.ClosestViablePosition))
                {
                    continue;
                }

                //Output(state.Grid, maxY, maxX, state.CurrentPosition);
                //Console.WriteLine(state.Steps);
                //Console.WriteLine();

                if (state.Steps > currentStep)
                {
                    currentStep = state.Steps;
                    Console.WriteLine(currentStep);
                }

                visitedStates.Add(state.CurrentPosition + " " + state.ClosestViablePosition);

                if (state.IsSuccess())
                {
                    Console.WriteLine(state.Steps);
                    break;
                }

                var nextStates = GetNextStates(state, maxY, maxX);
                foreach (var nextState in nextStates)
                {
                    stateAgenda.Enqueue(nextState);
                }
            }

            Console.ReadKey();
        }

        private static List<State> GetNextStates(State state, int maxY, int maxX)
        {
            var newStates = new List<State>();

            var newPos = new Position(state.CurrentPosition.X, state.CurrentPosition.Y - 1);
            newStates.AddRange(GetStateFromClosestViable(state, maxY, maxX, newPos));

            newPos = new Position(state.CurrentPosition.X - 1, state.CurrentPosition.Y);
            newStates.AddRange(GetStateFromClosestViable(state, maxY, maxX, newPos));

            newPos = new Position(state.CurrentPosition.X + 1, state.CurrentPosition.X);
            newStates.AddRange(GetStateFromClosestViable(state, maxY, maxX, newPos));

            newPos = new Position(state.CurrentPosition.X, state.CurrentPosition.Y + 1);
            newStates.AddRange(GetStateFromClosestViable(state, maxY, maxX, newPos));

            //if (newStates.Count != 0)
            //{
            //    return newStates;
            //}

            newPos = new Position(state.ClosestViablePosition.X, state.ClosestViablePosition.Y - 1);
            newStates.AddRange(GetStateFromNewViable(state, maxY, maxX, newPos));

            newPos = new Position(state.ClosestViablePosition.X - 1, state.ClosestViablePosition.Y);
            newStates.AddRange(GetStateFromNewViable(state, maxY, maxX, newPos));

            newPos = new Position(state.ClosestViablePosition.X + 1, state.ClosestViablePosition.Y);
            newStates.AddRange(GetStateFromNewViable(state, maxY, maxX, newPos));

            newPos = new Position(state.ClosestViablePosition.X, state.ClosestViablePosition.Y + 1);
            newStates.AddRange(GetStateFromNewViable(state, maxY, maxX, newPos));

            return newStates;
        }

        private static List<State> GetStateFromClosestViable(State state, int maxY, int maxX, Position newPos)
        {
            var grid = state.Grid;
            
            var newStates = new List<State>();
            if (newPos.Y >= 0 && newPos.Y < maxY && newPos.X >= 0 && newPos.X < maxX &&
                IsViable(grid[state.CurrentPosition.X, state.CurrentPosition.Y], grid[newPos.X, newPos.Y]))
            {
                var newGrid = new Node[maxX, maxY];
                for (int i = 0; i < maxX; i++)
                {
                    for (int j = 0; j < maxY; j++)
                    {
                        newGrid[i, j] = new Node(grid[i, j].Position, grid[i, j].Size, grid[i, j].Used, grid[i, j].Percentage);
                    }
                }

                newGrid[newPos.X, newPos.Y].Used += newGrid[state.CurrentPosition.X, state.CurrentPosition.Y].Used;
                newGrid[state.CurrentPosition.X, state.CurrentPosition.Y].Used = 0;

                var closestViablePosition = GetClosestViablePosition(newGrid, maxY, maxX, newPos);
                
                newStates.Add(new State(newGrid, newPos, closestViablePosition, state.Steps + 1));
            }

            return newStates;
        }

        private static List<State> GetStateFromNewViable(State state, int maxY, int maxX, Position newPos)
        {
            var grid = state.Grid;
            
            var newStates = new List<State>();
            if (newPos.Y >= 0 && newPos.Y < maxY && newPos.X >= 0 && newPos.X < maxX &&
                !(newPos.X == state.CurrentPosition.X && newPos.Y == state.CurrentPosition.Y) &&
                IsViable(grid[newPos.X, newPos.Y], grid[state.ClosestViablePosition.X, state.ClosestViablePosition.Y]))
            {
                var newGrid = new Node[maxX, maxY];
                for (int i = 0; i < maxX; i++)
                {
                    for (int j = 0; j < maxY; j++)
                    {
                        newGrid[i, j] = new Node(grid[i, j].Position, grid[i, j].Size, grid[i, j].Used, grid[i, j].Percentage);
                    }
                }

                newGrid[state.ClosestViablePosition.X, state.ClosestViablePosition.Y].Used += newGrid[newPos.X, newPos.Y].Used;
                newGrid[newPos.X, newPos.Y].Used = 0;

                var closestViablePosition = GetClosestViablePosition(newGrid, maxY, maxX, state.CurrentPosition);
                
                newStates.Add(new State(newGrid, state.CurrentPosition, closestViablePosition, state.Steps + 1));
            }

            return newStates;
        }

        private static bool IsViable(Node node1, Node node2)
        {
            return node1.Used != 0 && node1.Position != node2.Position && node1.Used <= node2.Available;
        }

        private static Position GetClosestViablePosition(Node[,] grid, int maxY, int maxX, Position currentPosition)
        {
            for (int i = 0; i < maxX; i++)
            {
                for (int j = 0; j < maxY; j++)
                {
                    if (grid[i, j].Used == 0)
                    {
                        return new Position(i, j);
                    }
                }
            }

            throw new InvalidOperationException();
        }

        //private static Position GetClosestViablePosition(Node[,] grid, int maxY, int maxX, Position currentPosition)
        //{
        //    var min = int.MaxValue;
        //    var pos = new Position(0, 0);
        //    for (int i = 0; i < maxY; i++)
        //    {
        //        for (int j = 0; j < maxX; j++)
        //        {
        //            if (IsViable(grid[i, j], grid[currentPosition.Y, currentPosition.X]) && Math.Abs(i  - currentPosition.Y) + Math.Abs(j - currentPosition.X) < min)
        //            {
        //                pos = new Position(i, j);
        //            }
        //        }
        //    }

        //    return pos;
        //}

        private static void Output(Node[,] grid, int maxY, int maxX, Position currentPosition)
        {
            for (int i = 0; i < maxX; i++)
            {
                for (int j = 0; j < maxY; j++)
                {
                    if (j == currentPosition.X && i == currentPosition.Y)
                    {
                        Console.Write("#" + grid[j, i] + "#" + "\t");
                    }
                    else
                    {
                        Console.Write(grid[j, i] + "\t");
                    }
                }

                Console.WriteLine();
            }

            Console.WriteLine();
        }
    }
}

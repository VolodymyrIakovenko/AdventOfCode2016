using System;
using System.Collections.Generic;

namespace AdventOfCodeDay24
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

        public string GetHashString()
        {
            return "|" + X + "," + Y + "|";
        }
    }

    public class State
    {
        public Position CurrentPosition;
        public string NotVisitedPositions;
        public int Steps;

        public State(Position currentPosition, string notVisitedPositions, int steps)
        {
            CurrentPosition = currentPosition;
            NotVisitedPositions = notVisitedPositions;
            Steps = steps;
        }

        public bool IsSuccess()
        {
            return NotVisitedPositions == string.Empty;
        }

        public override string ToString()
        {
            return CurrentPosition + "; " + NotVisitedPositions;
        }
    }

    class Program
    {
        public static int rows;
        public static int columns;
        public static Dictionary<string, string> visitedPositions = new Dictionary<string, string>();

        public static void Main(string[] args)
        {
            var text = System.IO.File.ReadAllText(@"..\..\..\Input\Day24.txt");
            var inputArray = text.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            rows = inputArray.Length;
            columns = inputArray[0].Length;

            var startPosition = new Position(0, 0);
            var notVisitedPositions = string.Empty;

            var map = new char[rows, columns];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    if (inputArray[i][j] == '0')
                    {
                        startPosition = new Position(i, j);
                    }
                    else if (inputArray[i][j] != '#' && inputArray[i][j] != '.')
                    {
                        notVisitedPositions += (new Position(i, j)).GetHashString();
                    }

                    map[i, j] = inputArray[i][j];
                }
            }

            // Output(map);

            var currentState = new State(startPosition, notVisitedPositions, 0);
            var stateAgenda = new Queue<State>();
            stateAgenda.Enqueue(currentState);

            var stepsCounter = 0;

            while (stateAgenda.Count != 0)
            {
                currentState = stateAgenda.Dequeue();
                var currentPositionHash = currentState.CurrentPosition.GetHashString();

                if (visitedPositions.ContainsKey(currentState.NotVisitedPositions))
                {
                    if (visitedPositions[currentState.NotVisitedPositions].Contains(currentPositionHash))
                    {
                        continue;
                    }

                    visitedPositions[currentState.NotVisitedPositions] += currentPositionHash;
                }
                else
                {
                    visitedPositions.Add(currentState.NotVisitedPositions, currentPositionHash);
                }

                if (currentState.Steps > stepsCounter)
                {
                    stepsCounter++;
                    //Console.WriteLine(stepsCounter);
                }

                if (currentState.IsSuccess())
                {
                    visitedPositions.Remove(startPosition.GetHashString());
                    visitedPositions.Remove("");
                    var stepsToStart = GetStepsToTheStart(map, currentState, startPosition);
                    if (stepsToStart != -1)
                    {
                        Console.WriteLine("!!! " + (currentState.Steps + stepsToStart));
                    }

                    continue;
                }

                var nextPositions = GetNextPositions(map, currentState.CurrentPosition, false);
                foreach (var nextPosition in nextPositions)
                {
                    var newNotVisitedPositions = currentState.NotVisitedPositions.Replace(nextPosition.GetHashString(), "");
                    stateAgenda.Enqueue(new State(nextPosition, newNotVisitedPositions, currentState.Steps + 1));
                }
            }

            Console.ReadKey();
        }

        public static int GetStepsToTheStart(char[,] map, State state, Position startPosition)
        {
            var currentState = new State(state.CurrentPosition, startPosition.GetHashString(), 0);
            var stateAgenda = new Queue<State>();
            stateAgenda.Enqueue(currentState);

            while (stateAgenda.Count != 0)
            {
                currentState = stateAgenda.Dequeue();
                var currentPositionHash = currentState.CurrentPosition.GetHashString();

                if (visitedPositions.ContainsKey(currentState.NotVisitedPositions))
                {
                    if (visitedPositions[currentState.NotVisitedPositions].Contains(currentPositionHash))
                    {
                        continue;
                    }

                    visitedPositions[currentState.NotVisitedPositions] += currentPositionHash;
                }
                else
                {
                    visitedPositions.Add(currentState.NotVisitedPositions, currentPositionHash);
                }

                if (currentState.IsSuccess())
                {
                    return currentState.Steps;
                }

                var nextPositions = GetNextPositions(map, currentState.CurrentPosition, false);
                foreach (var nextPosition in nextPositions)
                {
                    var newNotVisitedPositions = currentState.NotVisitedPositions.Replace(nextPosition.GetHashString(), "");
                    stateAgenda.Enqueue(new State(nextPosition, newNotVisitedPositions, currentState.Steps + 1));
                }
            }

            return -1;
        }

        public static List<Position> GetNextPositions(char[,] map, Position position, bool filterStartPosition)
        {
            var nextPositions = new List<Position>();

            var possiblePosition = new Position(position.X + 1, position.Y);
            if (IsValidPosition(map, possiblePosition, filterStartPosition))
            {
                nextPositions.Add(possiblePosition);
            }

            possiblePosition = new Position(position.X, position.Y + 1);
            if (IsValidPosition(map, possiblePosition, filterStartPosition))
            {
                nextPositions.Add(possiblePosition);
            }

            possiblePosition = new Position(position.X - 1, position.Y);
            if (IsValidPosition(map, possiblePosition, filterStartPosition))
            {
                nextPositions.Add(possiblePosition);
            }

            possiblePosition = new Position(position.X, position.Y - 1);
            if (IsValidPosition(map, possiblePosition, filterStartPosition))
            {
                nextPositions.Add(possiblePosition);
            }

            return nextPositions;
        }

        public static bool IsValidPosition(char[,] map, Position possiblePosition, bool filterStartPosition)
        {
            if (!filterStartPosition)
            {
                return possiblePosition.X >= 0 && possiblePosition.X < rows && possiblePosition.Y >= 0 && possiblePosition.Y < columns &&
                    map[possiblePosition.X, possiblePosition.Y] != '#';
            }
            else
            {
                return possiblePosition.X >= 0 && possiblePosition.X < rows && possiblePosition.Y >= 0 && possiblePosition.Y < columns &&
                map[possiblePosition.X, possiblePosition.Y] != '#' && map[possiblePosition.X, possiblePosition.Y] != '0';
            }
        }

        public static void Output(char[,] map)
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    Console.Write(map[i, j]);
                }

                Console.WriteLine();
            }
        }
    }
}

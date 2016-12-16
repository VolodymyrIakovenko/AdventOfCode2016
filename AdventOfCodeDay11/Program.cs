using System;

namespace AdventOfCodeDay11
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Text;

    class Program
    {
        private class State
        {
            public Dictionary<int, List<char>> Floors;
            public int StepsDone;
            public int CurrentFloor;
            public string Hash;

            public State(Dictionary<int, List<char>> floors, int currentFloor, int stepsDone)
            {
                Floors = floors;
                CurrentFloor = currentFloor;
                StepsDone = stepsDone;
                Hash = GetHash();
            }

            public List<State> GetNextStates()
            {
                var nextSteps = new List<State>();

                var itemsCount = nextSteps.Count;
                
                // Get states to go up.
                if (CurrentFloor + 1 <= 4)
                {

                    var combinations = GetCombinations(Floors[CurrentFloor]);
                    foreach (var combination in combinations)
                    {
                        var newFloor = new List<char>(Floors[CurrentFloor + 1]);
                        newFloor.AddRange(combination.ToList());

                        var newCurentFloor = new List<char>(Floors[CurrentFloor]);
                        foreach (var item in combination)
                        {
                            newCurentFloor.Remove(item);
                        }

                        if (IsFloorValid(newFloor) && IsFloorValid(newCurentFloor))
                        {
                            var newFloors = new Dictionary<int, List<char>>();
                            foreach (var floor in Floors)
                            {
                                newFloors.Add(floor.Key, new List<char>(floor.Value));
                            }

                            foreach (var item in combination)
                            {
                                newFloors[CurrentFloor].Remove(item);
                            }

                            newFloors[CurrentFloor + 1].AddRange(combination.ToList());

                            var state = new State(newFloors, CurrentFloor + 1, StepsDone + 1);
                            nextSteps.Add(state);
                        }
                    }                    

                    ////if (nextSteps.Count == itemsCount)
                    //{
                    //    foreach (var item in Floors[CurrentFloor].Where(i => char.IsLower(i)))
                    //    {
                    //        var newFloor = new List<char>(Floors[CurrentFloor + 1]);
                    //        newFloor.Add(item);

                        //        var newCurentFloor = new List<char>(Floors[CurrentFloor]);
                        //        newCurentFloor.Remove(item);

                        //        if (IsFloorValid(newFloor) && IsFloorValid(newCurentFloor))
                        //        {
                        //            var newFloors = new Dictionary<int, List<char>>();
                        //            foreach (var floor in Floors)
                        //            {
                        //                newFloors.Add(floor.Key, new List<char>(floor.Value));
                        //            }

                        //            newFloors[CurrentFloor].Remove(item);
                        //            newFloors[CurrentFloor + 1].Add(item);

                        //                var state = new State(newFloors, CurrentFloor + 1, StepsDone + 1);
                        //                nextSteps.Add(state);
                        //        }
                        //    }
                        //}
                }

                // Get states to go down.
                if (CurrentFloor - 1 >= 1)
                {
                    var combinations = GetCombinations(Floors[CurrentFloor]);
                    foreach (var combination in combinations)
                    {
                        var newFloor = new List<char>(Floors[CurrentFloor - 1]);
                        newFloor.AddRange(combination.ToList());

                        var newCurentFloor = new List<char>(Floors[CurrentFloor]);
                        foreach (var item in combination)
                        {
                            newCurentFloor.Remove(item);
                        }

                        if (IsFloorValid(newFloor) && IsFloorValid(newCurentFloor))
                        {
                            var newFloors = new Dictionary<int, List<char>>();
                            foreach (var floor in Floors)
                            {
                                newFloors.Add(floor.Key, new List<char>(floor.Value));
                            }

                            foreach (var item in combination)
                            {
                                newFloors[CurrentFloor].Remove(item);
                            }

                            newFloors[CurrentFloor - 1].AddRange(combination.ToList());

                            var state = new State(newFloors, CurrentFloor - 1, StepsDone + 1);
                            nextSteps.Add(state);
                        }
                    }
                }

                //var itemsDown = 0;
                //{
                //    for (int i = CurrentFloor; i >= 1; i--)
                //    {
                //        itemsDown += Floors[i].Count;
                //    }
                //}

                ////if (itemsDown > 0)
                //{
                //    itemsCount = nextSteps.Count;

                //    // Get states to go down.
                //    if (CurrentFloor - 1 >= 1)
                //    {
                //        foreach (var item in Floors[CurrentFloor].Where(i => char.IsLower(i)))
                //        {
                //            var newFloor = new List<char>(Floors[CurrentFloor - 1]);
                //            newFloor.Add(item);

                //            var newCurentFloor = new List<char>(Floors[CurrentFloor]);
                //            newCurentFloor.Remove(item);

                //            if (IsFloorValid(newFloor) && IsFloorValid(newCurentFloor))
                //            {
                //                var newFloors = new Dictionary<int, List<char>>();
                //                foreach (var floor in Floors)
                //                {
                //                    newFloors.Add(floor.Key, new List<char>(floor.Value));
                //                }

                //                newFloors[CurrentFloor].Remove(item);
                //                newFloors[CurrentFloor - 1].Add(item);

                //                    var state = new State(newFloors, CurrentFloor - 1, StepsDone + 1);
                //                    nextSteps.Add(state);
                //            }
                //        }

                //        //if (nextSteps.Count == itemsCount)
                //        {
                //            var pairs = GetCombinations(Floors[CurrentFloor]);
                //            foreach (var pair in pairs)
                //            {
                //                var newFloor = new List<char>(Floors[CurrentFloor - 1]);
                //                newFloor.AddRange(pair.ToList());

                //                var newCurentFloor = new List<char>(Floors[CurrentFloor]);
                //                newCurentFloor.Remove(pair[0]);
                //                newCurentFloor.Remove(pair[1]);

                //                if (IsFloorValid(newFloor) && IsFloorValid(newCurentFloor))
                //                {
                //                    var newFloors = new Dictionary<int, List<char>>();
                //                    foreach (var floor in Floors)
                //                    {
                //                        newFloors.Add(floor.Key, new List<char>(floor.Value));
                //                    }

                //                    newFloors[CurrentFloor].Remove(pair[0]);
                //                    newFloors[CurrentFloor].Remove(pair[1]);
                //                    newFloors[CurrentFloor - 1].AddRange(pair.ToList());

                //                        var state = new State(newFloors, CurrentFloor - 1, StepsDone + 1);
                //                        nextSteps.Add(state);
                //                }
                //            }
                //        }
                //    }
                //}

                return nextSteps;
            }

            //private string GetHash()
            //{
            //    var hashString = new StringBuilder();
            //    foreach (var floor in Floors)
            //    {
            //        var lowerCase = 0;
            //        var upperCase = 0;
            //        foreach (var item in floor.Value)
            //        {
            //            if (char.IsLower(item))
            //            {
            //                lowerCase++;
            //            }
            //            else
            //            {
            //                upperCase++;
            //            }
            //        }

            //        hashString.Append(upperCase);
            //        hashString.Append(';');
            //        hashString.Append(lowerCase);
            //        hashString.Append('|');
            //    }

            //    return hashString.ToString();
            //}

            //private string GetHash()
            //{
            //    var hashString = new StringBuilder();
            //    var floorItems = new StringBuilder();
            //    foreach (var floor in Floors)
            //    {
            //        floorItems.Clear();
            //        foreach (var item in floor.Value)
            //        {
            //            floorItems.Append(item);
            //        }

            //        hashString.Append(Alphabetize(floorItems.ToString()));
            //        hashString.Append('|');
            //    }

            //    hashString.Append(CurrentFloor);

            //    return hashString.ToString();
            //}

            private string GetHash()
            {
                var nextChar = 'a';
                var charMaps = new Dictionary<char, char>();

                var hashString = new StringBuilder();
                var floorItems = new StringBuilder();
                foreach (var floor in Floors)
                {
                    floorItems.Clear();

                    foreach (var item in floor.Value)
                    {
                        var newChar = '-';
                        var lowerChar = char.ToLower(item);

                        if (!charMaps.ContainsKey(lowerChar))
                        {
                            charMaps.Add(lowerChar, nextChar++);
                        }

                        if (char.IsLower(item))
                        {
                            floorItems.Append(charMaps[lowerChar]);
                        }
                        else
                        {
                            floorItems.Append(char.ToUpper(charMaps[lowerChar]));
                        }
                    }

                    hashString.Append(Alphabetize(floorItems.ToString()));
                    hashString.Append('|');
                }

                hashString.Append(CurrentFloor);
                return hashString.ToString();
            }

            public static string Alphabetize(string s)
            {
                char[] a = s.ToCharArray();
                Array.Sort(a);
                return new string(a);
            }

            private static List<string> GetCombinations(List<char> floor)
            {
                var combinations = (from left in floor
                                    from right in floor
                                    where right > left
                                    select string.Format("{0}{1}", left, right)).ToList();
                foreach (var item in floor)
                {
                    combinations.Add(item.ToString());
                }

                return combinations;
            }

            static bool IsFloorValid(List<char> floor)
            {
                var itemsWithoutPairs = new List<char>();
                foreach (var item in floor)
                {
                    if ((char.IsUpper(item) && floor.Contains(char.ToLower(item))) ||
                        (char.IsLower(item) && floor.Contains(char.ToUpper(item))))
                    {

                    }
                    else
                    {
                        itemsWithoutPairs.Add(item);
                    }
                }

                // If there microchips without pair and other generators on the floor.
                if (itemsWithoutPairs.Any(i => char.IsLower(i)) && floor.Any(i => char.IsUpper(i)))
                {
                    return false;
                }

                return true;
            }

            public bool IsSolution()
            {
                return Floors[4].Count == 14;
                //return Floors[4].Count == 6;
            }

            public void Output()
            {
                foreach (var floor in Floors.Reverse())
                {
                    Console.Write(floor.Key + ":\t");
                    foreach (var item in floor.Value)
                    {
                        Console.Write(item + "\t");
                    }

                    Console.WriteLine();
                }

                Console.WriteLine();
                Console.WriteLine(CurrentFloor + "\t" + StepsDone);
                Console.WriteLine();
                //Console.ReadKey();
            }

            public override string ToString()
            {
                return Hash;
            }
        }


        static void Main(string[] args)
        {
            var floors = new Dictionary<int, List<char>>();

            //floors.Add(1, new List<char> { 'a', 'b' });
            //floors.Add(2, new List<char> { 'A' });
            //floors.Add(3, new List<char> { 'B' });
            //floors.Add(4, new List<char> { });

            floors.Add(1, new List<char> { 'A', 'a', 'B', 'C', 'F', 'f', 'G', 'g' });
            floors.Add(2, new List<char> { 'b', 'c' });
            floors.Add(3, new List<char> { 'D', 'd', 'E', 'e' });
            //floors.Add(3, new List<char> {  });
            floors.Add(4, new List<char> { });

            State currentState = null;

            var stateAgenda = new Queue<State>();
            stateAgenda.Enqueue(new State(floors, 1, 0));

            var stopWatch = new Stopwatch();
            stopWatch.Start();

            var previousStates = new HashSet<string>();

            var stepsCounter = 0;
            while (stateAgenda.Count > 0)
            {
                
                currentState = stateAgenda.Dequeue();
                if (previousStates.Contains(currentState.Hash))
                {
                    continue;
                }

                previousStates.Add(currentState.Hash);

                if (currentState.StepsDone > stepsCounter)
                {
                    stepsCounter = currentState.StepsDone;
                    Console.WriteLine("Step: " + stepsCounter + "\t" + stateAgenda.Count + "\t" + stopWatch.ElapsedMilliseconds);
                    stopWatch.Restart();
                }

                if (currentState.IsSolution())
                {
                    currentState.Output();
                    Console.WriteLine("!!!!!!!!!!!!! " + currentState.StepsDone);
                    Console.WriteLine();
                    Console.ReadKey();
                }
                else
                {
                    var nextAgenda = currentState.GetNextStates();
                    foreach (var next in nextAgenda)
                    {
                        stateAgenda.Enqueue(next);
                    }
                   //stateAgenda = KeepTheBestSteps(stateAgenda, nextAgenda);
                }

            }

            Console.WriteLine("Done");

            Console.ReadKey();
        }

        //private static Queue<State> KeepTheBestSteps(Queue<State> existingSteps, List<State> newSteps)
        //{
        //    foreach(var existingStep in existingSteps)
        //    {
        //        var existingStepHash = State.GetHashCode(existingStep.Floors);

        //        for (int j = 0; j < newSteps.Count; j++)
        //        {
        //            var newStep = newSteps[j];
        //            if (newStep.CurrentFloor != existingStep.CurrentFloor)
        //            {
        //                continue;
        //            }

        //            if (!newStep.PreviousFloorsHashes.SequenceEqual(existingStep.PreviousFloorsHashes))
        //            {
        //                continue;
        //            }

        //            //if (newStep.PreviousFloorsHashes.Except(existingStep.PreviousFloorsHashes).Any())
        //            //{
        //            //    continue;
        //            //}

        //            var newStepHash = State.GetHashCode(existingStep.Floors);
        //            if (newStepHash.Equals(existingStepHash))
        //            {
        //                newSteps.RemoveAt(j);
        //                j--;
        //            }
        //        }
        //    }

        //    foreach (var step in newSteps)
        //    {
        //        existingSteps.Enqueue(step);
        //    }

        //    return existingSteps;
        //}


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCodeDay15
{
    class Program
    {
        static void Main(string[] args)
        {
            var discs = new List<Disc>();
            //discs.Add(new Disc(5, 4));
            //discs.Add(new Disc(2, 1));

            discs.Add(new Disc(13, 11));
            discs.Add(new Disc(5, 0));
            discs.Add(new Disc(17, 11));
            discs.Add(new Disc(3, 0));
            discs.Add(new Disc(7, 2));
            discs.Add(new Disc(19, 17));
            discs.Add(new Disc(11, 0));


            var startTime = 0;
            var currentTime = 0;
            var currentDisc = 0;

            while (true)
            {
                foreach (var disc in discs)
                {
                    disc.Reset();
                }

                currentTime = startTime;
                currentDisc = 0;

                foreach (var disc in discs)
                {
                    disc.Rotate(currentTime);
                }

                while (true)
                {
                    currentTime++;
                    foreach (var disc in discs)
                    {
                        disc.Rotate(1);
                    }

                    if (discs[currentDisc].IsEmpty())
                    {
                        currentDisc++;

                        if (currentDisc == discs.Count - 1)
                        {
                            Console.WriteLine(startTime + " " + discs[6].CurrentPosition);
                        }

                        if (currentDisc == discs.Count)
                        {
                            Console.WriteLine("!!! " + startTime);
                            Console.ReadKey();
                        }
                    }
                    else
                    {
                        break;
                    }
                }

                startTime++;
            }
        }
    }

    public class Disc
    {
        public int MaxPositions;
        public int CurrentPosition;

        private int defaultPosition; 

        public Disc(int maxPositions, int currentPosition)
        {
            MaxPositions = maxPositions;
            defaultPosition = CurrentPosition = currentPosition;
        }

        public void Rotate(int positions)
        {
            CurrentPosition += positions;

            CurrentPosition = CurrentPosition % MaxPositions;
        }

        public bool IsEmpty()
        {
            return CurrentPosition == 0;
        }

        public void Reset()
        {
            CurrentPosition = defaultPosition;
        }
    }
}
